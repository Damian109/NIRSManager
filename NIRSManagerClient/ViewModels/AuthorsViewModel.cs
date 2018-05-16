using NIRSCore;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using NIRSManagerClient.Views;
using System.Collections.Generic;
using NIRSManagerClient.HelpfulModels;


namespace NIRSManagerClient.ViewModels
{
    public sealed class AuthorsViewModel : ViewModel
    {
        //Первоначальный список авторов
        private List<AuthorHelper> _authors;
        private static string _search = string.Empty;

        /// <summary>
        /// Строка поиска
        /// </summary>
        public string Search
        {
            get => _search;
            set => _search = value;
        }

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
            get => new RelayCommand(obj => FillAuthors());
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public AuthorsViewModel() : base("Авторы")
        {
            _authors = new List<AuthorHelper>();
            FillAuthors();
        }

        /// <summary>
        /// Список авторов
        /// </summary>
        public List<AuthorHelper> Authors { get => _authors; }

        /// <summary>
        /// Заполнение списка авторов
        /// </summary>
        private void FillAuthors()
        {
            GetAuthors();
            if (_authors == null)
                _authors = new List<AuthorHelper>();
            OnPropertyChanged("Authors");
        }

        /// <summary>
        /// Получение списка авторов с учетом правил поиска
        /// </summary>
        private async void GetAuthors()
        {
            await Task.Run(() =>
            {
                List<AuthorHelper> queryResult = new List<AuthorHelper>();
                List<AuthorHelper> result = new List<AuthorHelper>();
                var query = NirsSystem.GetAuthors();
                if (query != null)
                    foreach (var elem in query)
                        queryResult.Add(new AuthorHelper(elem.UserId));
                foreach (var elem in queryResult)
                {
                    if (elem.Fio != null)
                    {
                        if (elem.Fio.Contains(_search))
                        {
                            result.Add(elem);
                            continue;
                        }
                    }
                    if (elem.Position != null)
                    {
                        if (elem.Position.Contains(_search))
                        {
                            result.Add(elem);
                            continue;
                        }
                    }
                    if (elem.Organization != null)
                    {
                        if (elem.Organization.Contains(_search))
                        {
                            result.Add(elem);
                            continue;
                        }
                    }
                }
                _authors = result;
                OnPropertyChanged("Authors");
            });
        }
    }
}
