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
    public sealed class WorksViewModel : ViewModel
    {
        //Полный список работ
        private List<Work> _worksFull;

        //Строка поиска. Остается на все время сессии
        private static string _search = string.Empty;

        /// <summary>
        /// Строка поиска
        /// </summary>
        public string Search
        {
            get => _search;
            set => _search = value;
        }

        //Список работ
        private List<WorkHelper> _works;

        /// <summary>
        /// Список работ
        /// </summary>
        public ObservableCollection<WorkHelper> Works
        {
            get
            {
                ObservableCollection<WorkHelper> works = new ObservableCollection<WorkHelper>();
                if (_works != null)
                    foreach (var elem in _works)
                        works.Add(elem);
                return works;
            }
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public WorksViewModel() : base("Работы")
        {
            GetWorks();
            NirsSystem.ChangeDatabase += NirsSystem_ChangeDatabase;
        }

        //Обработка события изменения базы данных
        private void NirsSystem_ChangeDatabase() => GetWorks();

        /// <summary>
        /// Поиск по названию
        /// </summary>
        public bool IsName { get; set; } = true;

        /// <summary>
        /// Поиск по руководителю
        /// </summary>
        public bool IsHeadAuthor { get; set; }

        /// <summary>
        /// Поиск по автору
        /// </summary>
        public bool IsAuthor { get; set; }

        /// <summary>
        /// Поиск по направлению
        /// </summary>
        public bool IsDirection { get; set; }

        private bool _isAccuracy = false;

        /// <summary>
        /// Искать ли с точным совпадением?
        /// </summary>
        public bool IsAccuracy
        {
            get => _isAccuracy;
            set
            {
                _isAccuracy = value;
                GetWorks();
                OnPropertyChanged("IsAccuracy");
            }
        }

        /// <summary>
        /// Команда - Добавить работу
        /// </summary>
        public RelayCommand CommandAdd
        {
            get => new RelayCommand(obj =>
            {
                ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
                window.mainGrid.Children.Clear();
                window.mainGrid.Children.Add(new WorkView(0));
            });
        }

        /// <summary>
        /// Команда поиска
        /// </summary>
        public RelayCommand CommandSearch
        {
            get => new RelayCommand(obj => GetWorks());
        }

        /// <summary>
        /// Получение списка работ с учетом правил поиска
        /// </summary>
        private async void GetWorks() => await Task.Run(() =>
        {
            _works = new List<WorkHelper>();
            _worksFull = (List<Work>)NirsSystem.GetListObject<Work>();
            if (_worksFull == null || _worksFull.Count < 1)
                return;
            if (_search == string.Empty)
            {
                foreach (var elem in _worksFull)
                    _works.Add(new WorkHelper(elem));
                OnPropertyChanged("Works");
                return;
            }
            foreach (var elem in _worksFull)
            {
                if (IsName)
                {
                    if (IsAccuracy)
                    {
                        if (elem.WorkName == _search)
                            _works.Add(new WorkHelper(elem));
                    }
                    else
                    {
                        if (elem.WorkName.Contains(_search))
                            _works.Add(new WorkHelper(elem));
                    }
                }

                if (IsHeadAuthor)
                {
                    if (elem.HeadAuthorId != null)
                    {
                        Author author = (Author)NirsSystem.GetObject<Author>((int)elem.HeadAuthorId);
                        if (IsAccuracy)
                        {
                            if (author.AuthorName == _search)
                                _works.Add(new WorkHelper(elem));
                        }
                        else
                        {
                            if (author.AuthorName.Contains(_search))
                                _works.Add(new WorkHelper(elem));
                        }
                    }
                }

                if(IsAuthor)
                {
                    List<CoAuthor> coAuthors = (List<CoAuthor>)NirsSystem.GetListObject<CoAuthor>();
                    if(coAuthors != null)
                    {
                        foreach(var query in coAuthors)
                            if(query.WorkId == elem.WorkId)
                            {
                                Author author = (Author)NirsSystem.GetObject<Author>(query.AuthorId);
                                if (IsAccuracy)
                                {
                                    if (author.AuthorName == _search)
                                    {
                                        _works.Add(new WorkHelper(elem));
                                        break;
                                    }
                                }
                                else
                                {
                                    if (author.AuthorName.Contains(_search))
                                    {
                                        _works.Add(new WorkHelper(elem));
                                        break;
                                    } 
                                }
                            }
                    }
                }

                if(IsDirection)
                {
                    List<DirectionWork> directionWorks = (List<DirectionWork>)NirsSystem.GetListObject<DirectionWork>();
                    if(directionWorks != null)
                    {
                        foreach (var query in directionWorks)
                            if (query.WorkId == elem.WorkId)
                            {
                                Direction direction = (Direction)NirsSystem.GetObject<Direction>(query.DirectionId);
                                if (IsAccuracy)
                                {
                                    if (direction.DirectionName == _search)
                                    {
                                        _works.Add(new WorkHelper(elem));
                                        break;
                                    }
                                }
                                else
                                {
                                    if (direction.DirectionName.Contains(_search))
                                    {
                                        _works.Add(new WorkHelper(elem));
                                        break;
                                    }
                                }
                            }
                    }
                }
            }
            OnPropertyChanged("Works");
            return;
        });

        /// <summary>
        /// Команда - Перейти к отчетам
        /// </summary>
        public RelayCommand CommandReport
        {
            get => new RelayCommand(obj =>
            {
                int n = 0;
                if (IsName)
                    n = 1;
                if (IsHeadAuthor)
                    n = 2;
                if (IsAuthor)
                    n = 3;
                if (IsDirection)
                    n = 4;
                ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
                window.mainGrid.Children.Clear();
                window.mainGrid.Children.Add(new ReportWorkView(_works, n, _search));
            });
        }
    }
}