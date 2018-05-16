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
        private Author _author;
        private Author _prevAuthor;
        private bool _isNew;

        //Доступность кнопки удаления
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
                    AcademicDegreeId = 0,
                    DateOfBirth = DateTime.Now,
                    Name = "",
                    SurName = "",
                    SecondName = "",
                    OrganizationId = 0,
                    PathPhoto = "\\data\\" + NirsSystem.GetLogin() + "\\photos\\0author.png",
                    PositionId = 0
                };
                _isNew = true;
            }
            else
            {
                _author = NirsSystem.GetAuthor(id);
                _prevAuthor = new Author
                {
                    AcademicDegreeId = _author.AcademicDegreeId,
                    DateOfBirth = _author.DateOfBirth,
                    Name = _author.Name,
                    OrganizationId = _author.OrganizationId,
                    PathPhoto = _author.PathPhoto,
                    PositionId = _author.PositionId,
                    SecondName = _author.SecondName,
                    SurName = _author.SurName,
                    UserId = _author.UserId
                };
                _isNew = false;
            }
            _academicDegrees = new List<AcademicDegree>();
            _selectedAcademicDegree = null;
            AcademicDegreeName = string.Empty;
            _organizations = new List<Organization>();
            _selectedOrganization = null;
            OrganizationName = string.Empty;
            _positions = new List<Position>();
            _selectedPosition = null;
            PositionName = string.Empty;

            GetAcademicDegrees();
            GetOrganizations();
            GetPositions();
        }

        //Свойства
        //Определяются по данным автора

        public string SurName
        {
            get => _author.SurName;
            set
            {
                _author.SurName = value;
                OnPropertyChanged("Fio");
            }
        }

        public string AName
        {
            get => _author.Name;
            set
            {
                _author.Name = value;
                OnPropertyChanged("Fio");
            }
        }

        public string SecondName
        {
            get => _author.SecondName;
            set
            {
                _author.SecondName = value;
                OnPropertyChanged("Fio");
            }
        }

        public DateTime DateOfBirth
        {
            get => _author.DateOfBirth;
            set => _author.DateOfBirth = value;
        }

        /// <summary>
        /// ФИО Автора
        /// </summary>
        public string Fio
        {
            get => SurName + " " + AName + " " + SecondName;
        }

        /// <summary>
        /// Путь к фото
        /// </summary>
        public string PathPhoto
        {
            get => _author.PathPhoto;
            set
            {
                _author.PathPhoto = "\\data\\" + NirsSystem.GetLogin() + "\\photos\\" + _author.UserId + "author.png";
                File.Copy(value, Environment.CurrentDirectory + _author.PathPhoto);
                OnPropertyChanged("PathPhoto");
            }
        }

        //Ученые степени

        private List<AcademicDegree> _academicDegrees;
        private AcademicDegree _selectedAcademicDegree;

        //Асинхронный запрос к базе данных списка ученых степеней
        private async void GetAcademicDegrees()
        {
            await Task.Run(() =>
            {
                List<AcademicDegree> list = NirsSystem.GetAcademicDegrees();
                if (list == null)
                    return;
                _academicDegrees = new List<AcademicDegree>();
                foreach (var elem in list)
                    _academicDegrees.Add(elem);
                OnPropertyChanged("AcademicDegrees");
            });
        }

        //Асинхронный запрос на добавление ученой степени в список БД
        private async void AddAcademicDegreeAsync()
        {
            await Task.Run(() =>
            {
                NirsSystem.AddAcademicDegree(new AcademicDegree { AcademicDegreeName = AcademicDegreeName });
                GetAcademicDegrees();
            });
        }

        //Добавление ученой степени
        private void AddAcademicDegree() => AddAcademicDegreeAsync();

        /// <summary>
        /// Коллекция ученых степеней
        /// </summary>
        public List<AcademicDegree> AcademicDegrees
        {
            get => _academicDegrees;
        }

        /// <summary>
        /// Выбранная ученая степень
        /// </summary>
        public AcademicDegree SelectedAcademicDegree
        {
            get => _selectedAcademicDegree;
            set
            {
                _selectedAcademicDegree = value;
                _author.AcademicDegreeId = value.AcademicDegreeId;
            }
        }

        /// <summary>
        /// Название новой ученой степени
        /// </summary>
        public string AcademicDegreeName { get; set; }

        /// <summary>
        /// Добавление новой ученой степени
        /// </summary>
        public RelayCommand CommandAddAcademicDegree
        {
            get => new RelayCommand(obj => AddAcademicDegree());
        }

        //Организации

        private List<Organization> _organizations;
        private Organization _selectedOrganization;

        //Асинхронный запрос к базе данных списка организаций
        private async void GetOrganizations()
        {
            await Task.Run(() =>
            {
                List<Organization> list = NirsSystem.GetOrganizations();
                if (list == null)
                    return;
                _organizations = new List<Organization>();
                foreach (var elem in list)
                    _organizations.Add(elem);
                OnPropertyChanged("Organizations");
            });
        }

        //Асинхронный запрос на добавление организации в список БД
        private async void AddOrganizationAsync()
        {
            await Task.Run(() =>
            {
                NirsSystem.AddOrganization(new Organization { OrganizationName = OrganizationName });
                GetOrganizations();
            });
        }

        //Добавление организации
        private void AddOrganization() => AddOrganizationAsync();

        /// <summary>
        /// Коллекция организаций
        /// </summary>
        public List<Organization> Organizations
        {
            get => _organizations;
        }

        /// <summary>
        /// Выбранная организация
        /// </summary>
        public Organization SelectedOrganization
        {
            get => _selectedOrganization;
            set
            {
                _selectedOrganization = value;
                _author.OrganizationId = value.OrganizationId;
            }
        }

        /// <summary>
        /// Название новой организации
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Добавление новой организации
        /// </summary>
        public RelayCommand CommandAddOrganization
        {
            get => new RelayCommand(obj => AddOrganization());
        }

        //Должности

        private List<Position> _positions;
        private Position _selectedPosition;

        //Асинхронный запрос к базе данных списка должностей
        private async void GetPositions()
        {
            await Task.Run(() =>
            {
                List<Position> list = NirsSystem.GetPositions();
                if (list == null)
                    return;
                _positions = new List<Position>();
                foreach (var elem in list)
                    _positions.Add(elem);
                OnPropertyChanged("Positions");
            });
        }

        //Асинхронный запрос на добавление должности в список БД
        private async void AddPositionAsync()
        {
            await Task.Run(() =>
            {
                NirsSystem.AddPosition(new Position { PositionName = PositionName });
                GetPositions();
            });
        }

        //Добавление должности
        private void AddPosition() => AddPositionAsync();

        /// <summary>
        /// Коллекция должностей
        /// </summary>
        public List<Position> Positions
        {
            get => _positions;
        }

        /// <summary>
        /// Выбранная должность
        /// </summary>
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                _author.PositionId = value.PositionId;
            }
        }

        /// <summary>
        /// Название новой должности
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Добавление новой должности
        /// </summary>
        public RelayCommand CommandAddPosition
        {
            get => new RelayCommand(obj => AddPosition());
        }

        private void BackWindow()
        {
            ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new Views.AuthorsView());
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
                if (_isNew)
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objUnDone => NirsSystem.AddAuthor(_author), null);

                    //Создание команды отмены операции
                    RelayCommand undone = new RelayCommand(objUnDone => NirsSystem.DeleteAuthor(_author), null);

                    //Создание операции
                    Operation operation = new Operation("Добавлен новый автор " + Fio, done, undone);

                    NirsSystem.StackOperations.AddOperation(operation);
                    operation.DoneCommand.Execute(null);
                }
                else
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objUnDone => NirsSystem.UpdateAuthor(_author), null);

                    //Создание команды отмены операции
                    RelayCommand undone = new RelayCommand(objUnDone => NirsSystem.UpdateAuthor(_prevAuthor), null);

                    //Создание операции
                    Operation operation = new Operation("Автор " + Fio + " был изменен", done, undone);

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
                RelayCommand done = new RelayCommand(objUnDone => NirsSystem.DeleteAuthor(_author), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => NirsSystem.AddAuthor(_author), null);

                //Создание операции
                Operation operation = new Operation("Удален автор " + Fio, done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
                BackWindow();
            });
        }
    }
}