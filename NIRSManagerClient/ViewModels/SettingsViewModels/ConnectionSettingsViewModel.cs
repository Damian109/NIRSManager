using System;
using NIRSCore;
using System.ServiceProcess;
using NIRSCore.StackOperations;
using System.Collections.Generic;
using NIRSManagerClient.HelpfulModels;

namespace NIRSManagerClient.ViewModels.SettingsViewModels
{
    /// <summary>
    /// Модель представления настроек соединения
    /// </summary>
    public sealed class ConnectionSettingsViewModel : ViewModel
    {
        #region Private
        /// <summary>
        /// Возвращает список доступных пользователю СУБД
        /// </summary>
        /// <returns></returns>
        private List<DatabaseSelection> GetServices()
        {
            List<DatabaseSelection> dbsNames = new List<DatabaseSelection>();
            IEnumerable<ServiceController> controllers = ServiceController.GetServices();

            foreach (var elem in controllers)
            {
                DatabaseSelection selection = IsTrueService(elem);
                if (selection != null)
                    dbsNames.Add(selection);
            }
            dbsNames.Add(new DatabaseSelection("SQLite", "System.Data.SQLite"));
            return dbsNames;
        }

        private string _dbmsNameNext;
        private string _databaseProviderNameNext;
        private string _loginNext;
        private string _passwordNext;
        private int _backupIntervalsNext;
        private bool _integratedSecurityNext;

        /// <summary>
        /// Определение - соответствует ли служба одной из СУБД
        /// </summary>
        /// <param name="controller">Служба Windows</param>
        /// <returns></returns>
        private DatabaseSelection IsTrueService(ServiceController controller)
        {
            switch (controller.ServiceName)
            {
                case "MSSQL$SQLEXPRESS":
                    return new DatabaseSelection("MS SQL Express", "System.Data.EntityClient");
                case "MySQL":
                    return new DatabaseSelection("MySQL", "MySql.Data.MySqlClient");
                default:
                    return null;
            }
        }

        private DatabaseSelection _selectedDBsNames;

        #endregion

        /// <summary>
        /// Имя СУБД
        /// </summary>
        public string DBMSName
        {
            get => NirsSystem.User.DBMSName;
            set
            {
                NirsSystem.User.DBMSName = value;
                OnPropertyChanged("DBMSName");
            }
        }

        /// <summary>
        /// Провайдер данных
        /// </summary>
        public string DatabaseProviderName
        {
            get => NirsSystem.User.DatabaseProviderName;
            set
            {
                NirsSystem.User.DatabaseProviderName = value;
                OnPropertyChanged("DatabaseProviderName");
            }
        }

        /// <summary>
        /// Логин для подключения к БД
        /// </summary>
        public string Login
        {
            get => NirsSystem.User.DatabaseLogin;
            set
            {
                NirsSystem.User.DatabaseLogin = value;
                OnPropertyChanged("Login");
            }
        }

        /// <summary>
        /// Пароль для подключения к БД
        /// </summary>
        public string Password
        {
            get => NirsSystem.User.DatabasePassword;
            set
            {
                NirsSystem.User.DatabasePassword = value;
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
        /// Проверка подлинности
        /// </summary>
        public bool IntegratedSecurity
        {
            get => NirsSystem.User.IntegratedSecurity;
            set
            {
                NirsSystem.User.IntegratedSecurity = value;
                OnPropertyChanged("IntegratedSecurity");
            }
        }

        /// <summary>
        /// Интервалы между созданием новой резервной копии
        /// </summary>
        public int BackupIntervals
        {
            get => NirsSystem.User.BackupIntervals;
            set
            {
                NirsSystem.User.BackupIntervals = value;
                OnPropertyChanged("BackupIntervals");
            }
        }

        /// <summary>
        /// Имя СУБД
        /// </summary>
        public string DBMSNameNext
        {
            get => _dbmsNameNext;
            set
            {
                _dbmsNameNext = value;
                OnPropertyChanged("DBMSNameNext");
            }
        }

        /// <summary>
        /// Провайдер данных
        /// </summary>
        public string DatabaseProviderNameNext
        {
            get => _databaseProviderNameNext;
            set
            {
                _databaseProviderNameNext = value;
                OnPropertyChanged("DatabaseProviderNameNext");
            }
        }

        /// <summary>
        /// Логин для подключения к БД
        /// </summary>
        public string LoginNext
        {
            get => _loginNext;
            set => _loginNext = value;
        }

        /// <summary>
        /// Пароль для подключения к БД
        /// </summary>
        public string PasswordNext
        {
            get => _passwordNext;
            set => _passwordNext = value;
        }

        /// <summary>
        /// Проверка подлинности
        /// </summary>
        public bool IntegratedSecurityNext
        {
            get => _integratedSecurityNext;
            set
            {
                _integratedSecurityNext = value;
                OnPropertyChanged("IntegratedSecurityNext");
            }
        }

        /// <summary>
        /// Интервалы между созданием новой резервной копии
        /// </summary>
        public int BackupIntervalsNext
        {
            get => _backupIntervalsNext;
            set => _backupIntervalsNext = value;
        }

        /// <summary>
        /// Список доступных СУБД
        /// </summary>
        public List<DatabaseSelection> DBSNames { get => GetServices(); }

        /// <summary>
        /// Выбранная СУБД
        /// </summary>
        public DatabaseSelection SelectedDBsNames
        {
            get => _selectedDBsNames;
            set
            {
                _selectedDBsNames = value;
                DatabaseProviderNameNext = _selectedDBsNames.ProviderName;
                DBMSNameNext = _selectedDBsNames.Name;
            }
        }

        /// <summary>
        /// Команда - сохранить настройки
        /// </summary>
        public RelayCommand CommandSave
        {
            get => new RelayCommand(obj =>
            {
                //Сохранение предыдущих результатов
                string prevDBSName = DBMSName;
                string prevDatabaseProviderName = DatabaseProviderName;
                string prevLogin = Login;
                string prevPassword = Password;
                bool prevIntegratedSecurity = IntegratedSecurity;
                int prevBackupIntervals = BackupIntervals;
                string prevConnectionString = NirsSystem.User.ConnectionString;

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone =>
                {
                    DBMSName = DBMSNameNext;
                    DatabaseProviderName = DatabaseProviderNameNext;
                    Login = LoginNext;
                    Password = PasswordNext;
                    IntegratedSecurity = IntegratedSecurityNext;
                    BackupIntervals = BackupIntervalsNext;

                    //Формирование пути к БД
                    string path = Environment.CurrentDirectory + "\\data\\" + NirsSystem.GetLogin() + "\\database";
                    if (NirsSystem.User.DBMSName == "MS SQL Express")
                        path += ".mdf";
                    else
                        path += ".db";

                    if (NirsSystem.User.DBMSName == "MS SQL Express")
                    {
                        NirsSystem.User.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" + path + "'; Integrated Security = " +
                            NirsSystem.User.IntegratedSecurity.ToString() + "; ";
                        NirsSystem.Close();
                    }
                });

                //Создание команды отмена операции
                //Создание команды выполнения операции
                RelayCommand undone = new RelayCommand(objUnDone =>
                {
                    DBMSName = prevDBSName;
                    DatabaseProviderName = prevDatabaseProviderName;
                    Login = prevLogin;
                    Password = prevPassword;
                    IntegratedSecurity = prevIntegratedSecurity;
                    BackupIntervals = prevBackupIntervals;
                    NirsSystem.User.ConnectionString = prevConnectionString;
                });

                //Создание операции
                Operation operation = new Operation("Изменение настроек подключения к БД", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        public ConnectionSettingsViewModel() : base("Главная форма") { }
    }
}