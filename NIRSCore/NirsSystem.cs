using System.Threading;
using NIRSCore.FileOperations;
using NIRSManagerClient.DataBaseModels;

namespace NIRSCore
{
    /// <summary>
    /// Статический класс Система
    /// </summary>
    public static partial class NirsSystem
    {
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
        /// Общие настройки программы
        /// </summary>
        public static ProgramSettings ProgramSettings { get; set; }

        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public static User User { get; set; }

        /// <summary>
        /// Клиентская база данных
        /// </summary>
        public static ClientDatabaseContext ClientDatabaseContext { get; set; }

        /// <summary>
        /// Существует ли клиентская БД
        /// </summary>
        public static bool IsDatabaseContextCreated { get; private set; }

        /// <summary>
        /// Менеджер ошибок
        /// </summary>
        public static ErrorManager.ErrorManager ErrorManager { get; set; }

        /// <summary>
        /// Стек операций
        /// </summary>
        public static StackOperations.StackOperations StackOperations { get; set; }

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

            IsDatabaseContextCreated = false;
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
    }
}
