using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSManagerClient.DataBaseModels
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
        /// Направление (Внешний ключ)
        /// </summary>
        public int DirectionId { get; set; }

        /// <summary>
        /// Организация (Внешний ключ)
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Научный руководитель (Внешний ключ)
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Оценка работы
        /// </summary>
        public int Mark { get; set; }

        /// <summary>
        /// Путь к файлу работы
        /// </summary>
        [StringLength(200)]
        public string PathWork { get; set; }

        /// <summary>
        /// Путь к файлу оценки экспертного заключения
        /// </summary>
        [StringLength(200)]
        public string PathMark { get; set; }

        /// <summary>
        /// Название работы
        /// </summary>
        [StringLength(200)]
        public string Name { get; set; }
    }
}