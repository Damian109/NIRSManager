using NIRSCore;
using NIRSCore.ErrorManager;
using System.Collections.Generic;

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
            Errors = NirsSystem.ErrorManager.GetErrors();
            OnPropertyChanged("Errors");
        }

        #endregion

        public ErrorsViewModel() : base("Главная форма") { }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public List<NirsError> Errors { get; set; }

        /// <summary>
        /// Команда Отправить на сервер
        /// </summary>
        public RelayCommand CommandSetToServer
        {
            get => new RelayCommand(obj => NirsSystem.ErrorManager.SetToServer(NirsSystem.ProgramSettings.AdressServer));
        }

        /// <summary>
        /// Команда Очистить
        /// </summary>
        public RelayCommand CommandClear
        {
            get => new RelayCommand(obj => NirsSystem.ErrorManager.Clear());
        }
    }
}
