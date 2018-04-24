using System;
using NIRSCore.FileOperations;
using System.Collections.Generic;

namespace NIRSCore.ErrorManager
{
    /// <summary>
    /// Статический класс, для управления системой отлова ошибок в программе
    /// </summary>
    public class ErrorManager
    {
        #region Private
        private List<NirsError> _nirsErrors;
        #endregion

        /// <summary>
        /// Статический конструктор
        /// </summary>
        public ErrorManager()
        {
            _nirsErrors = new List<NirsError>();
            FileErrors file = new FileErrors();
            file.Read();
            if(file.ErrorsItems.Count > 0)
                foreach (var elem in file.ErrorsItems)
                    _nirsErrors.Add(new NirsError(elem.NameSource, elem.NameSystem, elem.Message, elem.DateError));
        }

        /// <summary>
        /// Сохранение лога ошибок
        /// </summary>
        public void SaveErrors()
        {
            if (_nirsErrors.Count < 1)
                return;
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
            file.Write();
        }

        /// <summary>
        /// Добавление нового исключения в стек системы ошибок
        /// </summary>
        /// <param name="exception">Исключение</param>
        public void ExecuteException(NirsException exception) =>
            _nirsErrors.Add(new NirsError(exception.NameSource, exception.NameSystem, exception.Message, DateTime.Now));

        /// <summary>
        /// Получение всех ошибок, полученных в результате работы программы и не отправленных на сервер
        /// </summary>
        /// <returns>Список ошибок</returns>
        public List<NirsError> GetErrors() => _nirsErrors;

        /// <summary>
        /// Отправить ошибки на сервер и очистка списка
        /// </summary>
        public void SetToServer()
        {
            //
            //
            //
            //
            //
            //
            Clear();
        }

        /// <summary>
        /// Удаление списка ошибок
        /// </summary>
        public void Clear()
        {
            _nirsErrors.Clear();
            FileErrors file = new FileErrors();
            file.Delete();
        }
    }
}