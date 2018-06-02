using NIRSCore;
using NIRSCore.ErrorManager;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NIRSManagerClient.ViewModels
{
    public sealed class ErrorsViewModel : ViewModel
    {
        #region Private
        /// <summary>
        /// Получение списка ошибок
        /// </summary>
        private void GetErrors()
        {
            _errors = NirsSystem.ErrorManager.GetErrors();
            OnPropertyChanged("Errors");
        }

        #endregion

        public ErrorsViewModel() : base("Главная форма") => GetErrors();

        private List<NirsError> _errors;

        /// <summary>
        /// Список ошибок
        /// </summary>
        public ObservableCollection<NirsError> Errors
        {
            get
            {
                ObservableCollection<NirsError> errors = new ObservableCollection<NirsError>();
                if (_errors != null)
                    foreach (var elem in _errors)
                        errors.Add(elem);
                return errors;
            }
        }

        /// <summary>
        /// Команда Отправить на сервер
        /// </summary>
        public RelayCommand CommandSetToServer
        {
            get => new RelayCommand(obj =>
            {
                NirsSystem.ErrorManager.SetToServer(NirsSystem.ProgramSettings.AdressServer);
                _errors.Clear();
                OnPropertyChanged("Errors");
            });
        }

        /// <summary>
        /// Команда Очистить
        /// </summary>
        public RelayCommand CommandClear
        {
            get => new RelayCommand(obj =>
            {
                NirsSystem.ErrorManager.Clear();
                _errors.Clear();
                OnPropertyChanged("Errors");
            });
        }
    }
}
