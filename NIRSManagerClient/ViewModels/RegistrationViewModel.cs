using NIRSCore;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using NIRSCore.HelpfulEnumsStructs;

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

        //Завершение регистрации
        private void DoneRegistration()
        {
            _status = NirsSystem.Registration(Login, Password, IsServer, true);
            OnPropertyChanged("Status");
            OnPropertyChanged("StatusColor");
        }

        #endregion

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
                _status = NirsSystem.Registration(Login, Password, IsServer, false);
                OnPropertyChanged("Login");
                OnPropertyChanged("Status");
                OnPropertyChanged("StatusColor");
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
                _status = NirsSystem.Registration(Login, Password, IsServer, false);
                OnPropertyChanged("Password");
                OnPropertyChanged("Status");
                OnPropertyChanged("StatusColor");
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
                    case RegistrationStatus.RegLogin:
                        _color = Brushes.PaleVioletRed;
                        return "Введите Логин";
                    case RegistrationStatus.RegError:
                        _color = Brushes.PaleVioletRed;
                        return "Такой логин уже существует";
                    case RegistrationStatus.RegPassword:
                        _color = Brushes.PaleVioletRed;
                        return "Введите пароль";
                    case RegistrationStatus.RegOK:
                        _color = Brushes.LimeGreen;
                        return "Все данные введены";
                    case RegistrationStatus.RegServerErr:
                        _color = Brushes.PaleVioletRed;
                        return "Сервер недоступен";
                    case RegistrationStatus.RegGood:
                        _color = Brushes.LimeGreen;
                        return "Регистрация успешна";
                    default:
                        _color = Brushes.PaleVioletRed;
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