using System.Data.Entity;

namespace NIRSManagerServer.Models
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class ServerDatabaseContext : DbContext
    {
        /// <summary>
        /// Функция-некий костыль, чтобы избежать миграций
        /// </summary>
        /// <param name="modelBuilder"></param>
        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ServerDatabaseContext>(null);
            base.OnModelCreating(modelBuilder);
        }*/

        /// <summary>
        /// Таблица пользователей
        /// </summary>
        public virtual DbSet<UserTable> UserTables { get; set; }

        /// <summary>
        /// Таблица резервных копий
        /// </summary>
        public virtual DbSet<BackupTable> BackupTables { get; set; }

        /// <summary>
        /// Таблица обменов базами данных
        /// </summary>
        public virtual DbSet<DatabaseExchangeTable> DatabaseExchangeTables { get; set; }
    }
}