using System;
using NIRSCore;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using NIRSCore.FileOperations;
using NIRSCore.ErrorManager;
using NIRSCore.StackOperations;
using System.Collections.Generic;

namespace NIRSManagerClient.ViewModels
{
    public sealed class MenuPanelViewModel : ViewModel
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
            FIO = $"{_user.SurName} {_user.Name} {_user.SecondName}";
            if (FIO == string.Empty)
                FIO = "(ФИО не указано)";
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

        public List<IOperation> Operations
        {
            get => StackOperations.Operations;
        }






        public MenuPanelViewModel(User user) : base("Главная форма")
        {
            _user = user;
            GetFio();
            IsLeftPosition = _user.IsLeftPosition;
            GetLastOperation();
        }



        

        


        



        //Сколько запросов на обмен
        //Последняя операция в стеке

        //Список элементов меню почленно
        //Список операций для работы со списком

    }
}
