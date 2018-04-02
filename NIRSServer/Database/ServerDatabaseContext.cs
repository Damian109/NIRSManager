using System.Data.Entity;

namespace NIRSServer.Database
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class ServerDatabaseContext : DbContext
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public ServerDatabaseContext(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Таблица пользователей
        /// </summary>
        public virtual DbSet<UserTable> Users { get; set; }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        public void AddUser(UserTable user)
        {
            Users.Add(user);
            SaveChanges();
        }

        /// <summary>
        /// Таблица резервных копий
        /// </summary>
        public virtual DbSet<BackupTable> Backups { get; set; }

        /// <summary>
        /// Добавление новой резервной копии
        /// </summary>
        /// <param name="backup">Резервная копия</param>
        public void AddBackup(BackupTable backup)
        {
            Backups.Add(backup);
            SaveChanges();
        }

        /// <summary>
        /// Таблица обменов базами данных
        /// </summary>
        public virtual DbSet<DatabaseExchangeTable> Exchanges { get; set; }

        /// <summary>
        /// Добавление нового обмена
        /// </summary>
        /// <param name="exchange">Обмен</param>
        public void AddExchange(DatabaseExchangeTable exchange)
        {
            Exchanges.Add(exchange);
            SaveChanges();
        }
    }
}