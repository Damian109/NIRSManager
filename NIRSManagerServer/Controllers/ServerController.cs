using NIRSCore.FileOperations;
using NIRSCore.Syncronization;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;

namespace NIRSManagerServer.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за ответы на запросы клиента, касательно служебной информации
    /// </summary>
    [System.Web.Mvc.RoutePrefix("Server")]
    public class ServerController : Controller
    {
        /// <summary>
        /// Проверка работоспособности сервера и его доступности
        /// </summary>
        /// <returns>Всегда истина</returns>
        [System.Web.Mvc.HttpGet]
        public bool Ping() => true;

        /// <summary>
        /// Получение от клиента списка ошибок и сохранение в логе
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public bool ErrorsSet([FromBody]ErrorsData errors) 
        {
            if (errors.NirsErrors == null || errors.NirsErrors.Length <= 0)
                return false;
            FileErrors file = new FileErrors(Request.MapPath("..//data//"));
            file.Read();
            List<FileErrorsItem> items;
            if (file.ErrorsItems == null)
                items = new List<FileErrorsItem>();
            else
                items = file.ErrorsItems;
            foreach (var elem in errors.NirsErrors)
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