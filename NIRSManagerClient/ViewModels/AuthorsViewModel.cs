using NIRSCore;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using NIRSCore.DataBaseModels;
using NIRSManagerClient.Views;
using System.Collections.Generic;
using NIRSManagerClient.HelpfulModels;

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

        /// <summary>
        /// Список авторов
        /// </summary>
        public List<AuthorHelper> Authors { get; private set; }

        /// <summary>
        /// Получение списка авторов с учетом правил поиска
        /// </summary>
        private async void GetAuthors() => await Task.Run(() =>
        {
            Authors = new List<AuthorHelper>();
            _authorsFull = (List<Author>)NirsSystem.GetListObject<Author>();
            if (_authorsFull == null || _authorsFull.Count < 1)
                return;
            if (_search == string.Empty)
            {
                foreach (var elem in _authorsFull)
                    Authors.Add(new AuthorHelper(elem));
                OnPropertyChanged("Authors");
                return;
            }
            if (_search[0] == '#')
            {

            }
            else
            {
                foreach (var elem in _authorsFull)
                    if (elem.AuthorName.Contains(_search))
                        Authors.Add(new AuthorHelper(elem));
            }
            OnPropertyChanged("Authors");
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
    }
}
