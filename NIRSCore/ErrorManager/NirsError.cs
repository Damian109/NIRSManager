namespace NIRSCore.ErrorManager
{
    public class NirsError
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
        /// Сообщение ошибки
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Конструктор класса ошибки
        /// </summary>
        /// <param name="nameSource">Класс, в котором произошла ошибка</param>
        /// <param name="nameSystem">Подсистема, в которой произошла ошибка</param>
        /// <param name="message">Сообщение ошибки</param>
        public NirsError(string nameSource, string nameSystem, string message)
        {
            NameSource = nameSource;
            NameSystem = nameSystem;
            Message = message;
        }
    }
}
