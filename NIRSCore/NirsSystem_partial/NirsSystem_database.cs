using System;
using System.Linq;
using System.Threading.Tasks;
using NIRSCore.DataBaseModels;
using NIRSCore.FileOperations;

namespace NIRSCore
{
    public static partial class NirsSystem
    {
        /// <summary>
        /// Инициализация базы данных некоторыми изначальными данными
        /// </summary>
        public async static void InitialiseDB()
        {
            await Task.Run(() =>
            {

                FileInitialiser file = new FileInitialiser();
                file.Read();
                if (file.Items.Count < 1)
                    return;
                foreach (var elem in file.Items)
                {
                    switch (elem.TableName)
                    {
                        case "Организация":
                            AddObject(new Organization { OrganizationName = elem.ValueName });
                            break;
                        case "Факультет":
                            AddObject(new Faculty { FacultyName = elem.ValueName });
                            break;
                        case "Кафедра":
                            AddObject(new Department { DepartmentName = elem.ValueName });
                            break;
                        case "Группа":
                            AddObject(new Group { GroupName = elem.ValueName });
                            break;
                        case "Должность":
                            AddObject(new Position { PositionName = elem.ValueName });
                            break;
                        case "Степень":
                            AddObject(new AcademicDegree { AcademicDegreeName = elem.ValueName });
                            break;
                        case "Направление":
                            AddObject(new Direction { DirectionName = elem.ValueName });
                            break;
                        case "Награда":
                            AddObject(new Reward { RewardName = elem.ValueName });
                            break;
                        default:
                            break;
                    }
                }
            });
        }

        /// <summary>
        /// Добавление элемента в базу данных
        /// </summary>
        /// <param name="obj">Любой преобразованный элемент из моделей БД</param>
        public static void AddObject(object obj)
        {
            try
            {
                using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
                {
                    if (obj is Organization)
                        context.Organizations.Add((Organization)obj);
                    if (obj is Faculty)
                        context.Faculties.Add((Faculty)obj);
                    if(obj is Department)
                        context.Departments.Add((Department)obj);
                    if(obj is Group)
                        context.Groups.Add((Group)obj);
                    if(obj is Position)
                        context.Positions.Add((Position)obj);
                    if(obj is AcademicDegree)
                        context.AcademicDegrees.Add((AcademicDegree)obj);
                    if (obj is Author)
                        context.Authors.Add((Author)obj);
                    if (obj is Journal)
                        context.Journals.Add((Journal)obj);
                    if (obj is Conference)
                        context.Conferences.Add((Conference)obj);
                    if (obj is Work)
                        context.Works.Add((Work)obj);
                    if (obj is Direction)
                        context.Directions.Add((Direction)obj);
                    if (obj is Reward)
                        context.Rewards.Add((Reward)obj);
                    if (obj is CoAuthor)
                        context.CoAuthors.Add((CoAuthor)obj);
                    if (obj is DirectionWork)
                        context.DirectionWorks.Add((DirectionWork)obj);
                    if (obj is RewardWork)
                        context.RewardWorks.Add((RewardWork)obj);
                    context.SaveChanges();
                }
                IsDatabaseContextCreated = true;
                User.DateLastEditDatabase = DateTime.Now;
            }
            catch (Exception)
            {
                ErrorManager.ExecuteException(new ErrorManager.NirsException("Ошибка при добавлении элемента",
                    "База данных", "Работа с базой данных"));
            }
        }

        /// <summary>
        /// Поиск объекта
        /// </summary>
        /// <typeparam name="T">Тип модели базы данных</typeparam>
        /// <param name="id">Идентификатор записи</param>
        /// <returns>Объект по типу модели</returns>
        public static object GetObject<T>(int id)
        {
            if (!IsDatabaseContextCreated)
                return null;
            try
            {
                using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
                {
                    if (typeof(T) == typeof(Organization))
                        return context.Organizations.Where(u => u.OrganizationId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Faculty))
                        return context.Faculties.Where(u => u.FacultyId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Department))
                        return context.Departments.Where(u => u.DepartmentId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Group))
                        return context.Groups.Where(u => u.GroupId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Position))
                        return context.Positions.Where(u => u.PositionId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(AcademicDegree))
                        return context.AcademicDegrees.Where(u => u.AcademicDegreeId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Author))
                        return context.Authors.Where(u => u.AuthorId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Journal))
                        return context.Journals.Where(u => u.JournalId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Conference))
                        return context.Conferences.Where(u => u.ConferenceId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Work))
                        return context.Works.Where(u => u.WorkId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Direction))
                        return context.Directions.Where(u => u.DirectionId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(Reward))
                        return context.Rewards.Where(u => u.RewardId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(CoAuthor))
                        return context.CoAuthors.Where(u => u.CoAuthorId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(DirectionWork))
                        return context.DirectionWorks.Where(u => u.DirectionWorkId == id).ToList().FirstOrDefault();
                    if (typeof(T) == typeof(RewardWork))
                        return context.RewardWorks.Where(u => u.RewardWorkId == id).ToList().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                ErrorManager.ExecuteException(new ErrorManager.NirsException("Ошибка при поиске элемента",
                    "База данных", "Работа с базой данных"));
            }
            return null;
        }

        /// <summary>
        /// Получение списка объектов
        /// </summary>
        /// <typeparam name="T">Тип модели базы данных</typeparam>
        /// <returns></returns>
        public static object GetListObject<T>()
        {
            if (!IsDatabaseContextCreated)
                return null;
            try
            {
                using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
                {
                    if (typeof(T) == typeof(Organization))
                        return context.Organizations.ToList();
                    if (typeof(T) == typeof(Faculty))
                        return context.Faculties.ToList();
                    if (typeof(T) == typeof(Department))
                        return context.Departments.ToList();
                    if (typeof(T) == typeof(Group))
                        return context.Groups.ToList();
                    if (typeof(T) == typeof(Position))
                        return context.Positions.ToList();
                    if (typeof(T) == typeof(AcademicDegree))
                        return context.AcademicDegrees.ToList();
                    if (typeof(T) == typeof(Author))
                        return context.Authors.ToList();
                    if (typeof(T) == typeof(Journal))
                        return context.Journals.ToList();
                    if (typeof(T) == typeof(Conference))
                        return context.Conferences.ToList();
                    if (typeof(T) == typeof(Work))
                        return context.Works.ToList();
                    if (typeof(T) == typeof(Direction))
                        return context.Directions.ToList();
                    if (typeof(T) == typeof(Reward))
                        return context.Rewards.ToList();
                    if (typeof(T) == typeof(CoAuthor))
                        return context.CoAuthors.ToList();
                    if (typeof(T) == typeof(DirectionWork))
                        return context.DirectionWorks.ToList();
                    if (typeof(T) == typeof(RewardWork))
                        return context.RewardWorks.ToList();
                }
            }
            catch (Exception)
            {
                ErrorManager.ExecuteException(new ErrorManager.NirsException("Ошибка при выборке данных",
                    "База данных", "Работа с базой данных"));
            }
            return null;
        }

        /// <summary>
        /// Изменение элемента
        /// </summary>
        /// <param name="obj">Любой преобразованный элемент из моделей БД</param>
        public static void UpdateObject(object obj)
        {
            if (!IsDatabaseContextCreated)
                return;
            try
            {
                using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
                {
                    if (obj is Organization org)
                    {
                        var query = context.Organizations.FirstOrDefault(u => u.OrganizationId == org.OrganizationId);
                        query.OrganizationName = org.OrganizationName;
                    }
                    if (obj is Faculty fac)
                    {
                        var query = context.Faculties.FirstOrDefault(u => u.FacultyId == fac.FacultyId);
                        query.FacultyName = fac.FacultyName;
                    }
                    if (obj is Department dep)
                    {
                        var query = context.Departments.FirstOrDefault(u => u.DepartmentId == dep.DepartmentId);
                        query.DepartmentName = dep.DepartmentName;
                    }
                    if (obj is Group gro)
                    {
                        var query = context.Groups.FirstOrDefault(u => u.GroupId == gro.GroupId);
                        query.GroupName = gro.GroupName;
                    }
                    if (obj is Position pos)
                    {
                        var query = context.Positions.FirstOrDefault(u => u.PositionId == pos.PositionId);
                        query.PositionName = pos.PositionName;
                    }
                    if (obj is AcademicDegree aca)
                    {
                        var query = context.AcademicDegrees.FirstOrDefault(u => u.AcademicDegreeId == aca.AcademicDegreeId);
                        query.AcademicDegreeName = aca.AcademicDegreeName;
                    }
                    if (obj is Author aut)
                    {
                        var query = context.Authors.FirstOrDefault(u => u.AuthorId == aut.AuthorId);
                        query.AcademicDegreeId = aut.AcademicDegreeId;
                        query.AuthorName = aut.AuthorName;
                        query.DepartmentId = aut.DepartmentId;
                        query.FacultyId = aut.FacultyId;
                        query.GroupId = aut.GroupId;
                        query.OrganizationId = aut.OrganizationId;
                        query.PhotoPath = aut.PhotoPath;
                        query.PositionId = aut.PositionId;
                    }
                    if (obj is Journal jou)
                    {
                        var query = context.Journals.FirstOrDefault(u => u.JournalId == jou.JournalId);
                        query.JournalName = jou.JournalName;
                        query.JournalDate = jou.JournalDate;
                    }
                    if (obj is Conference con)
                    {
                        var query = context.Conferences.FirstOrDefault(u => u.ConferenceId == con.ConferenceId);
                        query.ConferenceName = con.ConferenceName;
                        query.ConferenceDate = con.ConferenceDate;
                    }
                    if (obj is Work wor)
                    {
                        var query = context.Works.FirstOrDefault(u => u.WorkId == wor.WorkId);
                        query.ConferenceId = wor.ConferenceId;
                        query.HeadAuthorId = wor.HeadAuthorId;
                        query.JournalId = wor.JournalId;
                        query.WorkMark = wor.WorkMark;
                        query.WorkName = wor.WorkName;
                        query.WorkPath = wor.WorkPath;
                        query.WorkSize = wor.WorkSize;
                    }
                    if (obj is Direction dir)
                    {
                        var query = context.Directions.FirstOrDefault(u => u.DirectionId == dir.DirectionId);
                        query.DirectionName = dir.DirectionName;
                    }
                    if (obj is Reward rew)
                    {
                        var query = context.Rewards.FirstOrDefault(u => u.RewardId == rew.RewardId);
                        query.RewardName = rew.RewardName;
                    }
                    if (obj is CoAuthor coa)
                    {
                        var query = context.CoAuthors.FirstOrDefault(u => u.CoAuthorId == coa.CoAuthorId);
                        query.AuthorId = coa.AuthorId;
                        query.Contribution = coa.Contribution;
                        query.WorkId = coa.WorkId;
                    }
                    if (obj is DirectionWork diw)
                    {
                        var query = context.DirectionWorks.FirstOrDefault(u => u.DirectionWorkId == diw.DirectionWorkId);
                        query.DirectionId = diw.DirectionId;
                        query.WorkId = diw.WorkId;
                    }
                    if (obj is RewardWork elem)
                    {
                        var query = context.RewardWorks.FirstOrDefault(u => u.RewardWorkId == elem.RewardWorkId);
                        query.RewardId = elem.RewardId;
                        query.WorkId = elem.WorkId;
                    }
                    context.SaveChanges();
                    User.DateLastEditDatabase = DateTime.Now;
                }
            }
            catch (Exception)
            {
                ErrorManager.ExecuteException(new ErrorManager.NirsException("Ошибка при изменении элемента",
                    "База данных", "Работа с базой данных"));
            }
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="obj">Любой преобразованный элемент из моделей БД</param>
        public static void DeleteObject(object obj)
        {
            if (!IsDatabaseContextCreated)
                return;
            try
            {
                using (ClientDatabaseContext context = new ClientDatabaseContext(User.ConnectionString))
                {
                    if (obj is Organization org)
                    {
                        var query = context.Organizations.FirstOrDefault(u => u.OrganizationId == org.OrganizationId);
                        if (query != null)
                            context.Organizations.Remove(query);
                    }
                    if (obj is Faculty fac)
                    {
                        var query = context.Faculties.FirstOrDefault(u => u.FacultyId == fac.FacultyId);
                        if (query != null)
                            context.Faculties.Remove(query);
                    }
                    if (obj is Department dep)
                    {
                        var query = context.Departments.FirstOrDefault(u => u.DepartmentId == dep.DepartmentId);
                        if (query != null)
                            context.Departments.Remove(query);
                    }
                    if (obj is Group gro)
                    {
                        var query = context.Groups.FirstOrDefault(u => u.GroupId == gro.GroupId);
                        if (query != null)
                            context.Groups.Remove(query);
                    }
                    if (obj is Position pos)
                    {
                        var query = context.Positions.FirstOrDefault(u => u.PositionId == pos.PositionId);
                        if (query != null)
                            context.Positions.Remove(query);
                    }
                    if (obj is AcademicDegree aca)
                    {
                        var query = context.AcademicDegrees.FirstOrDefault(u => u.AcademicDegreeId == aca.AcademicDegreeId);
                        if (query != null)
                            context.AcademicDegrees.Remove(query);
                    }
                    if (obj is Author aut)
                    {
                        var query = context.Authors.FirstOrDefault(u => u.AuthorId == aut.AuthorId);
                        if (query != null)
                            context.Authors.Remove(query);
                    }
                    if (obj is Journal jou)
                    {
                        var query = context.Journals.FirstOrDefault(u => u.JournalId == jou.JournalId);
                        if (query != null)
                            context.Journals.Remove(query);
                    }
                    if (obj is Conference con)
                    {
                        var query = context.Conferences.FirstOrDefault(u => u.ConferenceId == con.ConferenceId);
                        if (query != null)
                            context.Conferences.Remove(query);
                    }
                    if (obj is Work wor)
                    {
                        var query = context.Works.FirstOrDefault(u => u.WorkId == wor.WorkId);
                        if (query != null)
                            context.Works.Remove(query);
                    }
                    if (obj is Direction dir)
                    {
                        var query = context.Directions.FirstOrDefault(u => u.DirectionId == dir.DirectionId);
                        if (query != null)
                            context.Directions.Remove(query);
                    }
                    if (obj is Reward rew)
                    {
                        var query = context.Rewards.FirstOrDefault(u => u.RewardId == rew.RewardId);
                        if (query != null)
                            context.Rewards.Remove(query);
                    }
                    if (obj is CoAuthor coa)
                    {
                        var query = context.CoAuthors.FirstOrDefault(u => u.CoAuthorId == coa.CoAuthorId);
                        if (query != null)
                            context.CoAuthors.Remove(query);
                    }
                    if (obj is DirectionWork diw)
                    {
                        var query = context.DirectionWorks.FirstOrDefault(u => u.DirectionWorkId == diw.DirectionWorkId);
                        if (query != null)
                            context.DirectionWorks.Remove(query);
                    }
                    if (obj is RewardWork elem)
                    {
                        var query = context.RewardWorks.FirstOrDefault(u => u.RewardWorkId == elem.RewardWorkId);
                        if (query != null)
                            context.RewardWorks.Remove(query);
                    }
                    context.SaveChanges();
                    User.DateLastEditDatabase = DateTime.Now;
                }
            }
            catch (Exception)
            {
                ErrorManager.ExecuteException(new ErrorManager.NirsException("Ошибка при удалении элемента",
                    "База данных", "Работа с базой данных"));
            }
        }
    }
}
