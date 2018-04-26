using System.Data.Entity;
using System.Data.SQLite.EF6;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Infrastructure;

namespace NIRSCore.DataBaseModels
{
    /// <summary>
    /// Настройки конфигурации контекста подклкючения к БД
    /// </summary>
    public sealed class ContextConfiguration : DbConfiguration
    {
        public ContextConfiguration()
        {
            SetDefaultConnectionFactory(new SqlConnectionFactory());
            SetProviderServices(NirsSystem.User.DatabaseProviderName, SqlProviderServices.Instance);

            //SetDefaultConnectionFactory(new SQlit());
        }
    }
}
