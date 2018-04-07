using NIRSCore;
using NIRSCore.StackOperations;
using NIRSManagerClient.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NIRSManagerClient.ViewModels
{
    public sealed class ExtensionViewModel : ViewModel
    {
        #region Private
        private User _user;

        /// <summary>
        /// Получение последней выполненной операции
        /// </summary>
        private void GetLastOperation()
        {
            LastOperation = StackOperations.Operations.LastOrDefault().Name;
        }

        /// <summary>
        /// Получение ФИО пользователя
        /// </summary>
        private void GetFio()
        {
            if (_user.SurName == string.Empty && _user.Name == string.Empty &&
                _user.SecondName == string.Empty)
                FIO = "(ФИО не указано)";
            else
                FIO = $"{_user.SurName} {_user.Name} {_user.SecondName}";
        }

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
        public List<IOperation> Operations
        {
            get => StackOperations.Operations;
        }





        public ExtensionViewModel(User user) : base("Главная форма")
        {
            _user = user;
            GetFio();
            IsLeftPosition = _user.IsLeftPosition;
            GetLastOperation();
            LoadChild(new MainSettingsView(_user));
        }
    }
}