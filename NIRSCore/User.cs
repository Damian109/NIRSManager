using System;
using System.Windows.Media;

namespace NIRSCore
{
    /// <summary>
    /// Класс, который ответственен за хранение информации о пользователе
    /// </summary>
    [Serializable]
    public sealed class User
    {
        #region SettingsChanger

        //Событие изменения ФИО
        public delegate void eventSender();
        public event eventSender ChangeFIOEvent;

        private string _surName, _name, _secondName;
        private string _position;

        private string _databaseName, _databaseLogin, _databasePassword, _databasePath, _dBMSName;
        private string _databaseProviderName, _connectionString;
        private bool _integratedSecurity;
        private TimeSpan _backupIntervals;
        private DateTime _lastBackup;

        #endregion
        #region MainPropertyes
        /// <summary>
        /// Показывает был ли документ уже загружен
        /// </summary>
        public bool Changer { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string SurName
        {
            get => _surName;
            set
            {
                _surName = value;
                if(Changer)
                {
                    ChangeFIOEvent?.Invoke();
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (Changer)
                {
                    ChangeFIOEvent?.Invoke();
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        public string SecondName
        {
            get => _secondName;
            set
            {
                _secondName = value;
                if (Changer)
                {
                    ChangeFIOEvent?.Invoke();
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Должность пользователя
        /// </summary>
        public string Position {
            get => _position;
            set
            {
                _position = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }
        #endregion
        #region ConnectionPropertyes
        /// <summary>
        /// Название базы данных, представленное в папке пользователя
        /// </summary>
        public string DatabaseName
        {
            get => _databaseName;
            set
            {
                _databaseName = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Абсолютный путь к базе данных
        /// </summary>
        public string DatabasePath
        {
            get => _databasePath;
            set
            {
                _databasePath = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Название СУБД, под управлением которой находится база данных
        /// </summary>
        public string DBMSName
        {
            get => _dBMSName;
            set
            {
                _dBMSName = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Логин пользователя базы данных
        /// </summary>
        public string DatabaseLogin
        {
            get => _databaseLogin;
            set
            {
                _databaseLogin = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Пароль пользователя базы данных
        /// </summary>
        public string DatabasePassword
        {
            get => _databasePassword;
            set
            {
                _databasePassword = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Проверка подлинности
        /// </summary>
        public bool IntegratedSecurity
        {
            get => _integratedSecurity;
            set
            {
                _integratedSecurity = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Название провайдера данных
        /// </summary>
        public string DatabaseProviderName
        {
            get => _databaseProviderName;
            set
            {
                _databaseProviderName = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Интервалы между бекапами
        /// </summary>
        public TimeSpan BackupIntervals
        {
            get => _backupIntervals;
            set
            {
                _backupIntervals = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Последняя дата создания резервной копии
        /// </summary>
        public DateTime LastBackup
        {
            get => _lastBackup;
            set
            {
                _lastBackup = value;
                if (Changer)
                {
                    DateLastEditSettings = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        #endregion
        #region ServerPropertyes
        /// <summary>
        /// Подключаться ли к серверу или работать автономно?
        /// </summary>
        public bool IsConnectToServer { get; set; }

        /// <summary>
        /// Логин для подключения
        /// </summary>
        public string LoginToServer { get; set; }

        /// <summary>
        /// Пароль для подключения
        /// </summary>
        public string PasswordToServer { get; set; }

        /// <summary>
        /// Синхронизировать ли настройки с сервером?
        /// </summary>
        public bool IsSynchronizeSettingsWithServer { get; set; }

        /// <summary>
        /// Синхронизировать ли текущую основную БД с сервером?
        /// </summary>
        public bool IsSynchronizeDatabaseWithServer { get; set; }

        /// <summary>
        /// Хранить резервные копии на сервере?
        /// </summary>
        public bool IsSynchronizeBackupWithServer { get; set; }

        /// <summary>
        /// Хранить копии документов на сервере?
        /// </summary>
        public bool IsSynchronizeDocumentsWithServer { get; set; }
        #endregion
        #region ProgramPropertyes
        /// <summary>
        /// Дата последнего изменения настроек
        /// </summary>
        public DateTime DateLastEditSettings { get; set; }

        /// <summary>
        /// Дата последнего изменения базы данных
        /// </summary>
        public DateTime DateLastEditDatabase { get; set; }

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
        #endregion
        #region InterfacePropertyes
        /// <summary>
        /// Тема приложения
        /// </summary>
        public bool IsDarkTheme { get; set; }

        /// <summary>
        /// Позиция основного меню
        /// </summary>
        public bool IsLeftPosition { get; set; }

        /// <summary>
        /// Основные цвета
        /// </summary>
        public string MainColors { get; set; }

        /// <summary>
        /// Дополнительные цвета
        /// </summary>
        public string AdditionalColors { get; set; }
        #endregion
        #region EditorPropertyes
        /// <summary>
        /// Цвет чисел в редакторе
        /// </summary>
        public Color EditorNumberColor { get; set; }

        /// <summary>
        /// Цвет строк в редакторе
        /// </summary>
        public Color EditorStringColor { get; set; }

        /// <summary>
        /// Цвет Ключевых слов в редакторе
        /// </summary>
        public Color EditorKeywordColor { get; set; }

        /// <summary>
        /// Включен ли лексический анализатор
        /// </summary>
        public bool EditorIsLexic { get; set; }

        /// <summary>
        /// Включен ли синтаксический анализатор
        /// </summary>
        public bool EditorIsSyntax { get; set; }

        /// <summary>
        /// Включено ли автодополнение
        /// </summary>
        public bool EditorIsAddition { get; set; }
        #endregion

        /// <summary>
        /// Пустой конструктор класса
        /// </summary>
        public User()
        {
            Changer = false;
            //Основные настройки
            _name = _surName = _secondName = _position = string.Empty;

            //Настройки подключения
            _databaseName = _databaseLogin = _databasePassword = _databasePath = _dBMSName = _databaseProviderName = string.Empty;
            _connectionString = string.Empty;
            _integratedSecurity = true;
            _backupIntervals = TimeSpan.MinValue;
            _lastBackup = DateTime.MinValue;

            //Настройки синхронизации
            IsConnectToServer = IsSynchronizeSettingsWithServer = IsSynchronizeDatabaseWithServer = false;
            IsSynchronizeBackupWithServer = IsSynchronizeDocumentsWithServer = false;
            LoginToServer = PasswordToServer = string.Empty;

            //Настройки приложения
            DateLastEditSettings = DateLastEditDatabase = DateTime.MinValue;
            IsStartFromWindows = IsMinimizeToTray = IsShowNotifications = false;

            //Настройки редактора
            EditorIsAddition = EditorIsLexic = EditorIsSyntax = true;
            EditorKeywordColor = Colors.Blue;
            EditorNumberColor = Colors.OrangeRed;
            EditorStringColor = Colors.SandyBrown;

            //Настройки интерфейса
            IsDarkTheme = IsLeftPosition = true;
            MainColors = AdditionalColors = string.Empty;
        }
    }
}