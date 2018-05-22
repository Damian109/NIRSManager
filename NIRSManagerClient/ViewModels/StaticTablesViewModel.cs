using NIRSCore;
using System.Threading.Tasks;
using NIRSCore.DataBaseModels;
using NIRSCore.StackOperations;
using System.Collections.Generic;

namespace NIRSManagerClient.ViewModels
{
    public sealed class StaticTablesViewModel : ViewModel
    {
        #region Private

        //Организации
        private List<Organization> _organizations;
        private string _organizationName;

        private async void SetOrganizationsAsync() => await Task.Run(() =>
            _organizations = (List<Organization>)NirsSystem.GetListObject<Organization>());

        private async void AddOrganizationAsync(Organization organization) => await Task.Run(() =>
        {
            _organizationName = string.Empty;
            OnPropertyChanged("OrganizationName");
            NirsSystem.AddObject(organization);
            SetOrganizationsAsync();
        });

        private async void DeleteOrganizationAsync(Organization organization) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(organization);
            SetOrganizationsAsync();
        });

        //Факультеты
        List<Faculty> _faculties;
        private string _facultyName;

        private async void SetFacultiesAsync() => await Task.Run(() =>
            _faculties = (List<Faculty>)NirsSystem.GetListObject<Faculty>());

        private async void AddFacultyAsync(Faculty faculty) => await Task.Run(() =>
        {
            _facultyName = string.Empty;
            OnPropertyChanged("FacultyName");
            NirsSystem.AddObject(faculty);
            SetFacultiesAsync();
        });

        private async void DeleteFacultyAsync(Faculty faculty) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(faculty);
            SetFacultiesAsync();
        });

        //Кафедры
        List<Department> _departments;
        private string _departmentName;

        private async void SetDepartmentsAsync() => await Task.Run(() =>
            _departments = (List<Department>)NirsSystem.GetListObject<Department>());

        private async void AddDepartmentAsync(Department department) => await Task.Run(() =>
        {
            _departmentName = string.Empty;
            OnPropertyChanged("DepartmentName");
            NirsSystem.AddObject(department);
            SetDepartmentsAsync();
        });

        private async void DeleteDepartmentAsync(Department department) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(department);
            SetDepartmentsAsync();
        });

        //Группы
        List<Group> _groups;
        private string _groupName;

        private async void SetGroupsAsync() => await Task.Run(() =>
            _groups = (List<Group>)NirsSystem.GetListObject<Group>());

        private async void AddGroupAsync(Group group) => await Task.Run(() =>
        {
            _groupName = string.Empty;
            OnPropertyChanged("GroupName");
            NirsSystem.AddObject(group);
            SetGroupsAsync();
        });

        private async void DeleteGroupAsync(Group group) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(group);
            SetGroupsAsync();
        });

        //Должности
        List<Position> _positions;
        private string _positionName;

        private async void SetPositionsAsync() => await Task.Run(() =>
            _positions = (List<Position>)NirsSystem.GetListObject<Position>());

        private async void AddPositionAsync(Position position) => await Task.Run(() =>
        {
            _positionName = string.Empty;
            OnPropertyChanged("PositionName");
            NirsSystem.AddObject(position);
            SetPositionsAsync();
        });

        private async void DeletePositionAsync(Position position) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(position);
            SetPositionsAsync();
        });

        //Ученые степени
        List<AcademicDegree> _academicDegrees;
        private string _academicDegreeName;

        private async void SetAcademicDegreesAsync() => await Task.Run(() =>
            _academicDegrees = (List<AcademicDegree>)NirsSystem.GetListObject<AcademicDegree>());

        private async void AddAcademicDegreeAsync(AcademicDegree academicDegree) => await Task.Run(() =>
        {
            _academicDegreeName = string.Empty;
            OnPropertyChanged("AcademicDegreeName");
            NirsSystem.AddObject(academicDegree);
            SetAcademicDegreesAsync();
        });

        private async void DeleteAcademicDegreeAsync(AcademicDegree academicDegree) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(academicDegree);
            SetAcademicDegreesAsync();
        });

        //Направления
        List<Direction> _directions;
        private string _directionName;

        private async void SetDirectionsAsync() => await Task.Run(() =>
            _directions = (List<Direction>)NirsSystem.GetListObject<Direction>());

        private async void AddDirectionAsync(Direction direction) => await Task.Run(() =>
        {
            _directionName = string.Empty;
            OnPropertyChanged("DirectionName");
            NirsSystem.AddObject(direction);
            SetDirectionsAsync();
        });

        private async void DeleteDirectionAsync(Direction direction) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(direction);
            SetDirectionsAsync();
        });

        //Награды
        List<Reward> _rewards;
        private string _rewardName;

        private async void SetRewardsAsync() => await Task.Run(() =>
            _rewards = (List<Reward>)NirsSystem.GetListObject<Reward>());

        private async void AddRewardAsync(Reward reward) => await Task.Run(() =>
        {
            _rewardName = string.Empty;
            OnPropertyChanged("RewardName");
            NirsSystem.AddObject(reward);
            SetRewardsAsync();
        });

        private async void DeleteRewardAsync(Reward reward) => await Task.Run(() =>
        {
            NirsSystem.DeleteObject(reward);
            SetRewardsAsync();
        });

        //Получение тсатуса ввода для надписей
        private string GetStatus(int value)
        {
            switch (value)
            {
                case 1:
                    return "Введите данные";
                case 2:
                    return "Запись уже существует";
                case 3:
                    return "Данные введены верно";
                default:
                    return "";
            }
        }

        //Заполнение всех нужных полей значениями
        private async void SetAllFieldsAsync() => await Task.Run(() =>
        {
            SetOrganizationsAsync();
            SetDepartmentsAsync();
            SetFacultiesAsync();
            SetGroupsAsync();
            SetPositionsAsync();
            SetAcademicDegreesAsync();
            SetDirectionsAsync();
            SetRewardsAsync();
        });

        //Инициализация полей ввода и статусных надписей
        private void Initialise()
        {
            _organizationName = _facultyName = _departmentName = _groupName = string.Empty;
            _positionName = _academicDegreeName = _directionName = _rewardName = string.Empty;
            OrganizationStatus = FacultyStatus = DepartmentStatus = GroupStatus = GetStatus(1);
            PositionStatus = AcademicDegreeStatus = DirectionStatus = RewardStatus = GetStatus(1);
            OrganizationEnabled = FacultyEnabled = DepartmentEnabled = GroupEnabled = false;
            PositionEnabled = AcademicDegreeEnabled = DirectionEnabled = RewardEnabled = false;
        }

        #endregion
        
        /// <summary>
        /// Статус организации
        /// </summary>
        public string OrganizationStatus { get; set; }

        /// <summary>
        /// доступно ли добавление организации?
        /// </summary>
        public bool OrganizationEnabled { get; set; }

        /// <summary>
        /// Название организации
        /// </summary>
        public string OrganizationName
        {
            get => _organizationName;
            set
            {
                _organizationName = value;
                if (_organizationName == string.Empty)
                {
                    OrganizationStatus = GetStatus(1);
                    OrganizationEnabled = false;
                    OnPropertyChanged("OrganizationStatus");
                    OnPropertyChanged("OrganizationEnabled");
                    return;
                }
                if(_organizations != null)
                {
                    foreach (var elem in _organizations)
                        if (elem.OrganizationName == _organizationName)
                        {
                            OrganizationStatus = GetStatus(2);
                            OrganizationEnabled = false;
                            OnPropertyChanged("OrganizationStatus");
                            OnPropertyChanged("OrganizationEnabled");
                            return;
                        }
                }
                OrganizationStatus = GetStatus(3);
                OrganizationEnabled = true;
                OnPropertyChanged("OrganizationStatus");
                OnPropertyChanged("OrganizationEnabled");
                return;
            }
        }

        /// <summary>
        /// Статус кафедры
        /// </summary>
        public string DepartmentStatus { get; set; }

        /// <summary>
        /// доступно ли добавление кафедры?
        /// </summary>
        public bool DepartmentEnabled { get; set; }

        /// <summary>
        /// Название кафедры
        /// </summary>
        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                _departmentName = value;
                if (_departmentName == string.Empty)
                {
                    DepartmentStatus = GetStatus(1);
                    DepartmentEnabled = false;
                    OnPropertyChanged("DepartmentStatus");
                    OnPropertyChanged("DepartmentEnabled");
                    return;
                }
                if(_departments != null)
                {
                    foreach (var elem in _departments)
                        if (elem.DepartmentName == _departmentName)
                        {
                            DepartmentStatus = GetStatus(2);
                            DepartmentEnabled = false;
                            OnPropertyChanged("DepartmentStatus");
                            OnPropertyChanged("DepartmentEnabled");
                            return;
                        }
                }
                DepartmentStatus = GetStatus(3);
                DepartmentEnabled = true;
                OnPropertyChanged("DepartmentStatus");
                OnPropertyChanged("DepartmentEnabled");
                return;
            }
        }

        /// <summary>
        /// Статус факультета
        /// </summary>
        public string FacultyStatus { get; set; }

        /// <summary>
        /// доступно ли добавление факультета?
        /// </summary>
        public bool FacultyEnabled { get; set; }

        /// <summary>
        /// Название факультета
        /// </summary>
        public string FacultyName
        {
            get => _facultyName;
            set
            {
                _facultyName = value;
                if (_facultyName == string.Empty)
                {
                    FacultyStatus = GetStatus(1);
                    FacultyEnabled = false;
                    OnPropertyChanged("FacultyStatus");
                    OnPropertyChanged("FacultyEnabled");
                    return;
                }
                if(_faculties != null)
                {
                    foreach (var elem in _faculties)
                        if (elem.FacultyName == _facultyName)
                        {
                            FacultyStatus = GetStatus(2);
                            FacultyEnabled = false;
                            OnPropertyChanged("FacultyStatus");
                            OnPropertyChanged("FacultyEnabled");
                            return;
                        }
                }
                FacultyStatus = GetStatus(3);
                FacultyEnabled = true;
                OnPropertyChanged("FacultyStatus");
                OnPropertyChanged("FacultyEnabled");
                return;
            }
        }

        /// <summary>
        /// Статус группы
        /// </summary>
        public string GroupStatus { get; set; }

        /// <summary>
        /// доступно ли добавление группы?
        /// </summary>
        public bool GroupEnabled { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                if (_groupName == string.Empty)
                {
                    GroupStatus = GetStatus(1);
                    GroupEnabled = false;
                    OnPropertyChanged("GroupStatus");
                    OnPropertyChanged("GroupEnabled");
                    return;
                }
                if(_groups != null)
                {
                    foreach (var elem in _groups)
                        if (elem.GroupName == _groupName)
                        {
                            GroupStatus = GetStatus(2);
                            GroupEnabled = false;
                            OnPropertyChanged("GroupStatus");
                            OnPropertyChanged("GroupEnabled");
                            return;
                        }
                }
                GroupStatus = GetStatus(3);
                GroupEnabled = true;
                OnPropertyChanged("GroupStatus");
                OnPropertyChanged("GroupEnabled");
                return;
            }
        }

        /// <summary>
        /// Статус должности
        /// </summary>
        public string PositionStatus { get; set; }

        /// <summary>
        /// доступно ли добавление должности?
        /// </summary>
        public bool PositionEnabled { get; set; }

        /// <summary>
        /// Название должности
        /// </summary>
        public string PositionName
        {
            get => _positionName;
            set
            {
                _positionName = value;
                if (_positionName == string.Empty)
                {
                    PositionStatus = GetStatus(1);
                    PositionEnabled = false;
                    OnPropertyChanged("PositionStatus");
                    OnPropertyChanged("PositionEnabled");
                    return;
                }
                if(_positions != null)
                {
                    foreach (var elem in _positions)
                        if (elem.PositionName == _positionName)
                        {
                            PositionStatus = GetStatus(2);
                            PositionEnabled = false;
                            OnPropertyChanged("PositionStatus");
                            OnPropertyChanged("PositionEnabled");
                            return;
                        }
                }
                PositionStatus = GetStatus(3);
                PositionEnabled = true;
                OnPropertyChanged("PositionStatus");
                OnPropertyChanged("PositionEnabled");
                return;
            }
        }

        /// <summary>
        /// Статус ученой степени
        /// </summary>
        public string AcademicDegreeStatus { get; set; }

        /// <summary>
        /// доступно ли добавление ступени?
        /// </summary>
        public bool AcademicDegreeEnabled { get; set; }

        /// <summary>
        /// Название ученой степени
        /// </summary>
        public string AcademicDegreeName
        {
            get => _academicDegreeName;
            set
            {
                _academicDegreeName = value;
                if (_academicDegreeName == string.Empty)
                {
                    AcademicDegreeStatus = GetStatus(1);
                    AcademicDegreeEnabled = false;
                    OnPropertyChanged("AcademicDegreeStatus");
                    OnPropertyChanged("AcademicDegreeEnabled");
                    return;
                }
                if(_academicDegrees != null)
                {
                    foreach (var elem in _academicDegrees)
                        if (elem.AcademicDegreeName == _academicDegreeName)
                        {
                            AcademicDegreeStatus = GetStatus(2);
                            AcademicDegreeEnabled = false;
                            OnPropertyChanged("AcademicDegreeStatus");
                            OnPropertyChanged("AcademicDegreeEnabled");
                            return;
                        }
                }
                AcademicDegreeStatus = GetStatus(3);
                AcademicDegreeEnabled = true;
                OnPropertyChanged("AcademicDegreeStatus");
                OnPropertyChanged("AcademicDegreeEnabled");
                return;
            }
        }

        /// <summary>
        /// Статус направления
        /// </summary>
        public string DirectionStatus { get; set; }

        /// <summary>
        /// доступно ли добавление направления?
        /// </summary>
        public bool DirectionEnabled { get; set; }

        /// <summary>
        /// Название направления
        /// </summary>
        public string DirectionName
        {
            get => _directionName;
            set
            {
                _directionName = value;
                if (_directionName == string.Empty)
                {
                    DirectionStatus = GetStatus(1);
                    DirectionEnabled = false;
                    OnPropertyChanged("DirectionStatus");
                    OnPropertyChanged("DirectionEnabled");
                    return;
                }
                if(_directions != null)
                {
                    foreach (var elem in _directions)
                        if (elem.DirectionName == _directionName)
                        {
                            DirectionStatus = GetStatus(2);
                            DirectionEnabled = false;
                            OnPropertyChanged("DirectionStatus");
                            OnPropertyChanged("DirectionEnabled");
                            return;
                        }
                }
                DirectionStatus = GetStatus(3);
                DirectionEnabled = true;
                OnPropertyChanged("DirectionStatus");
                OnPropertyChanged("DirectionEnabled");
                return;
            }
        }

        /// <summary>
        /// Статус организации
        /// </summary>
        public string RewardStatus { get; set; }

        /// <summary>
        /// доступно ли добавление награды?
        /// </summary>
        public bool RewardEnabled { get; set; }

        /// <summary>
        /// Название организации
        /// </summary>
        public string RewardName
        {
            get => _rewardName;
            set
            {
                _rewardName = value;
                if (_rewardName == string.Empty)
                {
                    RewardStatus = GetStatus(1);
                    RewardEnabled = false;
                    OnPropertyChanged("RewardStatus");
                    OnPropertyChanged("RewardEnabled");
                    return;
                }
                if(_rewards != null)
                {
                    foreach (var elem in _rewards)
                        if (elem.RewardName == _rewardName)
                        {
                            RewardStatus = GetStatus(2);
                            RewardEnabled = false;
                            OnPropertyChanged("RewardStatus");
                            OnPropertyChanged("RewardEnabled");
                            return;
                        }
                }
                RewardStatus = GetStatus(3);
                RewardEnabled = true;
                OnPropertyChanged("RewardStatus");
                OnPropertyChanged("RewardEnabled");
                return;
            }
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public StaticTablesViewModel() : base("Авторы")
        {
            SetAllFieldsAsync();
            Initialise();
        }

        /// <summary>
        /// Команда добавления новой организации
        /// </summary>
        public RelayCommand CommandAddOrganization
        {
            get => new RelayCommand(obj =>
            {
                Organization organization = new Organization { OrganizationName = _organizationName };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddOrganizationAsync(organization), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteOrganizationAsync(organization), null);

                //Создание операции
                Operation operation = new Operation("Добавлена новая организация", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        /// <summary>
        /// Команда добавления нового факультета
        /// </summary>
        public RelayCommand CommandAddFaculty
        {
            get => new RelayCommand(obj =>
            {
                Faculty faculty = new Faculty { FacultyName = _facultyName };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddFacultyAsync(faculty), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteFacultyAsync(faculty), null);

                //Создание операции
                Operation operation = new Operation("Добавлен новый факультет", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        /// <summary>
        /// Команда добавления новой кафедры
        /// </summary>
        public RelayCommand CommandAddDepartment
        {
            get => new RelayCommand(obj =>
            {
                Department department = new Department { DepartmentName = _departmentName };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddDepartmentAsync(department), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteDepartmentAsync(department), null);

                //Создание операции
                Operation operation = new Operation("Добавлена новая кафедра", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        /// <summary>
        /// Команда добавления новой группы
        /// </summary>
        public RelayCommand CommandAddGroup
        {
            get => new RelayCommand(obj =>
            {
                Group group = new Group { GroupName = _groupName };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddGroupAsync(group), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteGroupAsync(group), null);

                //Создание операции
                Operation operation = new Operation("Добавлена новая группа", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        /// <summary>
        /// Команда добавления новой должности
        /// </summary>
        public RelayCommand CommandAddPosition
        {
            get => new RelayCommand(obj =>
            {
                Position position = new Position { PositionName = _positionName };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddPositionAsync(position), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeletePositionAsync(position), null);

                //Создание операции
                Operation operation = new Operation("Добавлена новая должность", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        /// <summary>
        /// Команда добавления новой ученой степени
        /// </summary>
        public RelayCommand CommandAddAcademicDegree
        {
            get => new RelayCommand(obj =>
            {
                AcademicDegree academicDegree = new AcademicDegree { AcademicDegreeName = _academicDegreeName };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddAcademicDegreeAsync(academicDegree), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteAcademicDegreeAsync(academicDegree), null);

                //Создание операции
                Operation operation = new Operation("Добавлена новая ученая степень", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        /// <summary>
        /// Команда добавления нового направления
        /// </summary>
        public RelayCommand CommandAddDirection
        {
            get => new RelayCommand(obj =>
            {
                Direction direction = new Direction { DirectionName = _directionName };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddDirectionAsync(direction), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteDirectionAsync(direction), null);

                //Создание операции
                Operation operation = new Operation("Добавлено новое направление ", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        /// <summary>
        /// Команда добавления новой награды
        /// </summary>
        public RelayCommand CommandAddReward
        {
            get => new RelayCommand(obj =>
            {
                Reward reward = new Reward { RewardName = _rewardName };

                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => AddRewardAsync(reward), null);

                //Создание команды отмены операции
                RelayCommand undone = new RelayCommand(objUnDone => DeleteRewardAsync(reward), null);

                //Создание операции
                Operation operation = new Operation("Добавлена новая награда", done, undone);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }
    }
}
