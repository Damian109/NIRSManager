using NIRSCore;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using NIRSCore.DataBaseModels;
using NIRSManagerClient.Views;
using System.Collections.Generic;
using NIRSManagerClient.HelpfulModels;
using System.Collections.ObjectModel;

namespace NIRSManagerClient.ViewModels
{
    public sealed class AuthorsViewModel : ViewModel
    {
        //Полный список авторов
        private List<Author> _authorsFull;

        //Строка поиска. Остается на все время сессии
        private static string _search = string.Empty;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public AuthorsViewModel() : base("Авторы") => GetAuthors();

        /// <summary>
        /// Строка поиска
        /// </summary>
        public string Search
        {
            get => _search;
            set => _search = value;
        }

        //Список авторов
        private List<AuthorHelper> _authors;

        /// <summary>
        /// Список авторов
        /// </summary>
        public ObservableCollection<AuthorHelper> Authors
        {
            get
            {
                ObservableCollection<AuthorHelper> authors = new ObservableCollection<AuthorHelper>();
                if (_authors != null)
                    foreach (var elem in _authors)
                        authors.Add(elem);
                return authors;
            }
        }

        /// <summary>
        /// Получение списка авторов с учетом правил поиска
        /// </summary>
        private async void GetAuthors() => await Task.Run(() =>
        {
            _authors = new List<AuthorHelper>();
            _authorsFull = (List<Author>)NirsSystem.GetListObject<Author>();
            if (_authorsFull == null || _authorsFull.Count < 1)
                return;
            if (_search == string.Empty)
            {
                foreach (var elem in _authorsFull)
                    _authors.Add(new AuthorHelper(elem));
                OnPropertyChanged("Authors");
                return;
            }
            foreach (var elem in _authorsFull)
            {
                if(IsFio)
                {
                    if(IsAccuracy)
                    {
                        if (elem.AuthorName == _search)
                            _authors.Add(new AuthorHelper(elem));
                    }
                    else
                    {
                        if (elem.AuthorName.Contains(_search))
                            _authors.Add(new AuthorHelper(elem));
                    }
                    
                }

                if (IsOrganization)
                {
                    if (elem.OrganizationId != null)
                    {
                        Organization organization = (Organization)NirsSystem.GetObject<Organization>((int)elem.OrganizationId);
                        if (IsAccuracy)
                        {
                            if (organization.OrganizationName == _search)
                                _authors.Add(new AuthorHelper(elem));
                        }
                        else
                        {
                            if (organization.OrganizationName.Contains(_search))
                                _authors.Add(new AuthorHelper(elem));
                        }
                    }
                }

                if (IsFaculty)
                {
                    if (elem.FacultyId != null)
                    {
                        Faculty faculty = (Faculty)NirsSystem.GetObject<Faculty>((int)elem.FacultyId);
                        if (IsAccuracy)
                        {
                            if (faculty.FacultyName == _search)
                                _authors.Add(new AuthorHelper(elem));
                        }
                        else
                        {
                            if (faculty.FacultyName.Contains(_search))
                                _authors.Add(new AuthorHelper(elem));
                        }
                    }
                }

                if (IsDepartment)
                {
                    if (elem.DepartmentId != null)
                    {
                        Department department = (Department)NirsSystem.GetObject<Department>((int)elem.DepartmentId);
                        if (IsAccuracy)
                        {
                            if (department.DepartmentName == _search)
                                _authors.Add(new AuthorHelper(elem));
                        }
                        else
                        {
                            if (department.DepartmentName.Contains(_search))
                                _authors.Add(new AuthorHelper(elem));
                        }
                    }
                }

                if (IsGroup)
                {
                    if (elem.GroupId != null)
                    {
                        Group group = (Group)NirsSystem.GetObject<Group>((int)elem.GroupId);
                        if (IsAccuracy)
                        {
                            if (group.GroupName == _search)
                                _authors.Add(new AuthorHelper(elem));
                        }
                        else
                        {
                            if (group.GroupName.Contains(_search))
                                _authors.Add(new AuthorHelper(elem));
                        }
                    }
                }
            }
            OnPropertyChanged("Authors");
            return;
        });

        /// <summary>
        /// Команда - Добавить автора
        /// </summary>
        public RelayCommand CommandAdd
        {
            get => new RelayCommand(obj =>
            {
                ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
                window.mainGrid.Children.Clear();
                window.mainGrid.Children.Add(new AuthorView(0));
            });
        }

        /// <summary>
        /// Команда поиска
        /// </summary>
        public RelayCommand CommandSearch
        {
            get => new RelayCommand(obj => GetAuthors());
        }

        /// <summary>
        /// Поиск по ФИО
        /// </summary>
        public bool IsFio { get; set; } = true;

        /// <summary>
        /// Поиск по организации
        /// </summary>
        public bool IsOrganization { get; set; }

        /// <summary>
        /// Поиск по факультету
        /// </summary>
        public bool IsFaculty { get; set; }

        /// <summary>
        /// Поиск по кафедре
        /// </summary>
        public bool IsDepartment { get; set; }

        /// <summary>
        /// Поиск по группе
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// Искать ли с точным совпадением?
        /// </summary>
        public bool IsAccuracy { get; set; } = false;
    }
}
