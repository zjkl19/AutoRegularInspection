using System;
using System.Windows;
using NLog;

namespace AutoRegularInspection.Services
{
    /// <summary>
    /// 统一的用户提示与日志记录助手。
    /// </summary>
    public static class UserNotification
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Info(string message, string title = "提示")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            Logger.Info(message);
        }

        public static void Warn(string message, string title = "警告")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
            Logger.Warn(message);
        }

        public static void Error(string message, Exception ex = null, string title = "错误")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            if (ex != null)
            {
                Logger.Error(ex, message);
            }
            else
            {
                Logger.Error(message);
            }
        }
    }
}
