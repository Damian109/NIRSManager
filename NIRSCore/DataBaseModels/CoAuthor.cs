using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы соавторства
    /// </summary>
    [Table("CoAuthorTable")]
    public sealed class CoAuthor
    {
        /// <summary>
        /// Идентификатор соавторства
        /// </summary>
        [Key]
        public int CoAuthorId { get; set; }

        /// <summary>
        /// Автор (соавтор)   (Внешний ключ)
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Работа (Внешний ключ)
        /// </summary>
        public int WorkId { get; set; }

        /// <summary>
        /// Вклад участника в работу
        /// </summary>
        public int Contribution { get; set; }
    }
}