using NIRSCore.FileOperations;
using System.Collections.Generic;

namespace NIRSCore.ErrorManager
{
    /// <summary>
    /// Статический класс, для управления системой отлова ошибок в программе
    /// </summary>
    public static class ErrorManager
    {
        #region Private
        private static List<NirsError> _nirsErrors;
        #endregion

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static ErrorManager()
        {
            _nirsErrors = new List<NirsError>();
            FileErrors file = new FileErrors();
            file.Open();
            if(file.ErrorsItems.Count > 0)
                foreach (var elem in file.ErrorsItems)
                    _nirsErrors.Add(new NirsError(elem.NameSource, elem.NameSystem, elem.Message));
        }

        /// <summary>
        /// Сохранение лога ошибок
        /// </summary>
        public static void SaveErrors()
        {
            FileErrors file = new FileErrors();
            List<FileErrorsItem> items = new List<FileErrorsItem>();
            foreach (var elem in _nirsErrors)
                items.Add(new FileErrorsItem()
                {
                    Message = elem.Message,
                    NameSource = elem.NameSource,
                    NameSystem = elem.NameSystem
                });
            file.ErrorsItems = items;
            file.Save();
        }

        /// <summary>
        /// Добавление нового исключения в стек системы ошибок
        /// </summary>
        /// <param name="exception">Исключение</param>
        public static void ExecuteException(NirsException exception) =>
            _nirsErrors.Add(new NirsError(exception.NameSource, exception.NameSystem, exception.Message));

        /// <summary>
        /// Получение всех ошибок, полученных в результате работы программы и не отправленных на сервер
        /// </summary>
        /// <returns>Список ошибок</returns>
        public static List<NirsError> GetErrors() => _nirsErrors;

    }
}