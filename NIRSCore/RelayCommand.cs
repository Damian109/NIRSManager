using System;
using System.Windows.Input;

namespace NIRSCore
{
    /// <summary>
    /// Класс, определяющий команды для связывания графического интерфейса 
    /// с моделями представления
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        /// <summary>
        /// Изменение условий выполнения команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Метод - выполнение команды</param>
        /// <param name="canExecute">Метод - может ли выполниться команда</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, может ли команда выполниться
        /// </summary>
        /// <param name="param"></param>
        /// <returns>Да или нет</returns>
        public bool CanExecute(object param) => canExecute == null || canExecute(param);

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="param"></param>
        public void Execute(object param) => execute(param);
    }
}