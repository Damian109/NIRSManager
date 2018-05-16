using System.Web.Mvc;

namespace NIRSManagerServer.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за ответы на запросы клиента, касательно служебной информации
    /// </summary>
    public class ServerController : Controller
    {
        /// <summary>
        /// Проверка работоспособности сервера и его доступности
        /// </summary>
        /// <returns>Всегда истина</returns>
        [HttpGet]
        public bool Ping() => true;
    }
}