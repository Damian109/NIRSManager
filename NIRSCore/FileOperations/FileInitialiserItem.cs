using System;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Класс служит для хранения информации о элементе инициализации БД
    /// </summary>
    [Serializable]
    public sealed class FileInitialiserItem
    {
        /// <summary>
        /// Название таблицы, в которую производится вставка
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string ValueName { get; set; }

        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public FileInitialiserItem() {  }
    }
}