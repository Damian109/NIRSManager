namespace NIRSCore.StackOperations
{
    public sealed class Operation
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
        /// Команда выполнения операции
        /// </summary>
        public RelayCommand DoneCommand { get; private set; }

        /// <summary>
        /// Команда отмены операции
        /// </summary>
        public RelayCommand UnDoneCommand { get; private set; }

        //
        public Operation(string name, RelayCommand doneCommand, RelayCommand undoneCommand)
        {
            Name = name;
            if(doneCommand == null)
                IsDone = false;
            else
            {
                DoneCommand = doneCommand;
                IsDone = true;
            }
            if (undoneCommand == null)
                IsUnDone = false;
            else
            {
                UnDoneCommand = undoneCommand;
                IsUnDone = true;
            }
        }
    }
}