using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы направлений
    /// </summary>
    [Table("DirectionTable")]
    public sealed class Direction
    {
        /// <summary>
        /// Идентификатор направления
        /// </summary>
        [Key]
        public int DirectionId { get; set; }

        /// <summary>
        /// Название направления
        /// </summary>
        [StringLength(300)]
        public string DirectionName { get; set; }
    }
}
