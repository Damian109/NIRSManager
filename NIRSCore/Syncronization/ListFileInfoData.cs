namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Объект представляет список всех файлов, которые могут быть синхронизированы
    /// </summary>
    public sealed class ListFileInfoData
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Список информации о файлах
        /// </summary>
        public FileInfoData[] FilesInfo { get; set; }

        public ListFileInfoData() => FilesInfo = null;

        public ListFileInfoData(FileInfoData[] data) => FilesInfo = data;
    }
}
