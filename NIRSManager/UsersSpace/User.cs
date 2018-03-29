using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSManager.UsersSpace
{
    /// <summary>
    /// Класс, который ответственен за хранение информации о пользователе
    /// </summary>
    [DataContract]
    internal sealed class User
    {
        #region Private
        private string _name;
        private string _surname;
        private string _secondName;
        private DateTime _dateOfBirth;
        private string _position;
        #endregion
        #region Propertyes
        /// <summary>
        /// Дата рождения
        /// </summary>
        [DataMember]
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Должность пользователя
        /// </summary>
        [DataMember]
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        [DataMember]
        public string SecondName
        {
            get { return _secondName; }
            set { _secondName = value; }
        }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [DataMember]
        public string SurName
        {
            get { return _surname; }
            set { _surname = value; }
        }
        #endregion



        private bool _isConnectToServer;
        private string _loginConnectToServer;
        private string _passwordConnectToServer;
        private bool _isSynchronizeSettingsToServer;
        private bool _isSynchronizeDatabaseToServer;
        private bool _isSynchronizeBackupToServer;
        private bool _isSynchronizeDocumentsToServer;

        private bool IsConnectToServer
        {
            get { return _isConnectToServer; }
            set { _isConnectToServer = value; }
        }




        /// <summary>
        /// Пустой конструктор класса
        /// </summary>
        public User()
        {
            _name = _surname = _secondName = _position = string.Empty;
            _dateOfBirth = DateTime.MinValue;
        }

        

        /* -----Настройки для сервера-------
         * Подключаться ли к серверу или работать автономно?
         * Логин для подключения
         * Пароль для подключения
         * Синхронизировать ли настройки с сервером?
         * Синхронизировать ли текущую основную БД с сервером?
         * Хранить резервные копии на сервере?
         * Хранить копии документов на сервере?
         * 
         * -----Как узнать что что-то изменилось в последнее время.------
         * Дата последнего изменения настроек
         * 
         * -----Настройки работы программы------
         * Запускаться при запуске Windows? 
         * При закрытии сворачивать в трей?
         * Показывать уведомления?
         * 
         * 
         * -----Настройки редактора-----
         * 
         * 
         * -----Настройки подключения-----
         * СУБД
         * Имя пользователя
         * Пароль пользователя
         * Название базы данных
         * Строка подключения????????
         * 
         * -----Настройки интерфейса-----
         * */


    }
}
