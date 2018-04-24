using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NIRSCore.FileOperations;
using NIRSCore.HelpfulEnumsStructs;
using NIRSCore.StackOperations;
using NIRSCore.Syncronization;

namespace NIRSCore
{
    /// <summary>
    /// Статический класс Система
    /// </summary>
    public static class NirsSystem
    {
        #region Private

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

        /// <summary>
        /// Задержка таймера перед следующей проверкой сервера
        /// </summary>
        private const int MSTOTIMER = 60000;

        /// <summary>
        /// Метод обратного вызова для таймера
        /// </summary>
        private static TimerCallback _timerCallback;

        /// <summary>
        /// Таймер для проверки доступности сервера
        /// </summary>
        private static Timer _timer;

        /// <summary>
        /// Доступен ли сервер
        /// </summary>
        private static bool _isServer;

        /// <summary>
        /// Проверка работоспособности сервера
        /// </summary>
        private async static void AsyncPingToServer(object obj) =>
            IsServer = await TaskPingToServer();

        /// <summary>
        /// Асинхронный запрос на сервер, с целью проверки его доступности
        /// </summary>
        /// <returns></returns>
        private static Task<bool> TaskPingToServer()
        {
            return Task.Run(() =>
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage message = client.GetAsync(ProgramSettings.AdressServer + "Server/Ping").Result;
                    bool result = false;
                    if (message.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string resultString = message.Content.ReadAsStringAsync().Result;
                        result = Convert.ToBoolean(resultString);
                    }
                    return result;
                }
            });
        }

        //Авторизация пользователя
        //Проверка есть ли логин в файле пользователей
        private static bool AuthLoginInFile(string input) => _fileUsers.IsUserCreated(input);

        //Регистрация пользователя
        
        /// <summary>
        /// Проверка существования логина на сервере
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        private async static Task<bool> AsyncRegistrationFindUser(string login) => await TaskRegistrationFindUser(login);

        /// <summary>
        /// Асинхронный запрос на сервер с целью установления существования логина
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        private static Task<bool> TaskRegistrationFindUser(string login)
        {
            return Task.Run(() =>
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage message = client.PostAsJsonAsync(ProgramSettings.AdressServer + "Registration/IsLogin",
                        new LoginData(login)).Result;
                    string resultString = message.Content.ReadAsStringAsync().Result;
                    bool result = Convert.ToBoolean(resultString);
                    return result;
                }
            });
        }

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
        /// Синхронизация с сервером
        /// </summary>
        private static void Synchronization()
        {

        }

        #endregion

        /// <summary>
        /// Менеджер ошибок
        /// </summary>
        public static ErrorManager.ErrorManager ErrorManager { get; set; }

        /// <summary>
        /// Стек операций
        /// </summary>
        public static StackOperations.StackOperations StackOperations { get; set; }

        /// <summary>
        /// Общие настройки программы
        /// </summary>
        public static ProgramSettings ProgramSettings { get; set; }

        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public static User User { get; set; }

        /// <summary>
        /// Делегат без параметров
        /// </summary>
        public delegate void eventSender();

        /// <summary>
        /// Событие изменения статуса доступности сервера
        /// </summary>
        public static event eventSender ChangeStatusServer;

        /// <summary>
        /// Доступен ли сервер
        /// </summary>
        public static bool IsServer
        {
            get => _isServer;
            private set
            {
                _isServer = value;
                ChangeStatusServer?.Invoke();
            }
        }

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static NirsSystem()
        {
            //Инициализация диспетчера ошибок, стека операций и пользовательских настроек
            ErrorManager = new ErrorManager.ErrorManager();
            StackOperations = new StackOperations.StackOperations();
            User = new User();

            //Загрузка настроек программы
            FileProgramSettings fileProgramSettings = new FileProgramSettings();
            fileProgramSettings.Read();
            ProgramSettings = fileProgramSettings.ProgramSettings;
            if (ProgramSettings == null)
                ProgramSettings = new ProgramSettings();

            //Загрузка файла учетных записей
            _fileUsers = new FileUsers();
            _fileUsers.Read();

            //Определение логина и хеш-суммы
            FileUsersItem fileUsersItem = _fileUsers.GetMainUser();
            if(fileUsersItem == null)
                _login = _md5 = string.Empty;
            else
            {
                _login = fileUsersItem.Login;
                _md5 = fileUsersItem.Md5;
            }

            //Проверка сервера на работоспособность
            //Устанавливаем метод обратного вызова
            _timerCallback = new TimerCallback(AsyncPingToServer);
            //Создаем таймер
            _timer = new Timer(_timerCallback, 0, 0, MSTOTIMER);

            //Выполнение синхронизации
            Synchronization();
        }

        /// <summary>
        /// Открыть пользовательские настройки
        /// </summary>
        public static bool OpenUserSettings()
        {
            if (_login == string.Empty)
                return false;
            FileSettings fileSettings = new FileSettings(_login, _md5);
            fileSettings.Read();
            User = fileSettings.User;
            if (User == null)
                User = new User();
            User.Changer = true;
            return true;
        }

        /// <summary>
        /// Завершение работы системы
        /// </summary>
        public static void Close()
        {
            ErrorManager.SaveErrors();
            _fileUsers.Write();
            FileSettings fileSettings = new FileSettings(_login, _md5)
            {
                User = User
            };
            fileSettings.Write();
            Synchronization();
        }

        /// <summary>
        /// Авторизация в системе
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="isDone">Закончена ли авторизация</param>
        /// <returns>Статус авторизации</returns>
        public static AuthorizationStatus Authorization(string login, string password, bool isDone)
        {
            if(isDone)
            {
                if (!AuthLoginInFile(login))
                    return AuthorizationStatus.AuthError;
                _login = login;
                _md5 = HashForSecurity.GetMd5Hash(login + password);
                string md5 = _fileUsers.GetMd5(login);
                if(md5 != _md5)
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
            if(isDone)
            {
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
            if(isServer)
            {
                if (IsServer)
                {
                    bool isQuery = AsyncRegistrationFindUser(login).Result;
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
    }
}
