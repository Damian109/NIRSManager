using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы работ
    /// </summary>
    [Table("WorkTable")]
    public sealed class Work
    {
        /// <summary>
        /// Идентификатор работы
        /// </summary>
        [Key]
        public int WorkId { get; set; }

        /// <summary>
        /// Название работы
        /// </summary>
        [StringLength(200)]
        public string WorkName { get; set; }

        /// <summary>
        /// Идентификатор руководителя (Внешний ключ)
        /// </summary>
        public int? HeadAuthorId { get; set; }

        /// <summary>
        /// Путь к файлу работы
        /// </summary>
        [StringLength(200)]
        public string WorkPath { get; set; }

        /// <summary>
        /// Оценка работы
        /// </summary>
        public int WorkMark { get; set; }

        /// <summary>
        /// Объем работы
        /// </summary>
        public double WorkSize { get; set; }

        /// <summary>
        /// Идентификатор журнала (Внешний ключ)
        /// </summary>
        public int? JournalId { get; set; }

        /// <summary>
        /// Идентификатор конференции (Внешний ключ)
        /// </summary>
        public int? ConferenceId { get; set; }
    }
}