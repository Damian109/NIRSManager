using System;

namespace NIRSCore
{
    /// <summary>
    /// Настройки программы
    /// </summary>
    [Serializable]
    public sealed class ProgramSettings
    {
        /// <summary>
        /// Адрес сервера
        /// </summary>
        public string AdressServer { get; set; }

        /// <summary>
        /// Запускаться при запуске Windows? 
        /// </summary>
        public bool IsStartFromWindows { get; set; }

        /// <summary>
        /// При закрытии сворачивать в трей?
        /// </summary>
        public bool IsMinimizeToTray { get; set; }

        /// <summary>
        /// Показывать уведомления?
        /// </summary>
        public bool IsShowNotifications { get; set; }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public ProgramSettings()
        {
            IsMinimizeToTray = IsShowNotifications = IsStartFromWindows = false;
            AdressServer = $"http://localhost:61096/";
        }
    }
}
