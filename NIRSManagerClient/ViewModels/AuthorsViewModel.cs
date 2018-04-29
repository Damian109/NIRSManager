using NIRSCore;
using System.Collections.ObjectModel;
using NIRSManagerClient.HelpfulModels;
using System.Windows;

namespace NIRSManagerClient.ViewModels
{
    public sealed class AuthorsViewModel : ViewModel
    {
        //Первоначальный список авторов
        private ObservableCollection<AuthorHelper> _authors;

        private static string _search = string.Empty;

        /// <summary>
        /// Получение списка авторов с учетом правил поиска
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<AuthorHelper> GetAuthorsLocal()
        {
            ObservableCollection<AuthorHelper> result = new ObservableCollection<AuthorHelper>();
            foreach(var elem in _authors)
                if (elem.Fio.Contains(_search) || elem.Organization.Contains(_search) || elem.Position.Contains(_search))
                    result.Add(elem);
            return result;
        }

        private ObservableCollection<AuthorHelper> GetAuthors()
        {
            ObservableCollection<AuthorHelper> result = new ObservableCollection<AuthorHelper>();
            var query = NirsSystem.GetAuthors();
            if(query != null)
                foreach (var elem in query)
                    result.Add(new AuthorHelper(elem.UserId));
            return result;
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public AuthorsViewModel() : base("Авторы")
        {
            _authors = GetAuthors();
        }

        /// <summary>
        /// Строка поиска
        /// </summary>
        public string Search 
        {
            get => _search;
            set
            {
                _search = value;
                OnPropertyChanged("Authors");
            }
        }

        /// <summary>
        /// Список авторов
        /// </summary>
        public ObservableCollection<AuthorHelper> Authors {  get => GetAuthorsLocal(); }

        public RelayCommand CommandSearch
        {
            get => new RelayCommand(obj => OnPropertyChanged("Authors"));
        }
    }
}
