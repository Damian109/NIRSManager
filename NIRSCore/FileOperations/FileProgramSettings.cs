using System;
using System.IO;
using NIRSCore.ErrorManager;
using System.Xml.Serialization;

namespace NIRSCore.FileOperations
{
    /// <summary>
    /// Отвечает за обработку файла, хранящего настройки программы
    /// </summary>
    internal sealed class FileProgramSettings : FileCore
    {
        /// <summary>
        /// Общие настройки программы
        /// </summary>
        public ProgramSettings ProgramSettings { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FileProgramSettings()
        {
            _filename = "ProgramSettings.xml";
            ProgramSettings = null;
        }

        /// <summary>
        /// Открыть файл настроек программы
        /// </summary>
        public sealed override void Read()
        {
            if (!File.Exists(_filename))
                return;
            try
            {
                //Выполняется десериализация в список объектов
                using (FileStream fileStream = new FileStream(_filename, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ProgramSettings));
                    ProgramSettings = (ProgramSettings)serializer.Deserialize(fileStream);
                }
            }
            catch (Exception)
            {
                throw new NirsException("Ошибка при загрузке файла настроек программы", "Файл настроек программы", "Файловая система");
            }
        }

        /// <summary>
        /// Сохранить файл настроек программы
        /// </summary>
        public sealed override void Write()
        {
            try
            {
                //Выполняется десериализация в список объектов
                using (FileStream fileStream = new FileStream(_filename, FileMode.OpenOrCreate))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ProgramSettings));
                    serializer.Serialize(fileStream, ProgramSettings);
                }
            }
            catch (Exception)
            {
                throw new NirsException("Ошибка при сохранении файла настроек программы", "Файл настроек программы", "Файловая система");
            }
        }
    }
}