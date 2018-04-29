using NIRSCore.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSCore
{
    public static partial class NirsSystem
    {
        public static Author GetAuthor(int id)
        {
            if (!IsDatabaseContextCreated)
                return null;
            Author result = null;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elems = context.Authors.Where(u => u.UserId == id).ToList();
                result = elems.FirstOrDefault();
            }
            return result;
        }

        public static List<Author> GetAuthors()
        {
            if (!IsDatabaseContextCreated)
                return null;
            List<Author> result = null;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elems = context.Authors.ToList();
                result = elems;
            }
            return result;
        }

        public static Organization GetOrganization(int id)
        {
            if (!IsDatabaseContextCreated)
                return null;
            Organization result = null;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elems = context.Organizations.Where(u => u.OrganizationId == id).ToList();
                result = elems.FirstOrDefault();
            }
            return result;
        }

        public static Position GetPosition(int id)
        {
            if (!IsDatabaseContextCreated)
                return null;
            Position result = null;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elems = context.Positions.Where(u => u.PositionId == id).ToList();
                result = elems.FirstOrDefault();
            }
            return result;
        }
    }
}
