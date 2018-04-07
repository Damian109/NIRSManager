using System.Collections.Generic;

namespace NIRSCore.StackOperations
{
    /// <summary>
    /// Класс - стек операций
    /// </summary>
    public static class StackOperations
    {
        #region Private
        //Список операций
        private static List<Operation> _operations;

        //Изменение операции
        private static void OperationEventExecuter() => ChangeStatusEvent?.Invoke();

        #endregion

        /// <summary>
        /// Реализация события - изменение состояния операции
        /// </summary>
        public delegate void eventSender();
        public static event eventSender ChangeStatusEvent;

        /// <summary>
        /// Список всех выполненных операций
        /// </summary>
        public static List<Operation> Operations
        {
            get => _operations;
            private set
            {
                _operations = value;
                ChangeStatusEvent();
            }
        }

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static StackOperations()
        {
            _operations = new List<Operation>();
        }

        /// <summary>
        /// Добавление операции
        /// </summary>
        /// <param name="operation">Операция</param>
        public static void AddOperation(Operation operation)
        {
            operation.ChangeStatusEvent += OperationEventExecuter;
            Operations.Insert(0, operation);
        }
    }
}
