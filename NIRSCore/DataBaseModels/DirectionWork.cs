using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы направлений работы
    /// </summary>
    [Table("DirectionWorkTable")]
    public sealed class DirectionWork
    {
        /// <summary>
        /// Идентификатор направления работы
        /// </summary>
        [Key]
        public int DirectionWorkId { get; set; }

        /// <summary>
        /// Идентификатор работы (Внешний ключ)
        /// </summary>
        public int WorkId { get; set; }

        /// <summary>
        /// Идентификатор направления (Внешний ключ)
        /// </summary>
        public int DirectionId { get; set; }
    }
}