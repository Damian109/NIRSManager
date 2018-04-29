using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы организаций
    /// </summary>
    [Table("OrganizationTable")]
    public sealed class Organization
    {
        /// <summary>
        /// Идентификатор организации
        /// </summary>
        [Key]
        public int OrganizationId { get; set; }

        /// <summary>
        /// Название организации
        /// </summary>
        [StringLength(200)]
        public string OrganizationName { get; set; }
    }
}