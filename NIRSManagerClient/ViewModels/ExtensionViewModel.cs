using NIRSCore;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using NIRSManagerClient.Views;
using System.Windows.Controls;
using NIRSCore.StackOperations;
using System.Collections.Generic;

namespace NIRSManagerClient.ViewModels
{
    public sealed class ExtensionViewModel : ViewModel
    {
        #region Private

        /// <summary>
        /// Получение последней выполненной операции
        /// </summary>
        private void GetLastOperation() =>
            LastOperation = NirsSystem.StackOperations.Operations.LastOrDefault().Name;

        /// <summary>
        /// Обработка закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if(NirsSystem.User.IsMinimizeToTray)
            {
                e.Cancel = true;
                return;
            }
            NirsSystem.Close();
        }

        /// <summary>
        /// Обработка события изменения ФИО
        /// </summary>
        private void _user_ChangeFIOEvent() => GetFio();

        //
        private void StackOperations_ChangeStatusEvent() => OnPropertyChanged("Operations");

        /// <summary>
        /// Получение ФИО пользователя
        /// </summary>
        private void GetFio()
        {
            if (NirsSystem.User.SurName == string.Empty && NirsSystem.User.Name == string.Empty &&
                NirsSystem.User.SecondName == string.Empty)
                FIO = "(ФИО не указано)";
            else
                FIO = $"{NirsSystem.User.SurName} {NirsSystem.User.Name} {NirsSystem.User.SecondName}";
            OnPropertyChanged("FIO");
        }

        /// <summary>
        /// Загрузка нового элемента в основное окно
        /// </summary>
        /// <param name="view"></param>
        private void LoadChild(UserControl view)
        {
            ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(view);
        }

        #endregion

        /// <summary>
        /// Фамилия, Имя, Отчество пользователя
        /// </summary>
        public string FIO { get; private set; }

        /// <summary>
        /// Позиция основного меню
        /// </summary>
        public bool IsLeftPosition { get; private set; }

        /// <summary>
        /// Последняя операция
        /// </summary>
        public string LastOperation { get; private set; }

        /// <summary>
        /// Список операций
        /// </summary>
        public List<Operation> Operations
        {
            get => NirsSystem.StackOperations.Operations;
        }

        public ExtensionViewModel() : base("Главная форма")
        {
            NirsSystem.User.ChangeFIOEvent += _user_ChangeFIOEvent;
            NirsSystem.StackOperations.ChangeStatusEvent += StackOperations_ChangeStatusEvent;
            GetFio();
            IsLeftPosition = NirsSystem.User.IsLeftPosition;
            GetLastOperation();

            ///
            LoadChild(new MainSettingsView());
        }
    }
}