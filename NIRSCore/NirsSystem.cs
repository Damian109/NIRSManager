namespace NIRSCore
{
    /// <summary>
    /// Статический класс Система
    /// </summary>
    public static class NirsSystem
    {
        /// <summary>
        /// Менеджер ошибок
        /// </summary>
        public static ErrorManager.ErrorManager ErrorManager { get; set; }

        /// <summary>
        /// Стек операций
        /// </summary>
        public static StackOperations.StackOperations StackOperations { get; set; }

        /// <summary>
        /// Файл учетных записей
        /// </summary>
        public static FileOperations.FileUsers FileUsers { get; set; }

        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public static User User { get; set; }

        /// <summary>
        /// Логин для входа
        /// </summary>
        public static string Login { get; set; }

        /// <summary>
        /// Md5-сумма логина и пароля
        /// </summary>
        public static string Md5 { get; set; }

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static NirsSystem()
        {
            ErrorManager = new ErrorManager.ErrorManager();
            StackOperations = new StackOperations.StackOperations();
            FileUsers = new FileOperations.FileUsers();
            FileUsers.Open();
            User = new User();
            Login = Md5 = string.Empty;
            Synchronization();
        }

        /// <summary>
        /// Открыть пользовательские настройки
        /// </summary>
        public static void OpenUserSettings()
        {
            FileOperations.FileSettings fileSettings = new FileOperations.FileSettings(Login, Md5);
            fileSettings.Open();
            User = fileSettings.GetUser();
        }

        /// <summary>
        /// Синхронизация с сервером
        /// </summary>
        private static void Synchronization()
        {
            
        }

        /// <summary>
        /// Завершение работы системы
        /// </summary>
        public static void Close()
        {
            ErrorManager.SaveErrors();
            FileOperations.FileSettings fileSettings = new FileOperations.FileSettings(Login, Md5);
            fileSettings.SetUser(User);
            fileSettings.Save();
            FileUsers.Save();
            Synchronization();
        }
    }
}
