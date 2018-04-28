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
    internal sealed class FileUsers : FileCore
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
        /// Открыть файл учетных данных пользователей
        /// </summary>
        public sealed override void Read()
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
            catch (Exception)
            {
                throw new NirsException("Ошибка при загрузке файла учетных записей", "Файл учетных записей", "Файловая система");
            }
        }

        /// <summary>
        /// Сохранить файл учетных данных пользователя
        /// </summary>
        public sealed override void Write()
        {
            if (_usersItems.Count < 1)
                return;
            if (!Directory.Exists("data//"))
                Directory.CreateDirectory("data//");
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
            catch (Exception e)
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

        /// <summary>
        /// Проверка существования пользователя с таким логином
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        public bool IsUserCreated(string login)
        {
            FileUsersItem usersItem = _usersItems.Where(u => u.Login == login).FirstOrDefault();
            if (usersItem == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Получить пользователя по умолчанию
        /// </summary>
        /// <returns></returns>
        public FileUsersItem GetMainUser()
        {
            return _usersItems.Where(u => u.IsMain).FirstOrDefault();
        }

        /// <summary>
        /// Задать пользователя по умолчанию
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        public void SetMainUser(string login = null)
        {
            foreach (var elem in _usersItems)
                elem.IsMain = false;
            if (login == null)
                return;
            _usersItems.Where(u => u.Login == login).FirstOrDefault().IsMain = true;
        }

        /// <summary>
        /// Возвращает md5-хеш
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        public string GetMd5(string login) => _usersItems.FirstOrDefault(u => u.Login == login)?.Md5;
    }
}