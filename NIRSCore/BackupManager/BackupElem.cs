using System;

namespace NIRSCore.BackupManager
{
    /// <summary>
    /// Описание резервной копии
    /// </summary>
    [Serializable]
    public sealed class BackupElem
    {
        /// <summary>
        /// Название резервной копии
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// СУБД
        /// </summary>
        public string DBMSName { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateOfCreate { get; set; }

        public BackupElem() { }
    }
}
