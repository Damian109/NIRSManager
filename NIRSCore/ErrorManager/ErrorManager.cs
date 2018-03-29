using NIRSCore.FileOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach (var elem in file.ErrorsItems)
                _nirsErrors.Add(new NirsError(elem.NameSource, elem.NameSystem, elem.Message));
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