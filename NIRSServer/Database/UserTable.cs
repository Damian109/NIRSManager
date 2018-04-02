using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRSServer.Database
{
    /// <summary>
    /// Класс для представления строки таблицы пользователей
    /// </summary>
    [Table("UserTable")]
    public sealed class UserTable
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// Логин пользователя на сервере
        /// </summary>
        [StringLength(80)]
        public string Login { get; set; }

        /// <summary>
        /// Md5-сумма логина и пароля
        /// </summary>
        [StringLength(30)]
        public string Md5 { get; set; }

        /// <summary>
        /// Дата последнего изменения настроек
        /// </summary>
        public DateTime DateEditSetting { get; set; }

        /// <summary>
        /// Дата последнего изменения базы данных
        /// </summary>
        public DateTime DateEditDatabase { get; set; }

        /// <summary>
        /// Название основной базы данных
        /// </summary>
        [StringLength(50)]
        public string NameDatabase { get; set; }
    }
}
