using System;
using System.IO;
using System.Text;
using System.Linq;
using NIRSCore.ErrorManager;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace NIRSCore.FileOperations
{
    public sealed class FileUsers : FileCore
    {
        private List<FileUsersItem> _usersItems;
        private byte[] _key;
        private byte[] _vector;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileUsers()
        {
            _filename = "Data//Users.bin";
            _usersItems = new List<FileUsersItem>();
            _key = Encoding.ASCII.GetBytes("ERF156TY73WE43WBN78GH243");
            _vector = Encoding.ASCII.GetBytes("12345678");
        }

        /// <summary>
        /// Метод возвращает логин пользователя в системе, который необходим для загрузки настроек пользователя
        /// </summary>
        /// <param name="md5">связка логина и пароля</param>
        /// <returns>В качестве возвращаемого значения - возвращает логин пользователя или пустую строку при несоответствии</returns>
        public string GetFileName(string input)
        {
            FileUsersItem usersItem = _usersItems.Where(u => HashForSecurity.VerifyMd5Hash(input, u.Md5)).FirstOrDefault();
            if (usersItem == null)
                return string.Empty;
            else
                return usersItem.Login;
        }

        /// <summary>
        /// Метод возвращает ключ, для дешифровки файла настроек
        /// </summary>
        /// <param name="login">логин пользователя</param>
        /// <returns>Ключ дешифрования в форме Md5-суммы логина и пароля</returns>
        public string GetKey(string login)
        {
            FileUsersItem usersItem = _usersItems.Where(u => u.Login == login).FirstOrDefault();
            if (usersItem == null)
                return string.Empty;
            else
                return usersItem.Md5;
        }

        /// <summary>
        /// Открыть файл учетных данных пользователей
        /// </summary>
        public override void Open()
        {
            if (!File.Exists(_filename))
                return;
            try
            {
                //Поток для чтения из файла
                using (FileStream fileStream = new FileStream(_filename, FileMode.Open))
                {
                    TripleDESCryptoServiceProvider serviceProvider = new TripleDESCryptoServiceProvider();

                    //Поток дешифровки файла
                    using (Stream cryptoStream = new CryptoStream(fileStream, serviceProvider.CreateDecryptor(_key, _vector), 
                        CryptoStreamMode.Read))
                    {
                        //Выполняется десериализация в список объектов
                        XmlSerializer serializer = new XmlSerializer(typeof(FileUsersItem[]));
                        FileUsersItem[] items = (FileUsersItem[])serializer.Deserialize(cryptoStream);
                        _usersItems = items.ToList();
                    }
                }
            }
            catch(Exception)
            {
                throw new NirsException("Ошибка при загрузке файла учетных записей", "Файл учетных записей", "Файловая система");
            }
        }

        /// <summary>
        /// Сохранить файл учетных данных пользователя
        /// </summary>
        public override void Save()
        {
            if (_usersItems.Count < 1)
                return;
            try
            {
                using (FileStream fileStream = new FileStream(_filename, FileMode.OpenOrCreate))
                {
                    TripleDESCryptoServiceProvider serviceProvider = new TripleDESCryptoServiceProvider();
                    //Поток шифрования файла
                    using (Stream cryptoStream = new CryptoStream(fileStream, serviceProvider.CreateEncryptor(_key, _vector),
                       CryptoStreamMode.Write))
                    {
                        //Выполняется сериализация в список объектов
                        XmlSerializer serializer = new XmlSerializer(typeof(FileUsersItem[]));
                        serializer.Serialize(cryptoStream, _usersItems.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                throw new NirsException("Ошибка при записи файла учетных записей", "Файл учетных записей", "Файловая система");
            }
        }

        /// <summary>
        /// Добавление нового пользователя в файл учетных записей
        /// </summary>
        /// <param name="item">Пользовательские данные</param>
        public void AddNewUsersItem(FileUsersItem item)
        {
            _usersItems.Add(item);
        }
    }
}