using System;
using System.Net.Http;
using NIRSCore.ErrorManager;
using System.Threading.Tasks;
using NIRSCore.Syncronization;
using NIRSCore.FileOperations;
using NIRSCore.StackOperations;
using NIRSCore.HelpfulEnumsStructs;

namespace NIRSCore
{
    public static partial class NirsSystem
    {
        /// <summary>
        /// Файл учетных записей
        /// </summary>
        private static FileUsers _fileUsers;

        /// <summary>
        /// Логин для входа
        /// </summary>
        private static string _login;

        /// <summary>
        /// Md5-сумма логина и пароля
        /// </summary>
        private static string _md5;

        //Авторизация пользователя
        //Проверка есть ли логин в файле пользователей
        private static bool AuthLoginInFile(string input) => _fileUsers.IsUserCreated(input);

        //Регистрация пользователя

        /// <summary>
        /// Проверка существования логина на сервере
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        private static bool RegistrationFindUser(string login) => TaskRegistrationFindUser(login);

        /// <summary>
        /// Запрос на сервер с целью установления существования логина
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        private static bool TaskRegistrationFindUser(string login)
        {
            if (!IsServer)
                return false;
            using (var client = new HttpClient())
            {
                HttpResponseMessage message = client.PostAsJsonAsync(ProgramSettings.AdressServer + "Registration/IsLogin",
                    new LoginData(login)).Result;
                string resultString = message.Content.ReadAsStringAsync().Result;
                bool result = Convert.ToBoolean(resultString);
                return result;
            }
        }

        /// <summary>
        /// Регистация пользователя в системе
        /// </summary>
        /// <param name="isServer">Выполнять через сервер</param>
        private static void RegistrationFunc(bool isServer)
        {
            //Регистраия на сервере
            if (isServer && IsServer)
                AsyncRegistrationToServer();

            //Регистрация на клиенте
            AsyncRegistrationToClient();
        }

        /// <summary>
        /// Асинхронный запрос на сервер с целью регистрации пользователя
        /// </summary>
        private static async void AsyncRegistrationToServer() =>
            await Task.Run(() =>
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage message = client.PostAsJsonAsync(ProgramSettings.AdressServer + "Registration/RegistrationUser",
                        new RegistrationData(_login, _md5)).Result;
                }
            });

        /// <summary>
        /// Асинхронная регистрация на клиенте
        /// </summary>
        private static async void AsyncRegistrationToClient() =>
            await Task.Run(() =>
            {
                _fileUsers.AddNewUsersItem(new FileUsersItem() { Login = _login, Md5 = _md5, IsMain = false });
            });

        /// <summary>
        /// Авторизация в системе
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="isDone">Закончена ли авторизация</param>
        /// <returns>Статус авторизации</returns>
        public static AuthorizationStatus Authorization(string login, string password, bool isDone)
        {
            if (isDone)
            {
                if (!AuthLoginInFile(login))
                    return AuthorizationStatus.AuthError;
                _login = login;
                _md5 = HashForSecurity.GetMd5Hash(login + password);
                string md5 = _fileUsers.GetMd5(login);
                if (md5 != _md5)
                    return AuthorizationStatus.AuthError;
                StackOperations.AddOperation(new Operation("Вход в систему", null, null));
            }
            if (login == string.Empty)
                return AuthorizationStatus.AuthLogin;
            if (password == string.Empty)
                return AuthorizationStatus.AuthPassword;
            return AuthorizationStatus.AuthOK;
        }

        /// <summary>
        /// Регистрация в системе
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="isServer">Регистрация на сервере</param>
        /// <param name="isDone">Закончена ли регистрация</param>
        /// <returns>Статус регистрации</returns>
        public static RegistrationStatus Registration(string login, string password, bool isServer, bool isDone)
        {
            if (isDone)
            {
                if (isServer && RegistrationFindUser(login))
                    return RegistrationStatus.RegError;
                _login = login;
                _md5 = HashForSecurity.GetMd5Hash(login + password);
                RegistrationFunc(isServer);
                StackOperations.AddOperation(new Operation("Регистрация", null, null));
                return RegistrationStatus.RegGood;
            }

            if (login == string.Empty)
                return RegistrationStatus.RegLogin;
            if (AuthLoginInFile(login))
                return RegistrationStatus.RegError;
            if (isServer)
            {
                if (IsServer)
                {
                    bool isQuery = RegistrationFindUser(login);
                    if (isQuery)
                        return RegistrationStatus.RegError;
                }
                else
                {
                    return RegistrationStatus.RegServerErr;
                }
            }
            if (password == string.Empty)
                return RegistrationStatus.RegPassword;
            return RegistrationStatus.RegOK;
        }

        /// <summary>
        /// Открыть пользовательские настройки
        /// </summary>
        public static bool OpenUserSettings()
        {
            if (_login == string.Empty)
                return false;
            try
            {
                FileSettings fileSettings = new FileSettings(_login, _md5);
                fileSettings.Read();

                //Проверка существования базы данных в виде файла
                if (fileSettings.FindDatabaseFile())
                    IsDatabaseContextCreated = true;

                User = fileSettings.User;
                
            }
            catch(NirsException exception)
            {
                ErrorManager.ExecuteException(exception);
                return false;
            }

            if (User == null)
                User = new User();
            User.Changer = true;

            //Выполнение синхронизации
            Synchronization();

            if (User.DateLastEditDatabase != DateTime.MinValue)
                IsDatabaseContextCreated = true;
            return true;
        }
    }
}
