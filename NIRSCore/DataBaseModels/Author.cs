using System;
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
        public int UserId { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [StringLength(80)]
        public string SurName { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [StringLength(80)]
        public string Name { get; set; }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        [StringLength(80)]
        public string SecondName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Ученая степень (Внешний ключ)
        /// </summary>
        public int AcademicDegreeId { get; set; }

        /// <summary>
        /// Должность (Внешний ключ)
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Название организации (Внешний ключ)
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Путь к личному фото
        /// </summary>
        public string PathPhoto { get; set; }
    }
}