using System.Collections.Generic;

namespace NIRSCore.Syncronization
{
    /// <summary>
    /// Объект представляет список всех файлов, которые могут быть синхронизированы
    /// </summary>
    public sealed class ListFileInfoData
    {
        /// <summary>
        /// Список информации о файлах
        /// </summary>
        public List<FileInfoData> FilesInfo { get; set; }

        public ListFileInfoData() { }

        public ListFileInfoData(List<FileInfoData> data) => FilesInfo = data;
    }
}
