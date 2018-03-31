using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NIRSCore.FileOperations
{
    internal sealed class FileUsers : FileCore
    {
        public List<FileUsersItem> usersItems;

        public override void Open()
        {
            Create();

            //Выполняется десериализация в список объектов
            using (FileStream fileStream = new FileStream(_filename, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FileErrorsItem[]));
                FileErrorsItem[] items = (FileErrorsItem[])serializer.Deserialize(fileStream);
                ErrorsItems = items.ToList();
            }
        }

        public override void Save()
        {
            base.Save();
        }
    }
}

// A: encrypting when writing
// 1. create backing storage stream. In your case a file stream
using(Stream innerStream = File.Create(path))
// 2. create a CryptoStream in write mode
using(Stream cryptoStream = new CryptoStream(innerStream, encryptor, CryptoStreamMode.Write))
{
    // 3. write to the cryptoStream
    binaryFormatter.Serialize(cryptoStream, obj);
}

// B: decrypting when reading
// 1. create backing storage stream. In your case a file stream
using(Stream innerStream = File.Open(path, FileMode.Open))
// 2. create a CryptoStream in read mode
using(Stream cryptoStream = new CryptoStream(innerStream, decryptor, CryptoStreamMode.Read))
{
    // 3. read from the cryptoStream
    obj = binaryFormatter.Deserialize(cryptoStream);
}



/*

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
}*/