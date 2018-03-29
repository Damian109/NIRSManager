using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Отвечает за обработку файла, хранящего лог ошибок
    /// </summary>
    internal sealed class FileErrors : FileCore
    {
        /// <summary>
        /// Список ошибок, возможно и считывать и записывать только в пределах сборки
        /// </summary>
        public List<FileErrorsItem> ErrorsItems { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileErrors()
        {
            _filename = "Errors.log";
        }

        /// <summary>
        /// Открыть файл Лог ошибок
        /// </summary>
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

        /// <summary>
        /// Сохранить файл Лог ошибок
        /// </summary>
        public override void Save()
        {
            Create();

            //Выполняется десериализация в список объектов
            using (FileStream fileStream = new FileStream(_filename, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FileErrorsItem[]));
                serializer.Serialize(fileStream, ErrorsItems.ToArray());
            }
        }
    }
}