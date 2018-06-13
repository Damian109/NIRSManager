using System;
using NIRSCore;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using System.Windows;
using Microsoft.Win32;
using System.Threading;
using System.Threading.Tasks;
using NIRSCore.DataBaseModels;
using NIRSCore.StackOperations;
using System.Collections.Generic;

namespace NIRSManagerClient.ViewModels
{
    /// <summary>
    /// Модель представления страницы импорта
    /// </summary>
    public sealed class ImportViewModel : ViewModel
    {
        public ImportViewModel() : base("Отчеты") { }

        /// <summary>
        /// Выполняется ли какая-то операция (Видимость)
        /// </summary>
        public Visibility IsDone { get; private set; } = Visibility.Hidden;

        /// <summary>
        /// Статус выполнения операции
        /// </summary>
        public string StatusString { get; private set; }

        /// <summary>
        /// Команда импорта
        /// </summary>
        public RelayCommand CommandImport
        {
            get => new RelayCommand(obj =>
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    DefaultExt = ".xlsx",
                    Filter = "Xlsx (.xlsx)|*.xlsx"
                };
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objDone => ImportXlsx(dialog.FileName), null);

                    //Создание операции
                    Operation operation = new Operation("Импорт информации", done, null);

                    NirsSystem.StackOperations.AddOperation(operation);
                    operation.DoneCommand.Execute(null);
                }

            });
        }

        /// <summary>
        /// Получение массива строк из одной строки
        /// </summary>
        /// <param name="stringFromCell">Объединенная строка</param>
        /// <returns></returns>
        private string[] StringSplit(string stringFromCell)
        {
            if (stringFromCell == null)
                return null;

            //Создаем и заполняем массив авторов
            string[] stringArray;

            if (stringFromCell.Contains(";"))
                stringArray = stringFromCell.Split(';');
            else
            {
                stringArray = new string[1];
                stringArray[0] = stringFromCell;
            }
            return stringArray;
        }

        /// <summary>
        /// Проверка нахождения работы с таким названием в БД
        /// </summary>
        /// <param name="workName">Название работы</param>
        /// <returns></returns>
        private bool IsWorkDatabase(string workName)
        {
            List<Work> works = (List<Work>)NirsSystem.GetListObject<Work>();
            if (works != null)
                foreach (var elem in works)
                    if (elem.WorkName == workName)
                        return true;
            return false;
        }

        /// <summary>
        /// Получение факультета
        /// </summary>
        /// <param name="name">Название фаультета</param>
        /// <returns></returns>
        private Faculty GetFaculty(string name)
        {
            List<Faculty> faculties = (List<Faculty>)NirsSystem.GetListObject<Faculty>();
            if (faculties != null)
                foreach (var elem in faculties)
                    if (elem.FacultyName == name)
                        return elem;
            NirsSystem.AddObject(new Faculty { FacultyName = name });
            faculties = (List<Faculty>)NirsSystem.GetListObject<Faculty>();
            return faculties.First(u => u.FacultyName == name);
        }

        /// <summary>
        /// Получение кафедры
        /// </summary>
        /// <param name="name">Название кафедры</param>
        /// <returns></returns>
        private Department GetDepartment(string name)
        {
            List<Department> departments = (List<Department>)NirsSystem.GetListObject<Department>();
            if (departments != null)
                foreach (var elem in departments)
                    if (elem.DepartmentName == name)
                        return elem;
            NirsSystem.AddObject(new Department { DepartmentName = name });
            departments = (List<Department>)NirsSystem.GetListObject<Department>();
            return departments.First(u => u.DepartmentName == name);
        }

        /// <summary>
        /// Получение автора
        /// </summary>
        /// <param name="name">ФИО автора</param>
        /// <param name="faculty">Факультет</param>
        /// <param name="department">Кафедра</param>
        /// <returns></returns>
        private Author GetAuthor(string name, Faculty faculty, Department department)
        {
            List<Author> authors = (List<Author>)NirsSystem.GetListObject<Author>();
            if (authors != null)
                foreach (var elem in authors)
                    if (elem.AuthorName == name)
                        return elem;
            NirsSystem.AddObject(new Author { AuthorName = name, DepartmentId = department.DepartmentId, FacultyId = faculty.FacultyId });
            authors = (List<Author>)NirsSystem.GetListObject<Author>();
            return authors.First(u => u.AuthorName == name);
        }

        /// <summary>
        /// Получение направления
        /// </summary>
        /// <param name="name">Название направления</param>
        /// <returns></returns>
        private Direction GetDirection(string name)
        {
            List<Direction> directions = (List<Direction>)NirsSystem.GetListObject<Direction>();
            if (directions != null)
                foreach (var elem in directions)
                    if (elem.DirectionName == name)
                        return elem;
            NirsSystem.AddObject(new Direction { DirectionName = name });
            directions = (List<Direction>)NirsSystem.GetListObject<Direction>();
            return directions.First(u => u.DirectionName == name);
        }

        /// <summary>
        /// Получение награды
        /// </summary>
        /// <param name="name">Название награды</param>
        /// <returns></returns>
        private Reward GetReward(string name)
        {
            List<Reward> rewards = (List<Reward>)NirsSystem.GetListObject<Reward>();
            if (rewards != null)
                foreach (var elem in rewards)
                    if (elem.RewardName == name)
                        return elem;
            NirsSystem.AddObject(new Reward { RewardName = name });
            rewards = (List<Reward>)NirsSystem.GetListObject<Reward>();
            return rewards.First(u => u.RewardName == name);
        }

        /// <summary>
        /// Добавление соавторов
        /// </summary>
        /// <param name="work">работа</param>
        /// <param name="authors">авторы</param>
        private void AddToCoauthors(Work work, List<Author> authors)
        {
            if (authors.Count > 0)
                foreach (var elem in authors)
                    NirsSystem.AddObject(new CoAuthor { AuthorId = elem.AuthorId, Contribution = 100, WorkId = work.WorkId });
        }

        /// <summary>
        /// Добавление сонаправлений
        /// </summary>
        /// <param name="work">работа</param>
        /// <param name="directions">направления</param>
        private void AddToDirectionWork(Work work, List<Direction> directions)
        {
            if (directions.Count > 0)
                foreach (var elem in directions)
                    NirsSystem.AddObject(new DirectionWork { DirectionId = elem.DirectionId, WorkId = work.WorkId });
        }

        /// <summary>
        /// Добавление сонаград
        /// </summary>
        /// <param name="work">работа</param>
        /// <param name="rewards">награды</param>
        private void AddToRewardWork(Work work, List<Reward> rewards)
        {
            if (rewards.Count > 0)
                foreach (var elem in rewards)
                    NirsSystem.AddObject(new RewardWork { RewardId = elem.RewardId, WorkId = work.WorkId });
        }

        /// <summary>
        /// Получение работы
        /// </summary>
        /// <param name="name">название работы</param>
        /// <param name="head">Руководитель работы</param>
        /// <param name="journal">Журнал</param>
        /// <param name="conference">Конференция</param>
        /// <param name="mark">Оценка</param>
        /// <param name="size">Размер</param>
        /// <returns></returns>
        private Work GetWork(string name, Author head, Journal journal, Conference conference, int mark, double size)
        {
            List<Work> works = (List<Work>)NirsSystem.GetListObject<Work>();
            if (works != null)
                foreach (var elem in works)
                    if (elem.WorkName == name)
                        return elem;

            Work work = new Work { WorkName = name, WorkMark = mark, WorkSize = size };
            if (journal != null)
                work.JournalId = journal.JournalId;
            if (conference != null)
                work.ConferenceId = conference.ConferenceId;
            if (head != null)
                work.HeadAuthorId = head.AuthorId;

            NirsSystem.AddObject(work);
            works = (List<Work>)NirsSystem.GetListObject<Work>();
            return works.First(u => u.WorkName == name);
        }

        /// <summary>
        /// Получение журнала
        /// </summary>
        /// <param name="name">Название журнала</param>
        /// <param name="date">Дата публикации</param>
        /// <returns></returns>
        private Journal GetJournal(string name, DateTime date)
        {
            if (date == DateTime.MinValue)
                date = DateTime.Now;

            List<Journal> journals = (List<Journal>)NirsSystem.GetListObject<Journal>();
            if (journals != null)
                foreach (var elem in journals)
                    if (elem.JournalName == name)
                        return elem;
            NirsSystem.AddObject(new Journal { JournalName = name, JournalDate = date });
            journals = (List<Journal>)NirsSystem.GetListObject<Journal>();
            return journals.First(u => u.JournalName == name);
        }

        /// <summary>
        /// Получение конференции
        /// </summary>
        /// <param name="name">Название конференции</param>
        /// <param name="date">Дата проведения конференции</param>
        /// <returns></returns>
        private Conference GetConference(string name, DateTime date)
        {
            if (date == DateTime.MinValue)
                date = DateTime.Now;

            List<Conference> conferences = (List<Conference>)NirsSystem.GetListObject<Conference>();
            if (conferences != null)
                foreach (var elem in conferences)
                    if (elem.ConferenceName == name)
                        return elem;
            NirsSystem.AddObject(new Conference { ConferenceName = name, ConferenceDate = date });
            conferences = (List<Conference>)NirsSystem.GetListObject<Conference>();
            return conferences.First(u => u.ConferenceName == name);
        }

        /// <summary>
        /// Асинхронная операция импорта данных из файла
        /// </summary>
        /// <param name="filename">Название файла</param>
        private async void ImportXlsx(string filename) => await Task.Run(() =>
        {
            IsDone = Visibility.Visible;
            OnPropertyChanged("IsDone");
            StatusString = "Выполняется импорт данных";
            OnPropertyChanged("StatusString");

            //Открываем документ
            FileInfo file = new FileInfo(filename);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCounter = 2;

                //Считываем построчно
                while (worksheet.Cells[rowCounter, 1].Value != null)
                {
                    //Получаем название работы
                    string workName = worksheet.Cells[rowCounter, 2]?.Value.ToString();
                    if (IsWorkDatabase(workName))
                    {
                        rowCounter++;
                        continue;
                    }

                    //Получаем список авторов работ
                    string[] authors = StringSplit(worksheet.Cells[rowCounter, 3]?.Value.ToString());

                    //Получаем кафедру и факультет
                    string faculty = worksheet.Cells[rowCounter, 4].Value?.ToString();
                    string department = worksheet.Cells[rowCounter, 5].Value?.ToString();

                    //Получаем руководителя
                    string headAuthor = worksheet.Cells[rowCounter, 6].Value?.ToString(); 

                    //Получаем разер в печатных листах
                    string tmpSize = worksheet.Cells[rowCounter, 7].Value?.ToString();
                    double workSize = 0.0d;
                    if (tmpSize != null)
                        workSize = Convert.ToDouble(tmpSize);

                    //Получаем оценку работы 
                    string tmpMark = worksheet.Cells[rowCounter, 8].Value?.ToString();
                    int workMark = 0;
                    if (tmpMark != null)
                        workMark = Convert.ToInt32(tmpMark);

                    //Получаем направления работы
                    string[] directions = StringSplit(worksheet.Cells[rowCounter, 9]?.Value.ToString());

                    //Получаем награды работы
                    string[] rewards = StringSplit(worksheet.Cells[rowCounter, 10]?.Value.ToString());

                    //Название журнала
                    string journalName = worksheet.Cells[rowCounter, 11].Value?.ToString();

                    //Название конференции
                    string conferenceName = worksheet.Cells[rowCounter, 12].Value?.ToString();

                    //Дата публикации
                    DateTime datePublic = DateTime.MinValue;
                    if(worksheet.Cells[rowCounter, 13].Value != null)
                        datePublic = (DateTime)worksheet.Cells[rowCounter, 13].Value;

                    //А потом заняться добавлением
                    Faculty facultyToAuthors = GetFaculty(faculty);
                    Department departmentToAuthors = GetDepartment(department);

                    //Добавление авторов, руководителя
                    List<Author> authorsToWork = new List<Author>();
                    Author authorHeaderToWork = null;

                    if (authors.Length > 0)
                        foreach (var elem in authors)
                            authorsToWork.Add(GetAuthor(elem, facultyToAuthors, departmentToAuthors));

                    if (headAuthor != null)
                        authorHeaderToWork = GetAuthor(headAuthor, facultyToAuthors, departmentToAuthors);

                    //Добавление направлений и наград
                    List<Direction> directionsToWork = new List<Direction>();
                    List<Reward> rewardsToWork = new List<Reward>();

                    if (directions.Length > 0)
                        foreach (var elem in directions)
                            directionsToWork.Add(GetDirection(elem));

                    if (rewards.Length > 0)
                        foreach (var elem in rewards)
                            rewardsToWork.Add(GetReward(elem));

                    //Добавление журнала / конференции
                    Conference conferenceToWork = null;
                    Journal journalToWork = null;

                    if (conferenceName != null)
                        conferenceToWork = GetConference(conferenceName, datePublic);
                    else if (journalName != null)
                        journalToWork = GetJournal(journalName, datePublic);

                    //Добавление работы
                    Work work = GetWork(workName, authorHeaderToWork, journalToWork, conferenceToWork, workMark, workSize);

                    //Добавление в таблицы соавторов, наград работы и направлений работы
                    AddToCoauthors(work, authorsToWork);
                    AddToDirectionWork(work, directionsToWork);
                    AddToRewardWork(work, rewardsToWork);

                    rowCounter++;
                }
            }
            Thread.Sleep(1000);
            IsDone = Visibility.Hidden;
            OnPropertyChanged("IsDone");
        });
    }
}
