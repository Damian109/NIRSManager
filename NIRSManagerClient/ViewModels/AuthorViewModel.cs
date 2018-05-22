using System;
using NIRSCore;
using System.IO;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using NIRSCore.DataBaseModels;
using NIRSCore.StackOperations;
using System.Collections.Generic;

namespace NIRSManagerClient.ViewModels
{
    public sealed class AuthorViewModel : ViewModel
    {
        //Текущее представление автора
        private Author _author;

        //Предыдущее представление автора
        private readonly Author _prevAuthor;

        //Является ли автор новым
        private readonly bool _isNew;

        //Заполнение всех списков значениями
        private async void GetTables() => await Task.Run(() =>
        {
            Organizations = (List<Organization>)NirsSystem.GetListObject<Organization>();
            if (Organizations == null)
                Organizations = new List<Organization>();
            Organizations.Insert(0, new Organization { OrganizationId = 0, OrganizationName = "(пусто)" });
            SelectedOrganization = Organizations.FirstOrDefault(u => u.OrganizationId == _author.OrganizationId);
            OnPropertyChanged("Organizations");
            OnPropertyChanged("SelectedOrganization");

            Faculties = (List<Faculty>)NirsSystem.GetListObject<Faculty>();
            if (Faculties == null)
                Faculties = new List<Faculty>();
            Faculties.Insert(0, new Faculty { FacultyId = 0, FacultyName = "(пусто)" });
            SelectedFaculty = Faculties.FirstOrDefault(u => u.FacultyId == _author.FacultyId);
            OnPropertyChanged("Faculties");
            OnPropertyChanged("SelectedFaculty");

            Departments = (List<Department>)NirsSystem.GetListObject<Department>();
            if (Departments == null)
                Departments = new List<Department>();
            Departments.Insert(0, new Department { DepartmentId = 0, DepartmentName = "(пусто)" });
            SelectedDepartment = Departments.FirstOrDefault(u => u.DepartmentId == _author.DepartmentId);
            OnPropertyChanged("Departments");
            OnPropertyChanged("SelectedDepartment");

            Groups = (List<Group>)NirsSystem.GetListObject<Group>();
            if (Groups == null)
                Groups = new List<Group>();
            Groups.Insert(0, new Group { GroupId = 0, GroupName = "(пусто)" });
            SelectedGroup = Groups.FirstOrDefault(u => u.GroupId == _author.GroupId);
            OnPropertyChanged("Groups");
            OnPropertyChanged("SelectedGroup");

            Positions = (List<Position>)NirsSystem.GetListObject<Position>();
            if (Positions == null)
                Positions = new List<Position>();
            Positions.Insert(0, new Position { PositionId = 0, PositionName = "(пусто)" });
            SelectedPosition = Positions.FirstOrDefault(u => u.PositionId == _author.PositionId);
            OnPropertyChanged("Positions");
            OnPropertyChanged("SelectedPosition");

            AcademicDegrees = (List<AcademicDegree>)NirsSystem.GetListObject<AcademicDegree>();
            if (AcademicDegrees == null)
                AcademicDegrees = new List<AcademicDegree>();
            AcademicDegrees.Insert(0, new AcademicDegree { AcademicDegreeId = 0, AcademicDegreeName = "(пусто)" });
            SelectedAcademicDegree = AcademicDegrees.FirstOrDefault(u => u.AcademicDegreeId == _author.AcademicDegreeId);
            OnPropertyChanged("AcademicDegrees");
            OnPropertyChanged("SelectedAcademicDegree");
        });

        //Загрузка страницы с авторами
        private void BackWindow()
        {
            ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new Views.AuthorsView());
        }

        /// <summary>
        /// ФИО автора
        /// </summary>
        public string AuthorName
        {
            get => _author.AuthorName;
            set
            {
                _author.AuthorName = value;
                OnPropertyChanged("AuthorName");
            }
        }

        /// <summary>
        /// Путь к фото
        /// </summary>
        public string PhotoPath
        {
            get => Environment.CurrentDirectory + _author.PhotoPath;
            set
            {
                _author.PhotoPath = value;
                OnPropertyChanged("PhotoPath");
                //
                //
                //
                //
                ///
                /* _author.PathPhoto = "\\data\\" + NirsSystem.GetLogin() + "\\photos\\" + _author.UserId + "author.png";
                File.Copy(value, Environment.CurrentDirectory + _author.PathPhoto);
                OnPropertyChanged("PathPhoto"); */
            }
        }

        /// <summary>
        /// Список доступных организаций
        /// </summary>
        public List<Organization> Organizations { get; private set; }

        /// <summary>
        /// Выбранная организация
        /// </summary>
        public Organization SelectedOrganization { get; set; }

        /// <summary>
        /// Список доступных факультетов
        /// </summary>
        public List<Faculty> Faculties { get; private set; }

        /// <summary>
        /// Выбранный факультет
        /// </summary>
        public Faculty SelectedFaculty { get; set; }

        /// <summary>
        /// Список доступных кафедр
        /// </summary>
        public List<Department> Departments { get; private set; }

        /// <summary>
        /// Выбранная кафедра
        /// </summary>
        public Department SelectedDepartment { get; set; }

        /// <summary>
        /// Список доступных групп
        /// </summary>
        public List<Group> Groups { get; private set; }

        /// <summary>
        /// Выбранная группа
        /// </summary>
        public Group SelectedGroup { get; set; }

        /// <summary>
        /// Список доступных должностей
        /// </summary>
        public List<Position> Positions { get; private set; }

        /// <summary>
        /// Выбранная должность
        /// </summary>
        public Position SelectedPosition { get; set; }

        /// <summary>
        /// Список доступных ученых степеней
        /// </summary>
        public List<AcademicDegree> AcademicDegrees { get; private set; }

        /// <summary>
        /// Выбранная ученая степень
        /// </summary>
        public AcademicDegree SelectedAcademicDegree { get; set; }

        /// <summary>
        /// Доступность некоторых кнопок
        /// </summary>
        public bool IsDeletable
        {
            get
            {
                if (_isNew)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Модель представления для автора
        /// </summary>
        /// <param name="id"> ID Автора или 0</param>
        public AuthorViewModel(int id) : base("Автор")
        {
            if(id == 0)
            {
                _author = new Author()
                {
                    AcademicDegreeId = null,
                    AuthorName = "",
                    DepartmentId = null,
                    FacultyId = null,
                    GroupId = null,
                    PhotoPath = "\\data\\author.PNG",
                    OrganizationId = null,
                    PositionId = null
                };
                _isNew = true;
            }
            else
            {
                _author = (Author)NirsSystem.GetObject<Author>(id);
                _prevAuthor = new Author
                {
                    AcademicDegreeId = _author.AcademicDegreeId,
                    AuthorName = _author.AuthorName,
                    DepartmentId = _author.DepartmentId,
                    FacultyId = _author.FacultyId,
                    GroupId = _author.GroupId,
                    PhotoPath = _author.PhotoPath,
                    OrganizationId = _author.OrganizationId,
                    PositionId = _author.PositionId,
                    AuthorId = _author.AuthorId
                };
                _isNew = false;
            }
            GetTables();
        }

        /// <summary>
        /// Команда назад
        /// </summary>
        public RelayCommand CommandBack
        {
            get => new RelayCommand(obj => BackWindow());
        }

        /// <summary>
        /// Команда сохранить
        /// </summary>
        public RelayCommand CommandSave
        {
            get => new RelayCommand(obj =>
            {
                if (SelectedOrganization == null || SelectedOrganization == Organizations[0])
                    _author.OrganizationId = null;
                else
                    _author.OrganizationId = SelectedOrganization.OrganizationId;

                if (SelectedFaculty == null || SelectedFaculty == Faculties[0])
                    _author.FacultyId = null;
                else
                    _author.FacultyId = SelectedFaculty.FacultyId;

                if (SelectedDepartment == null || SelectedDepartment == Departments[0])
                    _author.DepartmentId = null;
                else
                    _author.DepartmentId = SelectedDepartment.DepartmentId;

                if (SelectedGroup == null || SelectedGroup == Groups[0])
                    _author.GroupId = null;
                else
                    _author.GroupId = SelectedGroup.GroupId;

                if (SelectedPosition == null || SelectedPosition == Positions[0])
                    _author.PositionId = null;
                else
                    _author.PositionId = SelectedPosition.PositionId;

                if (SelectedAcademicDegree == null || SelectedAcademicDegree == AcademicDegrees[0])
                    _author.AcademicDegreeId = null;
                else
                    _author.AcademicDegreeId = SelectedAcademicDegree.AcademicDegreeId;

                if (_isNew)
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objDone => NirsSystem.AddObject(_author), null);

                    //Создание команды отмены операции
                    RelayCommand undone = new RelayCommand(objUnDone => NirsSystem.DeleteObject(_author), null);

                    //Создание операции
                    Operation operation = new Operation("Добавлен новый автор", done, undone);

                    NirsSystem.StackOperations.AddOperation(operation);
                    operation.DoneCommand.Execute(null);
                }
                else
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objDone => NirsSystem.UpdateObject(_author), null);

                    //Создание команды отмены операции
                    RelayCommand undone = new RelayCommand(objUnDone => NirsSystem.UpdateObject(_prevAuthor), null);

                    //Создание операции
                    Operation operation = new Operation("Автор был изменен", done, undone);

                    NirsSystem.StackOperations.AddOperation(operation);
                    operation.DoneCommand.Execute(null);
                }
                BackWindow();
            });
        }

        /// <summary>
        /// Команда Удалить
        /// </summary>
        public RelayCommand CommandDelete
        {
            get => new RelayCommand(obj =>
            {
                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => NirsSystem.DeleteObject(_author), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => NirsSystem.AddObject(_author), null);

                //Создание операции
                Operation operation = new Operation("Удален автор", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
                BackWindow();
            });
        }
    }
}