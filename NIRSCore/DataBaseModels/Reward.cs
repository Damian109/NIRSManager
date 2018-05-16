using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы наград
    /// </summary>
    [Table("RewardTable")]
    public sealed class Reward
    {
        /// <summary>
        /// Идентификатор награды
        /// </summary>
        [Key]
        public int RewardId { get; set; }

        /// <summary>
        /// Название награды
        /// </summary>
        [StringLength(200)]
        public string RewardName { get; set; }
    }
}