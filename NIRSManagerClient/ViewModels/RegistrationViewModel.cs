using System;
using NIRSCore;
using System.Linq;
using System.Windows;
using System.Net.Http;
using System.Windows.Media;
using NIRSCore.FileOperations;

namespace NIRSManagerClient.ViewModels
{
    public sealed class RegistrationViewModel : ViewModel
    {
        #region Private
        private string _login;
        private string _password;
        private bool _isServer;
        private Brush _color;
        private RegistrationStatus _status;

        private void DoneRegistration()
        {
            //Проверка, что регистрация идет не через сервер
            if (_isServer)
            {
                //Зарегистрироваться на сервере
                if (!RegistrationToServer())
                {
                    _status = RegistrationStatus.RegServerErr;
                    OnPropertyChanged("Status");
                    _color = Brushes.PaleVioletRed;
                    OnPropertyChanged("StatusColor");
                    return;
                }
            }

            string input = _login + _password;
            string hash = HashForSecurity.GetMd5Hash(input);

            //Добаление пользователя в каталог учетных записей
            if(!AddUserToUserFile(hash))
            {
                _status = RegistrationStatus.RegError;
                OnPropertyChanged("Status");
                _color = Brushes.PaleVioletRed;
                OnPropertyChanged("StatusColor");
                return;
            }

            //Создание Файла настроек
            FileSettings fileSettings = new FileSettings(Login, hash);
            //Создание рабочего каталога для пользователя
            fileSettings.Create();

            fileSettings.SetUser(new User());
            fileSettings.Save();

            _color = Brushes.LimeGreen;
            _status = RegistrationStatus.RegGood;
            OnPropertyChanged("StatusColor");
            OnPropertyChanged("Status");
        }

        private bool AddUserToUserFile(string hash)
        {
            FileUsers fileUsers = new FileUsers();
            fileUsers.Open();
            if (fileUsers.GetFileName(hash) != string.Empty)
                return false;
            fileUsers.AddNewUsersItem(new FileUsersItem() { Login = _login, Md5 = hash });
            fileUsers.Save();
            return true;
        }

        private bool RegistrationToServer()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage message = client.PostAsJsonAsync("http://localhost:61096/Registration/IsLogin", Login).Result;
                    string resultString = message.Content.ReadAsStringAsync().Result;
                    bool result = Convert.ToBoolean(resultString);
                    if (result)
                        return true;
                }
            }
            catch(Exception)
            {
                return false;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Перечисление, описывающее статус регистрации
        /// </summary>
        enum RegistrationStatus
        {
            RegNull,      //Данные не вводились
            RegLogin,     //Введен только логин
            RegPassword,  //Введен только пароль
            RegOK,        //Данные корректны
            RegError,     //Такой пользователь существует
            RegServerErr, //Сервер недоступен
            RegGood       //Регистрация успешна
        }

        /// <summary>
        /// Цвет текста статуса
        /// </summary>
        public Brush StatusColor { get => _color; }

        /// <summary>
        /// Переключение между регистрацией через сервер или автономным
        /// </summary>
        public bool IsServer
        {
            get => _isServer;
            set
            {
                _isServer = value;
                OnPropertyChanged("IsServer");
            }
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login
        {
            get => _login;
            set
            {
                _login = value;

                if (_status == RegistrationStatus.RegPassword || _status == RegistrationStatus.RegOK)
                {
                    _status = RegistrationStatus.RegOK;
                    _color = Brushes.LimeGreen;
                    OnPropertyChanged("StatusColor");
                }
                else
                    _status = RegistrationStatus.RegLogin;

                OnPropertyChanged("Login");
                OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password
        {
            get => _password;
            set
            {
                _password = value;

                if (_status == RegistrationStatus.RegLogin || _status == RegistrationStatus.RegOK)
                {
                    _status = RegistrationStatus.RegOK;
                    _color = Brushes.LimeGreen;
                    OnPropertyChanged("StatusColor");
                }
                else
                    _status = RegistrationStatus.RegPassword;

                OnPropertyChanged("Password");
                OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Статус ввода данных
        /// </summary>
        public string Status
        {
            get
            {
                switch (_status)
                {
                    case RegistrationStatus.RegError:
                        return "Такой логин уже существует";
                    case RegistrationStatus.RegLogin:
                        return "Введите пароль";
                    case RegistrationStatus.RegNull:
                        return "Введите необходимые данные";
                    case RegistrationStatus.RegOK:
                        return "Все данные введены";
                    case RegistrationStatus.RegPassword:
                        return "Введите Логин";
                    case RegistrationStatus.RegServerErr:
                        return "Сервер недоступен";
                    case RegistrationStatus.RegGood:
                        return "Регистрация успешна";
                    default:
                        return "Ошибка регистрации";
                }
            }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public RegistrationViewModel() : base("Форма регистрации")
        {
            _login = _password = string.Empty;
            _color = Brushes.PaleVioletRed;
            _isServer = false;
        }

        /// <summary>
        /// Команда регистрации
        /// </summary>
        public RelayCommand CommandRegistration
        {
            get => new RelayCommand(obj => DoneRegistration());
        }

        /// <summary>
        /// Команда Назад
        /// </summary>
        public RelayCommand CommandBack
        {
            get => new RelayCommand(obj =>
            {
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                window.mainGrid.Children.Clear();
                window.mainGrid.Children.Add(new Views.AuthorizationView());
            });
        }
    }
}
