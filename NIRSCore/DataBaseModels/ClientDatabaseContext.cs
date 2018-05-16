using System.Data.Entity;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class ClientDatabaseContext : DbContext
    {
        /// <summary>
        /// Создание контекста данных
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public ClientDatabaseContext(string conn) : base(conn) { }

        /// <summary>
        /// Таблица организаций
        /// </summary>
        public virtual DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Таблица факультетов
        /// </summary>
        public virtual DbSet<Faculty> Faculties { get; set; }

        /// <summary>
        /// Таблица кафедр
        /// </summary>
        public virtual DbSet<Department> Departments { get; set; }

        /// <summary>
        /// Таблица групп
        /// </summary>
        public virtual DbSet<Group> Groups { get; set; }

        /// <summary>
        /// Таблица должностей
        /// </summary>
        public virtual DbSet<Position> Positions { get; set; }

        /// <summary>
        /// Таблица ученых степеней
        /// </summary>
        public virtual DbSet<AcademicDegree> AcademicDegrees { get; set; }

        /// <summary>
        /// Таблица авторов
        /// </summary>
        public virtual DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Таблица журналов
        /// </summary>
        public virtual DbSet<Journal> Journals { get; set; }

        /// <summary>
        /// Таблица конференций
        /// </summary>
        public virtual DbSet<Conference> Conferences { get; set; }

        /// <summary>
        /// Таблица работ
        /// </summary>
        public virtual DbSet<Work> Works { get; set; }

        /// <summary>
        /// Таблица направлений
        /// </summary>
        public virtual DbSet<Direction> Directions { get; set; }

        /// <summary>
        /// Таблица наград
        /// </summary>
        public virtual DbSet<Reward> Rewards { get; set; }

        /// <summary>
        /// Таблица соавторства
        /// </summary>
        public virtual DbSet<CoAuthor> CoAuthors { get; set; }

        /// <summary>
        /// Таблица направлений работ
        /// </summary>
        public virtual DbSet<DirectionWork> DirectionWorks { get; set; }

        /// <summary>
        /// Таблица наград работ
        /// </summary>
        public virtual DbSet<RewardWork> RewardWorks { get; set; }
    }
}