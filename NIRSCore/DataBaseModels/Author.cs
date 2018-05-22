using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Класс для представления строки таблицы авторов, которые могут являться руководителями или соавторами
    /// </summary>
    [Table("AuthorTable")]
    public sealed class Author
    {
        /// <summary>
        /// Идентификатор автора
        /// </summary>
        [Key]
        public int AuthorId { get; set; }

        /// <summary>
        /// ФИО автора
        /// </summary>
        [StringLength(250)]
        public string AuthorName { get; set; }

        /// <summary>
        /// Идентификатор организации (Внешний ключ)
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Идентификатор факультета (Внешний ключ)
        /// </summary>
        public int? FacultyId { get; set; }

        /// <summary>
        /// Идентификатор кафедры (Внешний ключ)
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Идентификатор группы (Внешний ключ)
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Идентификатор должности (Внешний ключ)
        /// </summary>
        public int? PositionId { get; set; }

        /// <summary>
        /// Идентификатор ученой степени (Внешний ключ)
        /// </summary>
        public int? AcademicDegreeId { get; set; }

        /// <summary>
        /// Путь к личному фото
        /// </summary>
        public string PhotoPath { get; set; }
    }
}