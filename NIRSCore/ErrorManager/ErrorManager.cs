using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NIRSCore.FileOperations;
using System.Collections.Generic;
using NIRSCore.Syncronization;

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

            //Чтение файла ошибок
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
                    NameSystem = elem.NameSystem,
                    DateError = elem.DateError
                });
            file.ErrorsItems = items;
            file.Write();
        }

        /// <summary>
        /// Добавление нового исключения в стек системы ошибок
        /// </summary>
        /// <param name="exception">Исключение</param>
        public void ExecuteException(NirsException exception)
        {
            _nirsErrors.Add(new NirsError(exception.NameSource, exception.NameSystem, exception.Message, DateTime.Now, true));
            ChangeStatusEvent?.Invoke(exception.Message, exception.NameSystem);
        }

        /// <summary>
        /// Получение всех ошибок, полученных в результате работы программы и не отправленных на сервер
        /// </summary>
        /// <returns>Список ошибок</returns>
        public List<NirsError> GetErrors()
        {
            List<NirsError> errors = new List<NirsError>();
            foreach(var elem in _nirsErrors)
            {
                elem.IsNew = false;
                errors.Add(elem);
            }
            errors.Reverse();
            return errors;
        }

        /// <summary>
        /// Отправить ошибки на сервер и очистка списка
        /// </summary>
        public void SetToServer(string adress) => SetToServerAsync(adress);

        //Асинхронная отправка ошибок на сервер
        private async void SetToServerAsync(string adress) => await Task.Run(() =>
        {
            using (var client = new HttpClient())
            {
                try
                {
                    NirsError[] errors = new NirsError[_nirsErrors.Count];
                    int countr = 0;
                    foreach (var elem in _nirsErrors)
                        errors[countr++] = new NirsError(elem.NameSource, elem.NameSystem, elem.Message, elem.DateError);
                    ErrorsData data = new ErrorsData(errors);
                    HttpResponseMessage message2 = client.PostAsJsonAsync(adress + "Server/ErrorsSet", data).Result;
                }
                catch (Exception)
                {
                    return;
                }
            }
            Clear();
        });

        /// <summary>
        /// Удаление списка ошибок
        /// </summary>
        public void Clear()
        {
            _nirsErrors.Clear();
            FileErrors file = new FileErrors();
            file.Delete();
        }

        /// <summary>
        /// Реализация события - изменение состояния
        /// </summary>
        public delegate void eventSender(string message, string source);
        public event eventSender ChangeStatusEvent;

        /// <summary>
        /// Получение списка новых возникших ошибок
        /// </summary>
        /// <returns></returns>
        public int GetCountNewErrors() => _nirsErrors.Count(u => u.IsNew == true);
    }
}
