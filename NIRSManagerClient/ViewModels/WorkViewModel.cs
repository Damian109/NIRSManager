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
    public sealed class WorkViewModel : ViewModel
    {
        //Текущее представление работы
        private Work _work;

        //Является ли работа новой
        private readonly bool _isNew;

        //Загрузка страницы с работами
        private void BackWindow()
        {
            ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new Views.WorksView());
        }

        //Добавление автора
        private void AddAuthor()
        {
            if (SelectedNoAddedAuthor != null)
            {
                AddedAuthors.Add(SelectedNoAddedAuthor);
                NoAddedAuthors.Remove(SelectedNoAddedAuthor);
                OnPropertyChanged("AddedAuthors");
                OnPropertyChanged("NoAddedAuthors");
            }
        }

        //Удаление автора
        private void DeleteAuthor()
        {
            if (SelectedAddedAuthor != null)
            {
                NoAddedAuthors.Add(SelectedAddedAuthor);
                AddedAuthors.Remove(SelectedAddedAuthor);
                OnPropertyChanged("AddedAuthors");
                OnPropertyChanged("NoAddedAuthors");
            }
        }

        //Добавление направления
        private void AddDirection()
        {
            if (SelectedNoAddedDirection != null)
            {
                AddedDirections.Add(SelectedNoAddedDirection);
                NoAddedDirections.Remove(SelectedNoAddedDirection);
                OnPropertyChanged("AddedDirections");
                OnPropertyChanged("NoAddedDirections");
            }
        }

        //Удаление направления
        private void DeleteDirection()
        {
            if (SelectedAddedDirection != null)
            {
                NoAddedDirections.Add(SelectedAddedDirection);
                AddedDirections.Remove(SelectedAddedDirection);
                OnPropertyChanged("AddedDirections");
                OnPropertyChanged("NoAddedDirections");
            }
        }

        //Добавление награды
        private void AddReward()
        {
            if (SelectedNoAddedReward != null)
            {
                AddedRewards.Add(SelectedNoAddedReward);
                NoAddedRewards.Remove(SelectedNoAddedReward);
                OnPropertyChanged("AddedRewards");
                OnPropertyChanged("NoAddedRewards");
            }
        }

        //Удаление награды
        private void DeleteReward()
        {
            if (SelectedAddedReward != null)
            {
                NoAddedRewards.Add(SelectedAddedReward);
                AddedRewards.Remove(SelectedAddedReward);
                OnPropertyChanged("AddedRewards");
                OnPropertyChanged("NoAddedRewards");
            }
        }

        //Получение некоторых основных списков
        private async void GetTables() => await Task.Run(() =>
        {
            //Работа с руководителями
            HeaderAuthors = (List<Author>)NirsSystem.GetListObject<Author>();
            if (HeaderAuthors == null)
                HeaderAuthors = new List<Author>();
            HeaderAuthors.Insert(0, new Author { AuthorId = 0, AuthorName = "(пусто)" });
            SelectedHeaderAuthor = HeaderAuthors.FirstOrDefault(u => u.AuthorId == _work.HeadAuthorId);
            OnPropertyChanged("HeadAuthors");
            OnPropertyChanged("SelectedHeaderAuthor");

            //Работа с авторами
            List<Author> authors = (List<Author>)NirsSystem.GetListObject<Author>();
            AddedAuthors = new List<Author>();
            NoAddedAuthors = new List<Author>();
            if (authors != null)
            {
                List<CoAuthor> coAuthors = (List<CoAuthor>)NirsSystem.GetListObject<CoAuthor>();
                if (coAuthors != null)
                    foreach (var elem in coAuthors)
                        if (elem.WorkId == _work.WorkId)
                            AddedAuthors.Add(authors.FirstOrDefault(u => u.AuthorId == elem.AuthorId));
                        else
                            NoAddedAuthors.Add(authors.FirstOrDefault(u => u.AuthorId == elem.AuthorId));
            }
            OnPropertyChanged("AddedAuthors");
            OnPropertyChanged("NoAddedAuthors");

            //Работа с направлениями
            List<Direction> directions = (List<Direction>)NirsSystem.GetListObject<Direction>();
            AddedDirections = new List<Direction>();
            NoAddedDirections = new List<Direction>();
            if (directions != null)
            {
                List<DirectionWork> directionWorks = (List<DirectionWork>)NirsSystem.GetListObject<DirectionWork>();
                if (directionWorks != null)
                    foreach (var elem in directionWorks)
                        if (elem.WorkId == _work.WorkId)
                            AddedDirections.Add(directions.FirstOrDefault(u => u.DirectionId == elem.DirectionId));
                        else
                            NoAddedDirections.Add(directions.FirstOrDefault(u => u.DirectionId == elem.DirectionId));
            }
            OnPropertyChanged("AddedDirections");
            OnPropertyChanged("NoAddedDirections");

            //Работа с наградами
            List<Reward> rewards = (List<Reward>)NirsSystem.GetListObject<Reward>();
            AddedRewards = new List<Reward>();
            NoAddedRewards = new List<Reward>();
            if (rewards != null)
            {
                List<RewardWork> rewardWorks = (List<RewardWork>)NirsSystem.GetListObject<RewardWork>();
                if (rewardWorks != null)
                    foreach (var elem in rewardWorks)
                        if (elem.WorkId == _work.WorkId)
                            AddedRewards.Add(rewards.FirstOrDefault(u => u.RewardId == elem.RewardId));
                        else
                            NoAddedRewards.Add(rewards.FirstOrDefault(u => u.RewardId == elem.RewardId));
            }
            OnPropertyChanged("AddedRewards");
            OnPropertyChanged("NoAddedRewards");
        });

        //Получение списка журналов
        private async void GetJournals() => await Task.Run(() =>
        {
            Journals = (List<Journal>)NirsSystem.GetListObject<Journal>();
            if (Journals == null)
                Journals = new List<Journal>();
            Journals.Insert(0, new Journal { JournalId = 0, JournalName = "(пусто)" });
            SelectedJournal = Journals.FirstOrDefault(u => u.JournalId == _work.JournalId);
            OnPropertyChanged("Journals");
            OnPropertyChanged("SelectedJournal");
        });

        //Получение списка конференций
        private async void GetConferences() => await Task.Run(() =>
        {
            Conferences = (List<Conference>)NirsSystem.GetListObject<Conference>();
            if (Conferences == null)
                Conferences = new List<Conference>();
            Conferences.Insert(0, new Conference { ConferenceId = 0, ConferenceName = "(пусто)" });
            SelectedConference = Conferences.FirstOrDefault(u => u.ConferenceId == _work.ConferenceId);
            OnPropertyChanged("Conferences");
            OnPropertyChanged("SelectedConference");
        });

        //Добавление журнала
        private async void AddJournal(Journal journal) => await Task.Run(() =>
        {
            NirsSystem.AddObject(journal);
            GetJournals();
        });

        //Удаление журнала
        private async void DeleteJournal(Journal journal) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(journal);
            GetJournals();
        });

        //Добавление конференции
        private async void AddConference(Conference conference) => await Task.Run(() =>
        {
            NirsSystem.AddObject(conference);
            GetConferences();
        });

        //Удаление конференции
        private async void DeleteConference(Conference conference) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(conference);
            GetConferences();
        });

        //Очистка всех таблиц, связанных с работой
        private async void DeleteAllFromTables(int id) => await Task.Run(() =>
        {
            List<CoAuthor> coAuthors = (List<CoAuthor>)NirsSystem.GetListObject<CoAuthor>();
            foreach (var elem in coAuthors)
                if (elem.WorkId == id)
                    NirsSystem.DeleteObject(elem);

            List<DirectionWork> directionWorks = (List<DirectionWork>)NirsSystem.GetListObject<DirectionWork>();
            foreach (var elem in directionWorks)
                if (elem.WorkId == id)
                    NirsSystem.DeleteObject(elem);

            List<RewardWork> rewardWorks = (List<RewardWork>)NirsSystem.GetListObject<RewardWork>();
            foreach (var elem in rewardWorks)
                if (elem.WorkId == id)
                    NirsSystem.DeleteObject(elem);
        });

        //Добавление элементов в связанные таблицы
        private async void AddAllToTables(int id) => await Task.Run(() =>
        {
            DeleteAllFromTables(id);
            foreach (var elem in AddedAuthors)
                NirsSystem.AddObject(new CoAuthor { AuthorId = elem.AuthorId, Contribution = 100, WorkId = id });
            foreach (var elem in AddedDirections)
                NirsSystem.AddObject(new DirectionWork { DirectionId = elem.DirectionId, WorkId = id });
            foreach (var elem in AddedRewards)
                NirsSystem.AddObject(new RewardWork { RewardId = elem.RewardId, WorkId = id });
        });

        /// <summary>
        /// Название работы
        /// </summary>
        public string WorkName
        {
            get => _work.WorkName;
            set
            {
                _work.WorkName = value;
                OnPropertyChanged("WorkName");
            }
        }

        /// <summary>
        /// Размер работы
        /// </summary>
        public string WorkSize
        {
            get => _work.WorkSize.ToString();
            set
            {
                _work.WorkSize = Convert.ToDouble(value);
                OnPropertyChanged("WorkSize");
            }
        }

        /// <summary>
        /// Оценка работы
        /// </summary>
        public string WorkMark
        {
            get => _work.WorkMark.ToString();
            set
            {
                _work.WorkMark = Convert.ToInt32(value);
                OnPropertyChanged("WorkMark");
            }
        }

        /// <summary>
        /// Список руководителей
        /// </summary>
        public List<Author> HeaderAuthors { get; private set; }

        /// <summary>
        /// Выбранный руководитель
        /// </summary>
        public Author SelectedHeaderAuthor { get; set; }

        /// <summary>
        /// Список журналов
        /// </summary>
        public List<Journal> Journals { get; private set; }

        /// <summary>
        /// Выбранный журнал
        /// </summary>
        public Journal SelectedJournal { get; set; }

        /// <summary>
        /// Список конференций
        /// </summary>
        public List<Conference> Conferences { get; private set; }

        /// <summary>
        /// Выбранная конференция
        /// </summary>
        public Conference SelectedConference { get; set; }

        /// <summary>
        /// Список добавленных авторов
        /// </summary>
        public List<Author> AddedAuthors { get; set; }

        /// <summary>
        /// Выбранный добавленный автор
        /// </summary>
        public Author SelectedAddedAuthor { get; set; }

        /// <summary>
        /// Список авторов для добавления
        /// </summary>
        public List<Author> NoAddedAuthors { get; set; }

        /// <summary>
        /// Выбранный автор для добавления
        /// </summary>
        public Author SelectedNoAddedAuthor { get; set; }

        /// <summary>
        /// Список добавленных направлений
        /// </summary>
        public List<Direction> AddedDirections { get; set; }

        /// <summary>
        /// Выбранное добавленное направление
        /// </summary>
        public Direction SelectedAddedDirection { get; set; }

        /// <summary>
        /// Список направлений для добавления
        /// </summary>
        public List<Direction> NoAddedDirections { get; set; }

        /// <summary>
        /// Выбранное направление для добавления
        /// </summary>
        public Direction SelectedNoAddedDirection { get; set; }

        /// <summary>
        /// Список добавленных наград
        /// </summary>
        public List<Reward> AddedRewards { get; set; }

        /// <summary>
        /// Выбранная добавленная награда
        /// </summary>
        public Reward SelectedAddedReward { get; set; }

        /// <summary>
        /// Список наград для добавления
        /// </summary>
        public List<Reward> NoAddedRewards { get; set; }

        /// <summary>
        /// Выбранная награда для добавления
        /// </summary>
        public Reward SelectedNoAddedReward { get; set; }

        /// <summary>
        /// Название журнала
        /// </summary>
        public string NameJournal { get; set; }

        /// <summary>
        /// Дата публикации журнала
        /// </summary>
        public DateTime DateJournal { get; set; }

        /// <summary>
        /// Название конференции
        /// </summary>
        public string NameConference { get; set; }

        /// <summary>
        /// Дата проведения конференции
        /// </summary>
        public DateTime DateConference { get; set; }

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
        /// <param name="id"> ID Работы или 0</param>
        public WorkViewModel(int id) : base("Работа")
        {
            if (id == 0)
            {
                _work = new Work()
                {
                    ConferenceId = null,
                    HeadAuthorId = null,
                    JournalId = null,
                    WorkMark = 0,
                    WorkName = "",
                    WorkPath = "",
                    WorkSize = 0.0d
                };
                _isNew = true;
            }
            else
            {
                _work = (Work)NirsSystem.GetObject<Work>(id);
                _isNew = false;
            }
            GetTables();
            GetConferences();
            GetJournals();
        }

        /// <summary>
        /// Команда Добавить автора
        /// </summary>
        public RelayCommand CommandAddAuthor
        {
            get => new RelayCommand(obj => AddAuthor());
        }

        /// <summary>
        /// Команда Удалить автора
        /// </summary>
        public RelayCommand CommandDeleteAuthor
        {
            get => new RelayCommand(obj => DeleteAuthor());
        }

        /// <summary>
        /// Команда Добавить направление
        /// </summary>
        public RelayCommand CommandAddDirection
        {
            get => new RelayCommand(obj => AddDirection());
        }

        /// <summary>
        /// Команда Удалить направление
        /// </summary>
        public RelayCommand CommandDeleteDirection
        {
            get => new RelayCommand(obj => DeleteDirection());
        }

        /// <summary>
        /// Команда Добавить награду
        /// </summary>
        public RelayCommand CommandAddReward
        {
            get => new RelayCommand(obj => AddReward());
        }

        /// <summary>
        /// Команда Удалить награду
        /// </summary>
        public RelayCommand CommandDeleteReward
        {
            get => new RelayCommand(obj => DeleteReward());
        }

        /// <summary>
        /// Команда Загрузить новую работу
        /// </summary>
        public RelayCommand CommandLoadWork
        {
            get => new RelayCommand(obj =>
            {
                //Начальная инициализация диалогового окна
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
                {
                    FileName = "",
                    DefaultExt = ".docx",
                    Filter = "Документ (*.docx, *.pdf)|*.docx;*.pdf"
                };

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    string ext = ".pdf";
                    if (dialog.FileName.Contains(".docx"))
                        ext = ".docx";

                    string newPath = Environment.CurrentDirectory + "\\data\\" + NirsSystem.GetLogin() + "\\Documents\\" + _work.WorkId + ext;
                    if (File.Exists(newPath))
                        File.Delete(newPath);
                    File.Copy(dialog.FileName, newPath);
                    _work.WorkPath = "\\data\\" + NirsSystem.GetLogin() + "\\Documents\\" + _work.WorkId + ext;
                }
            });
        }

        /// <summary>
        /// Команда Добавление журнала
        /// </summary>
        public RelayCommand CommandAddJournal
        {
            get => new RelayCommand(obj =>
            {
                Journal journal = new Journal { JournalName = NameJournal, JournalDate = DateJournal };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddJournal(journal), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteJournal(journal), null);

                //Создание операции
                Operation operation = new Operation("Добавлен новый журнал", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        /// <summary>
        /// Команда Добавление конференции
        /// </summary>
        public RelayCommand CommandAddConference
        {
            get => new RelayCommand(obj =>
            {
                Conference conference = new Conference { ConferenceName = NameConference, ConferenceDate = DateConference };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddConference(conference), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteConference(conference), null);

                //Создание операции
                Operation operation = new Operation("Добавлена новая конференция", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
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
                if (SelectedHeaderAuthor == null || SelectedHeaderAuthor == HeaderAuthors[0])
                    _work.HeadAuthorId = null;
                else
                    _work.HeadAuthorId = SelectedHeaderAuthor.AuthorId;

                if (SelectedJournal == null || SelectedJournal == Journals[0])
                    _work.JournalId = null;
                else
                    _work.JournalId = SelectedJournal.JournalId;

                if (SelectedConference == null || SelectedConference == Conferences[0])
                    _work.ConferenceId = null;
                else
                    _work.ConferenceId = SelectedConference.ConferenceId;

                if (_isNew)
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objDone =>
                    {
                        AddAllToTables(_work.WorkId);
                        NirsSystem.AddObject(_work);
                    }, null);

                    //Создание команды отмены операции
                    RelayCommand undone = new RelayCommand(objUnDone =>
                    {
                        DeleteAllFromTables(_work.WorkId);
                        NirsSystem.DeleteObject(_work);
                    }, null);

                    //Создание операции
                    Operation operation = new Operation("Добавлена новая работа", done, undone);

                    NirsSystem.StackOperations.AddOperation(operation);
                    operation.DoneCommand.Execute(null);
                }
                else
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objDone =>
                    {
                        AddAllToTables(_work.WorkId);
                        NirsSystem.UpdateObject(_work);
                    }, null);

                    //Создание операции
                    Operation operation = new Operation("Работа была изменена", done, null);

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
                RelayCommand done = new RelayCommand(objDone => NirsSystem.DeleteObject(_work), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone =>
                {
                    NirsSystem.AddObject(_work);
                    List<Work> works = (List<Work>)NirsSystem.GetListObject<Work>();
                    AddAllToTables(works.Last().WorkId);
                }, null);

                //Создание операции
                Operation operation = new Operation("Удалена работа", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
                BackWindow();
            });
        }
    }
}
