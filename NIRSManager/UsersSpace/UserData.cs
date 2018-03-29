namespace NIRSManager.UsersSpace
{
    /// <summary>
    /// Класс предназначен для хранения пользовательских данных для определения соответствующих файлов настроек
    /// </summary>
    internal sealed class UserData
    {
        #region Private
        private string _login;
        private string _md5;
        #endregion

        /// <summary>
        /// Метод возвращает логин пользователя в системе, который необходим для загрузки настроек пользователя
        /// </summary>
        /// <param name="md5">md5-хеш логина и пароля</param>
        /// <returns>В качестве возвращаемого значения - возвращает логин пользователя или пустую строку при несоответствии</returns>
        public string GetFileName(string md5)
        {
            if (md5 == _md5)
                return _login;
            return string.Empty;
        }

        /// <summary>
        /// Конструктор по умолчанию, без параметров
        /// </summary>
        public UserData()
        {
            _login = string.Empty;
            _md5 = string.Empty;
        }

        /// <summary>
        /// Переопределенный конструктор с параметрами
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="md5">md5-хеш логина и пароля</param>
        public UserData(string login, string md5)
        {
            _login = login;
            _md5 = md5;
        }
    }
}
