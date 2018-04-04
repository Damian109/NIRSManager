namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Специальный контейнер-обертка для обмена данными с сервером
    /// </summary>
    public sealed class LoginData
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public LoginData() => Login = string.Empty;

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="login">огин пользователя</param>
        public LoginData(string login) => Login = login;
    }
}