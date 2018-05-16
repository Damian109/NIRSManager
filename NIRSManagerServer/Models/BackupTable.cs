using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSManagerServer.Models
{
    /// <summary>
    /// Класс для представления строки таблицы резервных копий
    /// </summary>
    [Table("BackupTable")]
    public sealed class BackupTable
    {
        /// <summary>
        /// Идентификатор резервной копии
        /// </summary>
        [Key]
        public int BackupId { get; set; }

        /// <summary>
        /// Идентификатор пользователя (внешний ключ)
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Название резервной копии на сервере
        /// </summary>
        [StringLength(100)]
        public string BackupName { get; set; }

        /// <summary>
        /// Дата создания резервной копии
        /// </summary>
        public DateTime? DateOfCreate { get; set; }
    }
}