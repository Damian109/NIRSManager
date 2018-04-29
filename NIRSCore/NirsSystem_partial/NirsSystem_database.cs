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
        /// <summary>
        /// Получение Автора
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение списка авторов
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Добавление автора
        /// </summary>
        /// <param name="author"></param>
        public static void AddAuthor(Author author)
        {
            if (!IsDatabaseContextCreated)
                return;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                context.Authors.Add(author);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Изменение автора
        /// </summary>
        /// <param name="author"></param>
        public static void UpdateAuthor(Author author)
        {
            if (!IsDatabaseContextCreated)
                return;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elem = context.Authors.FirstOrDefault(u => u.UserId == author.UserId);
                elem = author;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление автора
        /// </summary>
        /// <param name="author"></param>
        public static void DeleteAuthor(Author author)
        {
            if (!IsDatabaseContextCreated)
                return;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                context.Authors.Remove(author);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Получение организации
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение списка организаций
        /// </summary>
        /// <returns></returns>
        public static List<Organization> GetOrganizations()
        {
            if (!IsDatabaseContextCreated)
                return null;
            List<Organization> result = null;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elems = context.Organizations.ToList();
                result = elems;
            }
            return result;
        }

        /// <summary>
        /// Добавление организации
        /// </summary>
        /// <param name="organization"></param>
        public static void AddOrganization(Organization organization)
        {
            if (!IsDatabaseContextCreated)
                return;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                context.Organizations.Add(organization);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Получение должности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение списка должностей
        /// </summary>
        /// <returns></returns>
        public static List<Position> GetPositions()
        {
            if (!IsDatabaseContextCreated)
                return null;
            List<Position> result = null;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elems = context.Positions.ToList();
                result = elems;
            }
            return result;
        }

        /// <summary>
        /// Добавление должности
        /// </summary>
        /// <param name="position"></param>
        public static void AddPosition(Position position)
        {
            if (!IsDatabaseContextCreated)
                return;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                context.Positions.Add(position);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Получение ученой степени
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static AcademicDegree GetAcademicDegree(int id)
        {
            if (!IsDatabaseContextCreated)
                return null;
            AcademicDegree result = null;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elems = context.AcademicDegrees.Where(u => u.AcademicDegreeId == id).ToList();
                result = elems.FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// Получение списка ученых степеней
        /// </summary>
        /// <returns></returns>
        public static List<AcademicDegree> GetAcademicDegrees()
        {
            if (!IsDatabaseContextCreated)
                return null;
            List<AcademicDegree> result = null;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                var elems = context.AcademicDegrees.ToList();
                result = elems;
            }
            return result;
        }

        /// <summary>
        /// Добавление ученой степени
        /// </summary>
        /// <param name="academicDegree"></param>
        public static void AddAcademicDegree(AcademicDegree academicDegree)
        {
            if (!IsDatabaseContextCreated)
                return;
            using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
            {
                context.AcademicDegrees.Add(academicDegree);
                context.SaveChanges();
            }
        }
    }
}
