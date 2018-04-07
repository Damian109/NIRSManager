using System.Collections.Generic;

namespace NIRSCore.StackOperations
{
    /// <summary>
    /// Класс - стек операций
    /// </summary>
    public static class StackOperations
    {
        /// <summary>
        /// Список всех выполненных операций
        /// </summary>
        public static List<Operation> Operations { get; private set; }

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static StackOperations()
        {
            Operations = new List<Operation>();
        }

        /// <summary>
        /// Добавление операции
        /// </summary>
        /// <param name="operation">Операция</param>
        public static void AddOperation(Operation operation)
        {
            int count = Operations.Count;
            Operations.Insert(count, operation);
        }
    }
}
