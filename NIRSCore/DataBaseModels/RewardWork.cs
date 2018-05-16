using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы наград за работу
    /// </summary>
    [Table("RewardWorkTable")]
    public sealed class RewardWork
    {
        /// <summary>
        /// Идентификатор награды за работу
        /// </summary>
        [Key]
        public int RewardWorkId { get; set; }

        /// <summary>
        /// Идентификатор работы (Внешний ключ)
        /// </summary>
        public int WorkId { get; set; }

        /// <summary>
        /// Идентификатор награды (Внешний ключ)
        /// </summary>
        public int RewardId { get; set; }
    }
}