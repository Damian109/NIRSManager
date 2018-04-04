using System;
using System.Text;
using System.Security.Cryptography;

namespace NIRSCore
{
    /// <summary>
    /// Статический класс, предназначенный для работы с Md5-хешем
    /// </summary>
    public static class HashForSecurity
    {
        /// <summary>
        /// Получение хеша
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Хеш</returns>
        public static string GetMd5Hash(string input)
        {
            StringBuilder sBuilder = new StringBuilder();

            using (MD5 hashCreator = MD5.Create())
            {
                byte[] data = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(input));

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// Определение, является ли хеш новой строки равный данному
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="hash">Сравниваемый хеш</param>
        /// <returns>Булевское значение</returns>
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string newHash = GetMd5Hash(input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (comparer.Compare(hash, newHash) == 0)
                return true;
            return false;
        }
    }
}