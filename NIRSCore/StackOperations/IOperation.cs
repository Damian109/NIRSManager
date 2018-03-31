namespace NIRSCore.StackOperations
{
    /// <summary>
    /// Интерфейс, определяющий поведение операций
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Может ли операция отменяться
        /// </summary>
        bool IsUnDone { get; }

        /// <summary>
        /// Может ли операция применяться
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// Название операции
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Выполнить операцию
        /// </summary>
        void Done();

        /// <summary>
        /// Отменить операцию
        /// </summary>
        void UnDone();
    }
}
