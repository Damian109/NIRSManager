using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Отвечает за обработку файла, хранящего данные для инициализации БД
    /// </summary>
    internal sealed class FileInitialiser : FileCore
    {
        /// <summary>
        /// Список элементов для инициализации
        /// </summary>
        public List<FileInitialiserItem> Items { get; set; }

        public FileInitialiser()
        {
            _filename = "Initialise.init";
            Items = new List<FileInitialiserItem>();
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        public sealed override void Read()
        {
            if (!File.Exists(_filename))
                return;
            //Выполняется десериализация в список объектов
            using (FileStream fileStream = new FileStream(_filename, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FileInitialiserItem[]));
                FileInitialiserItem[] items = (FileInitialiserItem[])serializer.Deserialize(fileStream);
                Items = items.ToList();
            }
        }

        /// <summary>
        /// Сохранить файл (Не используется)
        /// </summary>
        public sealed override void Write() { }
    }
}