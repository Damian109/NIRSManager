using NIRSCore;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Data.SQLite;
using System.IO;
using System;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    //[DbConfigurationType(typeof(ContextConfiguration))]
    public class ClientDatabaseContext : DbContext
    {
        /// <summary>
        /// Создание контекста данных
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public ClientDatabaseContext(string conn) : base(conn) {
        }

        /// <summary>
        /// Таблица ученых степеней
        /// </summary>
        public virtual DbSet<AcademicDegree> AcademicDegrees { get; set; }

        /// <summary>
        /// Таблица авторов
        /// </summary>
        public virtual DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Таблица соавторства
        /// </summary>
        public virtual DbSet<CoAuthor> CoAuthors { get; set; }

        /// <summary>
        /// Таблица направлений
        /// </summary>
        public virtual DbSet<Direction> Directions { get; set; }

        /// <summary>
        /// Таблица организаций
        /// </summary>
        public virtual DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Таблица должностей
        /// </summary>
        public virtual DbSet<Position> Positions { get; set; }

        /// <summary>
        /// Таблица работ
        /// </summary>
        public virtual DbSet<Work> Works { get; set; }
    }
}