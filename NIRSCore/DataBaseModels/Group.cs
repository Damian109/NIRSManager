using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы групп
    /// </summary>
    [Table("GroupTable")]
    public sealed class Group
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        [Key]
        public int GroupId { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        [StringLength(200)]
        public string GroupName { get; set; }
    }
}