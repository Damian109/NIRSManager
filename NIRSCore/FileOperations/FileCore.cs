using System.IO;

namespace NIRSCore
{
    /// <summary>
    /// Абстрактный класс, предоставляющий потомкам возможности по редактированию файлов
    /// </summary>
    public abstract class FileCore
    {
        //Название файла для работы
        protected string _filename;

        /// <summary>
        /// Удаление файла
        /// </summary>
        public void Delete()
        {
            if (File.Exists(_filename))
                File.Delete(_filename);
        }

        /// <summary>
        /// Создание файла
        /// </summary>
        public void Create()
        {
            if (!File.Exists(_filename))
                File.Create(_filename);
        }

        /// <summary>
        /// Реализация закрытия соединения с файлом
        /// </summary>
        public void Close()
        {
            Create();
            Write();
        }

        /// <summary>
        /// Открытие и десериализация файла
        /// </summary>
        public abstract void Read();

        /// <summary>
        /// Сериализация файла
        /// </summary>
        public abstract void Write();
    }
}
