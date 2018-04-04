using MaterialDesignThemes.Wpf;
using NIRSCore;
using NIRSCore.FileOperations;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media;

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
            RegError,     //Данные введены неверно
            RegServerErr  //Сервер недоступен
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
