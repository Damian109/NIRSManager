using System.IO;
using System.Linq;
using NIRSCore.BackupManager;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Отвечает за обработку файла, хранящего описание резервной копии
    /// </summary>
    public sealed class FileBackupReadme : FileCore
    {
        /// <summary>
        /// Описание резервной копии
        /// </summary>
        public BackupElem BackupElement { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileBackupReadme(string login) => _filename = "data//" + login + "//temp//back";

        /// <summary>
        /// Открыть файл
        /// </summary>
        public sealed override void Read()
        {
            if (!File.Exists(_filename))
                return;
            //Выполняется десериализация
            using (FileStream fileStream = new FileStream(_filename, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BackupElem));
                BackupElement = (BackupElem)serializer.Deserialize(fileStream);
            }
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public sealed override void Write()
        {
            //Выполняется сериализация
            using (FileStream fileStream = new FileStream(_filename, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BackupElem));
                serializer.Serialize(fileStream, BackupElement);
            }
        }
    }
}
