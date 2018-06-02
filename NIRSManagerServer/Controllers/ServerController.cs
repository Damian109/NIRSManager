using NIRSCore.ErrorManager;
using NIRSCore.FileOperations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NIRSManagerServer.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за ответы на запросы клиента, касательно служебной информации
    /// </summary>
    [RoutePrefix("Server")]
    public class ServerController : Controller
    {
        /// <summary>
        /// Проверка работоспособности сервера и его доступности
        /// </summary>
        /// <returns>Всегда истина</returns>
        [HttpGet]
        public bool Ping() => true;

        /// <summary>
        /// Получение от клиента списка ошибок и сохранение в логе
        /// </summary>
        /// <param name="errors">Список ошибок</param>
        /// <returns></returns>
        [HttpPost]
        public bool ErrorsSet(List<NirsError> errors) 
        {
            if (errors.Count <= 0)
                return false;
            FileErrors file = new FileErrors();
            file.Read();
            List<FileErrorsItem> items;
            if (file.ErrorsItems == null)
                items = new List<FileErrorsItem>();
            else
                items = file.ErrorsItems;
            foreach (var elem in errors)
                items.Add(new FileErrorsItem()
                {
                    Message = elem.Message,
                    NameSource = elem.NameSource,
                    NameSystem = elem.NameSystem,
                    DateError = elem.DateError
                });
            file.ErrorsItems = items;
            file.Write();
            return true;
        }
    }
}