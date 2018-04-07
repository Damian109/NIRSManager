using System;
using NIRSCore;

namespace NIRSManagerClient.ViewModels.SettingsViewModels
{
    public sealed class MainSettingsViewModel : ViewModel
    {
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string SurName
        {
            get => NirsSystem.User.SurName;
            set => NirsSystem.User.SurName = value;
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string NName
        {
            get => NirsSystem.User.Name;
            set => NirsSystem.User.Name = value;
        }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        public string SecondName
        {
            get => NirsSystem.User.SecondName;
            set => NirsSystem.User.SecondName = value;
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth
        {
            get => NirsSystem.User.DateOfBirth;
            set => NirsSystem.User.DateOfBirth = value;
        }

        /// <summary>
        /// Должность пользователя
        /// </summary>
        public string Position
        {
            get => NirsSystem.User.Position;
            set => NirsSystem.User.Position = value;
        }

        public MainSettingsViewModel() : base("Главная форма") { }
    }
}
