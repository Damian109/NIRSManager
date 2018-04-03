using System.IO;
using System.Linq;
using System.Web.Mvc;
using NIRSManagerServer.Models;

namespace NIRSManagerServer.Controllers
{
    /// <summary>
    /// Контроллер API обрабатывающий запросы, касающиеся регистрации
    /// </summary>
    [RoutePrefix("Registration")]
    public class RegistrationController : Controller
    {
        #region Private
        //Возвращает true, если такого пользователя в базе данных не существует
        private bool IsLoginFromBase(string login)
        {
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                var query = databaseContext.Users.FirstOrDefault(u => u.Login == login);
                if (query == null)
                    return true;
            }
            return false;
        }

        //Создать структуру файлов и папок для пользователя
        private void GenerateStructDirectoryes(string login)
        {
            Directory.CreateDirectory(login);
            Directory.CreateDirectory(login + "//Backups");
            Directory.CreateDirectory(login + "//Documents");
            Directory.CreateDirectory(login + "//Photos");
        }

        #endregion

        /// <summary>
        /// Проверка Логина на уникальность
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns>Есть ли пользователь в базе данных</returns>
        [HttpGet]
        public bool IsLogin(string login)
        {
            return IsLoginFromBase(login);
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="md5">Md5-сумма логина и пароля</param>
        /// <returns>Получилось ли зарегистрировать пользователя</returns>
        [HttpGet]
        public bool RegistrationUser(string login, string md5)
        {
            if (IsLoginFromBase(login))
                return false;
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                databaseContext.AddUser(new UserTable()
                {
                    Login = login,
                    Md5 = md5,
                    DateEditDatabase = null,
                    DateEditSetting = null,
                    NameDatabase = null
                });

                GenerateStructDirectoryes(login);
            }
            return true;
        }
    }
}