using System.IO;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Управление каталогами пользователя
    /// </summary>
    internal static class UserDirectory
    {
        /// <summary>
        /// Проверяем присутствуют ли каталоги в системе
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Все ли каталоги присутствуют</returns>
        public static bool IsValid(string login)
        {
            if ((!Directory.Exists("data//" + login)) || (!Directory.Exists("data//" + login + "//Backups")) ||
                (!Directory.Exists("data//" + login + "//Documents")) || (!Directory.Exists("data//" + login + "//Photos")))
                return false;
            return true;
        }

        /// <summary>
        /// Создание структуры каталогов для пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        public static void CreateDirectoryes(string login)
        {
            if (IsValid(login))
                return;
            if (!Directory.Exists("data//" + login))
                Directory.CreateDirectory("data//" + login);
            if (!Directory.Exists("data//" + login + "//Backups"))
                Directory.CreateDirectory("data//" + login + "//Backups");
            if (!Directory.Exists("data//" + login + "//Documents"))
                Directory.CreateDirectory("data//" + login + "//Documents");
            if (!Directory.Exists("data//" + login + "//Photos"))
                Directory.CreateDirectory("data//" + login + "//Photos");
        }
    }
}
