using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы должностей
    /// </summary>
    [Table("PositionTable")]
    public sealed class Position
    {
        /// <summary>
        /// Идентификатор должности
        /// </summary>
        [Key]
        public int PositionId { get; set; }

        /// <summary>
        /// Название должности
        /// </summary>
        [StringLength(100)]
        public string PositionName { get; set; }
    }
}
