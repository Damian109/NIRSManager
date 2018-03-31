namespace NIRSCore.StackOperations
{
    /// <summary>
    /// Класс для ранения информации об операциях, которые нельзя отменить
    /// </summary>
    public sealed class NotUnDoneOperation : IOperation
    {
        /// <summary>
        /// Может ли операция отменяться
        /// </summary>
        public bool IsUnDone { get; private set; }

        /// <summary>
        /// Может ли операция применяться
        /// </summary>
        public bool IsDone { get; private set; }

        /// <summary>
        /// Название операции
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Выполнить операцию
        /// </summary>
        public void Done() { }

        /// <summary>
        /// Отменить операцию
        /// </summary>
        public void UnDone() { }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="name">Название операции</param>
        public NotUnDoneOperation(string name)
        {
            Name = name;
            IsDone = false;
            IsUnDone = false;
        }
    }
}