using NIRSCore;
using System.Windows;
using System.Threading;

namespace NIRSManagerClient.ViewModels.SettingsViewModels
{
    /// <summary>
    /// Модель представления настроек синхронизации
    /// </summary>
    public sealed class ServerSettingsViewModel : ViewModel
    {
        public ServerSettingsViewModel() : base("Main") { }

        /// <summary>
        /// Синхронизировать ли с сервером
        /// </summary>
        public bool IsSyncServer
        {
            get => NirsSystem.User.IsConnectToServer;
            set
            {
                NirsSystem.User.IsConnectToServer = value;
                OnPropertyChanged("IsSyncServer");
            }
        }

        /// <summary>
        /// Синхронизировать ли настройки
        /// </summary>
        public bool IsSyncSettings
        {
            get => NirsSystem.User.IsSynchronizeSettingsWithServer;
            set => NirsSystem.User.IsSynchronizeSettingsWithServer = value;
        }

        /// <summary>
        /// Синхронизировать ли базу данных
        /// </summary>
        public bool IsSyncDatabase
        {
            get => NirsSystem.User.IsSynchronizeDatabaseWithServer;
            set => NirsSystem.User.IsSynchronizeDatabaseWithServer = value;
        }

        /// <summary>
        /// Хранить ли документы только на сервере
        /// </summary>
        public bool IsSyncDocuments
        {
            get => NirsSystem.User.IsSynchronizeDocumentsWithServer;
            set => NirsSystem.User.IsSynchronizeDocumentsWithServer = value;
        }

        /// <summary>
        /// Хранить ли резервные копии только на сервере
        /// </summary>
        public bool IsSyncBackups
        {
            get => NirsSystem.User.IsSynchronizeBackupWithServer;
            set => NirsSystem.User.IsSynchronizeBackupWithServer = value;
        }

        /// <summary>
        /// Выполняется ли какая-то операция (Видимость)
        /// </summary>
        public Visibility IsDone { get; private set; } = Visibility.Hidden;

        /// <summary>
        /// Статус выполнения операции
        /// </summary>
        public string StatusString { get; private set; }

        /// <summary>
        /// Команда синхронизации
        /// </summary>
        public RelayCommand CommandSync
        {
            get => new RelayCommand(obj =>
            {
                IsDone = Visibility.Visible;
                OnPropertyChanged("IsDone");
                StatusString = "Выполняется синхронизация";
                OnPropertyChanged("StatusString");
                int status = NirsSystem.Synchronization(true);
                switch(status)
                {
                    case 1:
                        StatusString = "Возможно отключена синхронизация. Включите и попробуйте снова";
                        break;
                    case 2:
                        StatusString = "Отсутствует соединение с сервером";
                        break;
                    case 3:
                        StatusString = "Данные для авторизации на сервере не совпадают";
                        break;
                    default:
                        StatusString = "Синхронизация выполнена успешна. Для загрузки настроек перезагрузите приложение";
                        break;
                }
                OnPropertyChanged("StatusString");
                Thread.Sleep(1000);
                IsDone = Visibility.Hidden;
                OnPropertyChanged("IsDone");
            });
        }
    }
}
