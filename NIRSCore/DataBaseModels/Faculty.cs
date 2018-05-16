using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы факультетов
    /// </summary>
    [Table("FacultyTable")]
    public sealed class Faculty
    {
        /// <summary>
        /// Идентификатор факультета
        /// </summary>
        [Key]
        public int FacultyId { get; set; }

        /// <summary>
        /// Название факультета
        /// </summary>
        [StringLength(200)]
        public string FacultyName { get; set; }
    }
}