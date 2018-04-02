using System;
using System.IO;
using System.Text;
using NIRSCore.ErrorManager;
using System.Xml.Serialization;
using NIRSCore.StackOperations;
using System.Security.Cryptography;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Класс для работы с файлом пользовательских настроек
    /// </summary>
    public sealed class FileSettings : FileCore
    {
        private byte[] _key;
        private byte[] _vector;

        private string _login, _md5;
        private User _user;

        /// <summary>
        /// Формирование ключа
        /// </summary>
        private void FormKey() => _key = Encoding.ASCII.GetBytes(_md5.Substring(0, 24));

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="md5">Md5-сумма логина и пароля</param>
        public FileSettings(string login, string md5)
        {
            _filename = login + "//Settings.bin";
            _login = login;
            _md5 = md5;
            _vector = Encoding.ASCII.GetBytes("12345678");
            FormKey();
            _user = null;
        }

        /// <summary>
        /// Создать файл настроек
        /// </summary>
        public override void Create()
        {
            Directory.CreateDirectory(_login);
            base.Create();
        }

        /// <summary>
        /// Установить пользователя и сохранить настройки
        /// </summary>
        /// <param name="user">Настройки пользователя</param>
        public void SetUser(User user) => _user = user;

        /// <summary>
        /// Получить настройки пользователя
        /// </summary>
        /// <returns>Настройки пользователя</returns>
        public User GetUser() => _user;

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
                        XmlSerializer serializer = new XmlSerializer(typeof(User));
                        _user = (User)serializer.Deserialize(cryptoStream);

                        //Добавить операцию в стек операций
                        NotUnDoneOperation operation = new NotUnDoneOperation("Файл настроек загружен");
                        StackOperations.StackOperations.AddOperation((IOperation)operation);
                    }
                }
            }
            catch (Exception)
            {
                throw new NirsException("Ошибка при загрузке файла настроек", "Файл настроек", "Файловая система");
            }
        }

        /// <summary>
        /// Сохранить файл учетных данных пользователя
        /// </summary>
        public override void Save()
        {
            if (_user == null)
                return;
            Create();
            try
            {
                using (FileStream fileStream = new FileStream(_filename, FileMode.Open))
                {
                    TripleDESCryptoServiceProvider serviceProvider = new TripleDESCryptoServiceProvider();
                    //Поток шифрования файла
                    using (Stream cryptoStream = new CryptoStream(fileStream, serviceProvider.CreateEncryptor(_key, _vector),
                       CryptoStreamMode.Write))
                    {
                        //Выполняется сериализация в список объектов
                        XmlSerializer serializer = new XmlSerializer(typeof(User));
                        serializer.Serialize(cryptoStream, _user);
                    }
                }
            }
            catch (Exception)
            {
                throw new NirsException("Ошибка при записи файла настроек", "Файл настроек", "Файловая система");
            }
        }
    }
}