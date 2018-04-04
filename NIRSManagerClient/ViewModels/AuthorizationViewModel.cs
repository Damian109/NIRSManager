using System;
using NIRSCore;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using NIRSCore.FileOperations;

namespace NIRSManagerClient.ViewModels
{
    /// <summary>
    /// Модель представления - Авторизация
    /// </summary>
    public sealed class AuthorizationViewModel : ViewModel
    {
        #region Private
        private string _login;
        private string _password;
        private Brush _color;
        private AuthorizationStatus _status;

        //Выполняется вход в приложение
        private void Enter()
        {
            FileUsers file = new FileUsers();
            file.Open();

            string input = _login + _password;
            string login = file.GetFileName(input);

            if(login == string.Empty)
            {
                _status = AuthorizationStatus.AuthError;
                _color = Brushes.PaleVioletRed;
                OnPropertyChanged("StatusColor");
                OnPropertyChanged("Status");
            }
            else
            {
                FileSettings settings = new FileSettings(login, HashForSecurity.GetMd5Hash(input));
                settings.Open();
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                window.Close();
                ExtensionView extensionView = new ExtensionView(settings.GetUser());
                extensionView.Show();
            }
        }
        #endregion

        /// <summary>
        /// Перечисление, описывающее статус авторизации
        /// </summary>
        enum AuthorizationStatus
        {
            AuthNull,      //Данные не вводились
            AuthLogin,     //Введен только логин
            AuthPassword,  //Введен только пароль
            AuthOK,        //Данные корректны
            AuthError      //Данные введены неверно
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

                if (_status == AuthorizationStatus.AuthPassword || _status == AuthorizationStatus.AuthOK)
                {
                    _status = AuthorizationStatus.AuthOK;
                    _color = Brushes.LimeGreen;
                    OnPropertyChanged("StatusColor");
                }
                else
                    _status = AuthorizationStatus.AuthLogin;

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

                if (_status == AuthorizationStatus.AuthLogin || _status == AuthorizationStatus.AuthOK)
                {
                    _status = AuthorizationStatus.AuthOK;
                    _color = Brushes.LimeGreen;
                    OnPropertyChanged("StatusColor");
                }
                else
                    _status = AuthorizationStatus.AuthPassword;

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
                    case AuthorizationStatus.AuthError:
                        return "Не удалось авторизоваться";
                    case AuthorizationStatus.AuthLogin:
                        return "Введите пароль";
                    case AuthorizationStatus.AuthNull:
                        return "Введите данные для входа";
                    case AuthorizationStatus.AuthOK:
                        return "Все данные введены";
                    case AuthorizationStatus.AuthPassword:
                        return "Введите Логин";
                    default:
                        return "Ошибка авторизации";
                }
            }
        }

        /// <summary>
        /// Цвет текста статуса
        /// </summary>
        public Brush StatusColor { get => _color; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public AuthorizationViewModel() : base("Форма авторизации")
        {
            _login = _password = string.Empty;
            _color = Brushes.PaleVioletRed;
        }

        /// <summary>
        /// Команда выхода из приложения
        /// </summary>
        public RelayCommand CommandExit
        {
            get => new RelayCommand(obj => Environment.Exit(0));
        }

        /// <summary>
        /// Команда входа
        /// </summary>
        public RelayCommand CommandEnter
        {
            get => new RelayCommand(obj => Enter());
        }

        /// <summary>
        /// Команда перехода к регистрации
        /// </summary>
        public RelayCommand CommandRegistration
        {
            get => new RelayCommand(obj =>
            {
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                window.mainGrid.Children.Clear();
                window.mainGrid.Children.Add(new Views.RegistrationView());
            });
        }
    }
}
