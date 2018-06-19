using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NIRSCore.BackupManager;
using NIRSCore.FileOperations;

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
        /// Есть ли пользователь по умолчанию
        /// </summary>
        public static bool IsMainUser { get; set; }

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
                IsMainUser = true;
            }

            //Проверка сервера на работоспособность
            //Устанавливаем метод обратного вызова
            _timerCallback = new TimerCallback(AsyncPingToServer);
            //Создаем таймер
            _timer = new Timer(_timerCallback, 0, 0, MSTOTIMER);

            IsDatabaseContextCreated = false;
        }

        /// <summary>
        /// Сделать пользователем по умолчанию
        /// </summary>
        public static void SetMainUser()
        {
            _fileUsers.SetMainUser(_login);
            IsMainUser = true;
        }

        /// <summary>
        ///Удалить пользователя по умолчанию
        /// </summary>
        public static void DeleteMainUser()
        {
            _fileUsers.SetMainUser();
            IsMainUser = false;
        }

        /// <summary>
        /// Завершение работы системы
        /// </summary>
        public static void Close(bool sync = true)
        {
            ErrorManager.SaveErrors();
            _fileUsers.Write();
            FileSettings fileSettings = new FileSettings(_login, _md5)
            {
                User = User
            };
            fileSettings.Write();
            FileProgramSettings fileProgramSettings = new FileProgramSettings
            {
                ProgramSettings = ProgramSettings
            };
            fileProgramSettings.Write();
            if(sync)
                Synchronization(true).GetAwaiter();

            //Уничтожение файлов
            string[] masTemp = Directory.GetFiles(Environment.CurrentDirectory + "\\data\\" + _login + "\\temp\\");

            //Создание резервной копии
            if(User.BackupIntervals != 0)
            {
                if (DateTime.Now - User.LastBackup > TimeSpan.FromDays(User.BackupIntervals))
                    CreateBackup();
            }

            foreach (var f in masTemp)
            {
                FileInfo info = new FileInfo(f);
                info.Delete();
            }
        }

        /// <summary>
        /// Получение Логина пользователя
        /// </summary>
        /// <returns></returns>
        public static string GetLogin() => _login;

        /// <summary>
        /// Получить список резервных копий
        /// </summary>
        /// <returns></returns>
        public static List<BackupElem> GetListBackups() => BackupManager.BackupManager.GetListOfBackups(Environment.CurrentDirectory + "\\data\\" + _login,
            _login);

        /// <summary>
        /// Создать новую резервную копию
        /// </summary>
        public static void CreateBackup()
        {
            BackupManager.BackupManager.CreateBackup(Environment.CurrentDirectory + "\\data\\" + _login, _login, User.DBMSName);
            User.LastBackup = DateTime.Now;
        }

        /// <summary>
        /// Восстановить базу данных из резервной копии
        /// </summary>
        /// <param name="name">Название резервной копии</param>
        public static void CreateDB(string name) => BackupManager.BackupManager.CreateDatabase(Environment.CurrentDirectory + "\\data\\" + _login, 
            name, User.DBMSName);
    }
}
