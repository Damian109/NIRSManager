using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы ученых степеней
    /// </summary>
    [Table("AcademicDegreeTable")]
    public sealed class AcademicDegree
    {
        /// <summary>
        /// Идентификатор ученой степени
        /// </summary>
        [Key]
        public int AcademicDegreeId { get; set; }

        /// <summary>
        /// Название ученой степени
        /// </summary>
        [StringLength(100)]
        public string AcademicDegreeName { get; set; }
    }
}
