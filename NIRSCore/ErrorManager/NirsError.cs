using System;

namespace NIRSCore.ErrorManager
{
    public class NirsError
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
        /// Сообщение ошибки
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Дата возникновения ошибки
        /// </summary>
        public DateTime DateError { get; set; }

        public bool IsNew { get; set; }

        /// <summary>
        /// Конструктор класса ошибки
        /// </summary>
        /// <param name="nameSource">Класс, в котором произошла ошибка</param>
        /// <param name="nameSystem">Подсистема, в которой произошла ошибка</param>
        /// <param name="message">Сообщение ошибки</param>
        public NirsError(string nameSource, string nameSystem, string message, DateTime date, bool isNew = false)
        {
            NameSource = nameSource;
            NameSystem = nameSystem;
            Message = message;
            DateError = date;
            IsNew = isNew;
        }

        public NirsError() { }
    }
}
