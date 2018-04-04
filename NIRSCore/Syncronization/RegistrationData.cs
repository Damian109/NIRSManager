namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Специальный контейнер-обертка для данных регистрации на сервере
    /// </summary>
    public sealed class RegistrationData
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Md5-сумма логина и пароля
        /// </summary>
        public string Md5 { get; set; }

        public RegistrationData() => Login = Md5 = string.Empty;

        public RegistrationData(string login, string md5)
        {
            Login = login;
            Md5 = md5;
        }
    }
}
