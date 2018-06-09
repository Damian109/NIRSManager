using NIRSCore;
using NIRSCore.BackupManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSManagerClient.ViewModels
{
    /// <summary>
    /// Модель представления для страницы резервных копий
    /// </summary>
    public sealed class BackupViewModel : ViewModel
    {
        private List<BackupElem> _backups;
        private BackupElem _selectedBackup;
        private bool _isServerBackup;

        //Запрос всех бэкапов
        private async void GetBackups() => await Task.Run(() =>
        {
            _backups = NirsSystem.GetListBackups();
            IsCreateBackup = true;
            foreach (var elem in _backups)
                if (elem.DateOfCreate.Date == DateTime.Now.Date)
                    IsCreateBackup = false;
            if(!NirsSystem.IsDatabaseContextCreated)
                IsCreateBackup = false;
            OnPropertyChanged("IsCreateBackup");
            OnPropertyChanged("Backups");
            _isServerBackup = false;
        });

        public BackupViewModel() : base("Backups") => GetBackups();

        /// <summary>
        /// Список резервных копий
        /// </summary>
        public ObservableCollection<BackupElem> Backups
        {
            get
            {
                ObservableCollection<BackupElem> backups = new ObservableCollection<BackupElem>();
                if (_backups != null)
                    foreach (var elem in _backups)
                        backups.Add(elem);
                return backups;
            }
        }

        /// <summary>
        /// Выбранная резервная копия
        /// </summary>
        public BackupElem SelectedBackup
        {
            get => _selectedBackup;
            set
            {
                _selectedBackup = value;
                if(_selectedBackup != null)
                {
                    IsCreateDatabase = true;
                    OnPropertyChanged("IsCreateDatabase");
                }
                else
                {
                    IsCreateDatabase = false;
                    OnPropertyChanged("IsCreateDatabase");
                }
            }
        }

        /// <summary>
        /// Возможно ли восстановление
        /// </summary>
        public bool IsCreateDatabase { get; set; }

        /// <summary>
        /// Возможно ли создание резервной копии
        /// </summary>
        public bool IsCreateBackup { get; set; }

        /// <summary>
        /// Возможно ли получение резервных копий с сервера
        /// </summary>
        public bool IsGetBackup { get => NirsSystem.IsServer; }

        /// <summary>
        /// Команда создания резервной копии
        /// </summary>
        public RelayCommand CommandCreateBackup
        {
            get => new RelayCommand(obj =>
            {
                NirsSystem.CreateBackup();
                GetBackups();
            });
        }

        /// <summary>
        /// Команда восстановления резервной копии
        /// </summary>
        public RelayCommand CommandCreateDatabase
        {
            get => new RelayCommand(obj =>
            {
                NirsSystem.CreateDB(SelectedBackup.Name);
                GetBackups();
            });
        }



        //Запрос Бэкапов с сервера
    }
}
