using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы кафедр
    /// </summary>
    [Table("DepartmentTable")]
    public sealed class Department
    {
        /// <summary>
        /// Идентификатор кафедры
        /// </summary>
        [Key]
        public int DepartmentId { get; set; }

        /// <summary>
        /// Название кафедры
        /// </summary>
        [StringLength(200)]
        public string DepartmentName { get; set; }
    }
}