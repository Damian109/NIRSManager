using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы журналов
    /// </summary>
    [Table("JournalTable")]
    public sealed class Journal
    {
        /// <summary>
        /// Идентификатор журнала
        /// </summary>
        [Key]
        public int JournalId { get; set; }

        /// <summary>
        /// Название журнала
        /// </summary>
        [StringLength(200)]
        public string JournalName { get; set; }

        /// <summary>
        /// Дата публикации статьи в журнале
        /// </summary>
        public DateTime JournalDate { get; set; }
    }
}