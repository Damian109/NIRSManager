using NIRSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSManagerClient.ViewModels.SettingsViewModels
{
    public sealed class MainSettingsViewModel : ViewModel
    {
        #region Private
        private User _user;
        #endregion

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string SurName
        {
            get => _user.SurName;
            set => _user.SurName = value;
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string NName
        {
            get => _user.Name;
            set => _user.Name = value;
        }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        public string SecondName
        {
            get => _user.SecondName;
            set => _user.SecondName = value;
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth
        {
            get => _user.DateOfBirth;
            set => _user.DateOfBirth = value;
        }

        /// <summary>
        /// Должность пользователя
        /// </summary>
        public string Position
        {
            get => _user.Position;
            set => _user.Position = value;
        }

        public MainSettingsViewModel(User user) : base("Главная форма") => _user = user;
    }
}
