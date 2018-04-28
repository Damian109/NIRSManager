using System;
using NIRSCore;
using System.ServiceProcess;
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
                if(selection != null)
                    dbsNames.Add(selection);
            }
            dbsNames.Add(new DatabaseSelection("SQLite", "System.Data.SQLite"));
            return dbsNames;
        }

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
                    return new DatabaseSelection("MS SQL Express", "System.Data.SqlClient");
                case "MySQL":
                    return new DatabaseSelection("MySQL", "MySql.Data.MySqlClient");
                default:
                    return null;
            }
        }

        /// <summary>
        /// Формирование строки подключения
        /// </summary>
        /// <returns></returns>
        private string FormConnectionString()
        {
            string str = @"Data Source=.\data\\" + NirsSystem.User.DatabaseLogin + "\\Database" + ";Integrated Security = " +
                NirsSystem.User.IntegratedSecurity.ToString();
            return str;
        }

        private DatabaseSelection _selectedDBsNames;

        #endregion
        /// <summary>
        /// Дата последнего сделанного бэкапа
        /// </summary>
        public DateTime LastBackup
        {
            get => NirsSystem.User.LastBackup;
        }

        /// <summary>
        /// Логин для подключения к БД
        /// </summary>
        public string Login
        {
            get => NirsSystem.User.DatabaseLogin;
            set => NirsSystem.User.DatabaseLogin = value;
        }

        /// <summary>
        /// Пароль для подключения к БД
        /// </summary>
        public string Password
        {
            get => NirsSystem.User.DatabasePassword;
            set => NirsSystem.User.DatabasePassword = value;
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
        public TimeSpan BackupIntervals
        {
            get => NirsSystem.User.BackupIntervals;
            set => NirsSystem.User.BackupIntervals = value;
        }

        /// <summary>
        /// Список доступных СУБД
        /// </summary>
        public List<DatabaseSelection> DBSNames { get => GetServices(); }

        public DatabaseSelection SelectedDBsNames
        {
            get => _selectedDBsNames;
            set
            {
                _selectedDBsNames = value;
                DatabaseProviderName = _selectedDBsNames.ProviderName;
                DBMSName = _selectedDBsNames.Name;
            }
        }

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

        public ConnectionSettingsViewModel() : base("Главная форма") { }

    }
}
