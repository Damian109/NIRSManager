using System.Collections.Generic;

namespace NIRSCore.StackOperations
{
    /// <summary>
    /// Класс - стек операций
    /// </summary>
    public class StackOperations
    {
        #region Private
        //Список операций
        private List<Operation> _operations;

        //Изменение операции
        private void OperationEventExecuter() => ChangeStatusEvent?.Invoke();

        #endregion

        /// <summary>
        /// Реализация события - изменение состояния операции
        /// </summary>
        public delegate void eventSender();
        public event eventSender ChangeStatusEvent;

        /// <summary>
        /// Список всех выполненных операций
        /// </summary>
        public List<Operation> Operations
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
        public StackOperations() => _operations = new List<Operation>();

        /// <summary>
        /// Добавление операции
        /// </summary>
        /// <param name="operation">Операция</param>
        public void AddOperation(Operation operation)
        {
            operation.ChangeStatusEvent += OperationEventExecuter;
            Operations.Insert(0, operation);
        }
    }
}
