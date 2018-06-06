using System;
using NIRSCore.HelpfulEnumsStructs;

namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Объект содержит информацию о файле для синхронизации с сервером
    /// </summary>
    public class FileInfoData
    {
        /// <summary>
        /// Название файла, включая каталоги
        /// </summary>
        public string NameFile { get; set; }

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        public string PathFile { get; set; }

        /// <summary>
        /// Изменяемый ли файл
        /// </summary>
        public bool IsChanged { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime DateChange { get; set; }

        /// <summary>
        /// Тип файла для скачивания
        /// </summary>
        public FileToUpload FileType { get; set; }

        /// <summary>
        /// Нужно ли отправлять на сервер
        /// </summary>
        public bool IsUpload { get; set; }

        /// <summary>
        /// Нужно ли запрашивать с сервера
        /// </summary>
        public bool IsDownload { get; set; }

        public FileInfoData() { }

        public FileInfoData(string name, string path, bool isChanged, DateTime date, FileToUpload fileType, bool isUp, bool isDown)
        {
            NameFile = name;
            PathFile = path;
            IsChanged = isChanged;
            DateChange = date;
            FileType = fileType;
            IsUpload = isUp;
            IsDownload = isDown;
        }
    }
}
