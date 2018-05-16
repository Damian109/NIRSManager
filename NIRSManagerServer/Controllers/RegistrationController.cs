using System.IO;
using System.Linq;
using System.Web.Mvc;
using NIRSCore.Syncronization;
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
                if (query != null)
                    return true;
            }
            return false;
        }

        //Создать структуру файлов и папок для пользователя
        private void GenerateStructDirectoryes(string login)
        {
            DirectoryInfo Dir = new DirectoryInfo(Request.MapPath("..//data"));
            Dir.CreateSubdirectory(login);
            Dir.CreateSubdirectory(login + "//Backups");
            Dir.CreateSubdirectory(login + "//Documents");
            Dir.CreateSubdirectory(login + "//Photos");
        }

        #endregion

        /// <summary>
        /// Проверка Логина на уникальность
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns>Есть ли пользователь в базе данных</returns>
        [HttpPost]
        public bool IsLogin(LoginData data) => IsLoginFromBase(data.Login);

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="md5">Md5-сумма логина и пароля</param>
        /// <returns>Получилось ли зарегистрировать пользователя</returns>
        [HttpPost]
        public bool RegistrationUser(RegistrationData data)
        {
            if (IsLoginFromBase(data.Login))
                return false;
            using (ServerDatabaseContext databaseContext = new ServerDatabaseContext())
            {
                databaseContext.AddUser(new UserTable()
                {
                    Login = data.Login,
                    Md5 = data.Md5,
                    DateEditDatabase = null,
                    DateEditSetting = null
                });

                GenerateStructDirectoryes(data.Login);
            }
            return true;
        }
    }
}