using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы конференций
    /// </summary>
    [Table("ConferenceTable")]
    public sealed class Conference
    {
        /// <summary>
        /// Идентификатор конференции
        /// </summary>
        [Key]
        public int ConferenceId { get; set; }

        /// <summary>
        /// Название конференции
        /// </summary>
        [StringLength(200)]
        public string ConferenceName { get; set; }

        /// <summary>
        /// Дата выступления на конференции
        /// </summary>
        public DateTime ConferenceDate { get; set; }
    }
}