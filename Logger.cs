using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Snap.Core.Logging
{
    public class Logger
    {
        private string? lastCallerFilePath;
        private string? lastCallerMemberName;

        /// <summary>
        /// 核心日志方法
        /// </summary>
        /// <param name="info"></param>
        /// <param name="formatter"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="callerFilePath"></param>
        internal void LogInternal(object? info, Func<object?, string>? formatter = null,
            string? callerMemberName = null, int? callerLineNumber = null, string? callerFilePath = null)
        {
            //pre format string
            if (formatter != null)
            {
                info = formatter.Invoke(info);
            }
            //trim callerFilePath
            if (callerFilePath is not null)
            {
                int pos = callerFilePath.IndexOf("Snap.Genshin", StringComparison.Ordinal);
                callerFilePath = callerFilePath[pos..];
            }

            if (callerMemberName == lastCallerMemberName && callerFilePath == lastCallerFilePath)
            {
                Debug.WriteLine($"[Line:{callerLineNumber,6}] {info}");
            }
            else
            {
                string log = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | {callerFilePath} | {callerMemberName} |\n[Line:{callerLineNumber,6}] {info}";

                if (lastCallerFilePath != callerFilePath)
                {
                    Debug.Write('\n');
                }

                Debug.WriteLine(log);
            }

            lastCallerMemberName = callerMemberName;
            lastCallerFilePath = callerFilePath;
        }

        public static void LogStatic(object? info, Func<object?, string>? formatter = null,
            [CallerMemberName] string? callerMemberName = null,
            [CallerLineNumber] int? callerLineNumber = null,
            [CallerFilePath] string? callerFilePath = null)
        {
            Instance.LogInternal(info, formatter, callerMemberName, callerLineNumber, callerFilePath);
        }

        #region 单例
        private static volatile Logger? instance;
        [SuppressMessage("", "IDE0044")]
        private static object _locker = new();
        private Logger() { }
        public static Logger Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (_locker)
                    {
                        instance ??= new();
                    }
                }
                return instance;
            }
        }
        #endregion
    }
}

