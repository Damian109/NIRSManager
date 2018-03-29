using System;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Класс служит для хранения информации о строке файла с ошибками
    /// </summary>
    [Serializable]
    public sealed class FileErrorsItem
    {
        /// <summary>
        /// Название класса, вызвавшего исключение
        /// </summary>
        public string NameSource { get; set; }

        /// <summary>
        /// Название подсистемы, в которой сгенерировалось исключение
        /// </summary>
        public string NameSystem { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Пустой конструктор для сериализации
        /// </summary>
        public FileErrorsItem() { }
    }
}
