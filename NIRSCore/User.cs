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
        private DateTime _dateOfBirth;

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
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                if (Changer)
                {
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
        public string DatabaseName { get; set; }

        /// <summary>
        /// Абсолютный путь к базе данных
        /// </summary>
        public string DatabasePath { get; set; }

        /// <summary>
        /// Название СУБД, под управлением которой находится база данных
        /// </summary>
        public string DBMSName { get; set; }

        /// <summary>
        /// Логин пользователя базы данных
        /// </summary>
        public string DatabaseLogin { get; set; }

        /// <summary>
        /// Пароль пользователя базы данных
        /// </summary>
        public string DatabasePassword { get; set; }

        /// <summary>
        /// Проверка подлинности
        /// </summary>
        public bool IntegratedSecurity { get; set; }

        /// <summary>
        /// Название провайдера данных
        /// </summary>
        public string DatabaseProviderName { get; set; }

        /// <summary>
        /// Интервалы между бекапами
        /// </summary>
        public TimeSpan BackupIntervals { get; set; }

        /// <summary>
        /// Последняя дата создания резервной копии
        /// </summary>
        public DateTime LastBackup { get; set; }

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
        public int MainColors { get; set; }

        /// <summary>
        /// Дополнительные цвета
        /// </summary>
        public int AdditionalColors { get; set; }
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
            Name = SurName = SecondName = Position = string.Empty;
            DateOfBirth = DateTime.MinValue;

            //Настройки подключения
            DatabaseName = DatabaseLogin = DatabasePassword = DatabasePath = DBMSName = DatabaseProviderName = string.Empty;
            IntegratedSecurity = true;
            BackupIntervals = TimeSpan.MinValue;
            LastBackup = DateTime.MinValue;

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
            MainColors = AdditionalColors = 0;
        }
    }
}