using System.Data.Entity;

namespace NIRSManagerClient.DataBaseModels
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class ClientDatabaseContext : DbContext
    {
        /// <summary>
        /// Функция-некий костыль, чтобы избежать миграций
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ClientDatabaseContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Конструктор контекста
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public ClientDatabaseContext(string connectionString) : base(connectionString) { }

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