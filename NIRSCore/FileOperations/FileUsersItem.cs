using System;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Класс, для хранения информации о пользовательских данных для входа
    /// </summary>
    [Serializable]
    public sealed class FileUsersItem
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Md5-сумма логина и пароля
        /// </summary>
        public string Md5 { get; set; }

        /// <summary>
        /// Стандартный конструктор без параметров
        /// </summary>
        public FileUsersItem() { }
    }
}
