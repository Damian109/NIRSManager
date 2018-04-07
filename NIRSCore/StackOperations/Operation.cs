﻿namespace NIRSCore.StackOperations
{
    public sealed class Operation
    {
        #region Private
        private bool _isUnDone, _isDone;
        #endregion

        /// <summary>
        /// Реализация события - изменение состояния операции
        /// </summary>
        public delegate void eventSender();
        public event eventSender ChangeStatusEvent;

        /// <summary>
        /// Может ли операция отменяться
        /// </summary>
        public bool IsUnDone
        {
            get => _isUnDone;
            set
            {
                _isUnDone = value;
                ChangeStatusEvent?.Invoke();
            }
        }

        /// <summary>
        /// Может ли операция применяться
        /// </summary>
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                ChangeStatusEvent?.Invoke();
            }
        }

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