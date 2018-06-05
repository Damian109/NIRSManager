namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Представляет файл и его название
    /// </summary>
    public sealed class FileData
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public string FileBytes { get; set; }

        public FileData()
        {
        }

        public FileData(string name, string filebytes)
        {
            FileName = name;
            FileBytes = filebytes;
        }
    }
}
