﻿using NIRSCore;

namespace NIRSManagerClient.ViewModels.SettingsViewModels
{
    public sealed class ProgramSettingsViewModel : ViewModel
    {
        public ProgramSettingsViewModel() : base("Главная форма") { }

        /// <summary>
        /// Адрес сервера
        /// </summary>
        public string ServerString
        {
            get => NirsSystem.ProgramSettings.AdressServer;
            set => NirsSystem.ProgramSettings.AdressServer = value;
        }

        /// <summary>
        /// Запуск вместе с ОС
        /// </summary>
        public bool IsStartFromWindows
        {
            get => NirsSystem.ProgramSettings.IsStartFromWindows;
            set => NirsSystem.ProgramSettings.IsStartFromWindows = value;
        }

        /// <summary>
        /// Скрывать в трей
        /// </summary>
        public bool IsMinimizeToTray
        {
            get => NirsSystem.ProgramSettings.IsMinimizeToTray;
            set => NirsSystem.ProgramSettings.IsMinimizeToTray = value;
        }

        /// <summary>
        /// Показывать уведомления
        /// </summary>
        public bool IsShowNotifications
        {
            get => NirsSystem.ProgramSettings.IsShowNotifications;
            set => NirsSystem.ProgramSettings.IsShowNotifications = value;
        }

        /// <summary>
        /// Сделать пользователем по умолчанию
        /// </summary>
        public bool IsMainUser
        {
            get => NirsSystem.IsMainUser;
            set
            {
                if (value)
                    NirsSystem.SetMainUser();
                else
                    NirsSystem.DeleteMainUser();
            }
        }
    }
}
