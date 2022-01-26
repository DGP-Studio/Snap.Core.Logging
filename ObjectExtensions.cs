using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Snap.Core.Logging
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="obj">记录日志的调用类</param>
        /// <param name="info">要记录入日志的信息</param>
        /// <param name="formatter">格式化输入</param>
        [Conditional("DEBUG")]
        public static void Log<T>(this T obj, object? info, Func<object?, string>? formatter = null,
            [CallerMemberName] string? callerMemberName = null,
            [CallerLineNumber] int? callerLineNumber = null,
            [CallerFilePath] string? callerFilePath = null)
        {
            Logger.Instance.LogInternal(info, formatter, callerMemberName, callerLineNumber, callerFilePath);
        }

        public static void WriteToDesktopFile<T>(this T obj, string? info,string fileName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.WriteAllText(Path.Combine(desktopPath, fileName), info);
        }
    }
}
