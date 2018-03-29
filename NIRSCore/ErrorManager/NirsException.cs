using System;

namespace NIRSCore.ErrorManager
{
    /// <summary>
    /// Класс определяет исключение, возникшее в результате работы системы
    /// </summary>
    public sealed class NirsException : Exception
    {
        /// <summary>
        /// Название класса, вызвавшего исключение
        /// </summary>
        public string NameSource { get; }

        /// <summary>
        /// Название подсистемы, в которой сгенерировалось исключение
        /// </summary>
        public string NameSystem { get; }

        /// <summary>
        /// Конструктор исключения
        /// </summary>
        /// <param name="message">Сообщение об исключении</param>
        /// <param name="nameSource">Класс, вызвавший исключение</param>
        /// <param name="nameSystem">Подсистема, в которой возникло исключение</param>
        public NirsException(string message, string nameSource, string nameSystem) : base(message)
        {
            NameSource = nameSource;
            NameSystem = nameSystem;
        }
    }
}