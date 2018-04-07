using System;
using NIRSCore;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using NIRSCore.FileOperations;
using NIRSCore.ErrorManager;
using NIRSCore.StackOperations;

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
            string input = _login + _password;
            LoginInFile(input);
            if (_status == AuthorizationStatus.AuthError)
                return;
            try
            {
                FileSettings settings = new FileSettings(_login, HashForSecurity.GetMd5Hash(input));
                settings.Open();
                ExtensionView extensionView = new ExtensionView(settings.GetUser());
                extensionView.Show();
                StackOperations.AddOperation(new Operation("Вход в систему", null, null));
            }
            catch (NirsException exception)
            {
                ErrorManager.ExecuteException(exception);
            }
            finally
            {
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                window.Close();
            }
        }

        //Проверка есть ли логин в файле пользователей
        private void LoginInFile(string input)
        {
            string login = string.Empty;
            try
            {
                FileUsers file = new FileUsers();
                file.Open();
                login = file.GetFileName(input);
            }
            catch(NirsException exception)
            {
                ErrorManager.ExecuteException(exception);
            }
            finally
            {
                if (login == string.Empty)
                {
                    _status = AuthorizationStatus.AuthError;
                    _color = Brushes.PaleVioletRed;
                    OnPropertyChanged("StatusColor");
                    OnPropertyChanged("Status");
                }
            }
        }

        //Проверка логина для статуса
        private bool IsStatusLoginNull()
        {
            if (_login == string.Empty)
            {
                _status = AuthorizationStatus.AuthLogin;
                _color = Brushes.PaleVioletRed;
                return true;
            }
            return false;
        }

        //Проверка пароля для статуса
        private bool IsStatusPasswordNull()
        {
            if (!IsStatusLoginNull())
            {
                if (_password == string.Empty)
                {
                    _status = AuthorizationStatus.AuthPassword;
                    _color = Brushes.PaleVioletRed;
                    return true;
                }
                _status = AuthorizationStatus.AuthOK;
                _color = Brushes.LimeGreen;
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Перечисление, описывающее статус авторизации
        /// </summary>
        enum AuthorizationStatus
        {
            AuthLogin,     //Необходимо ввести логин
            AuthPassword,  //Необходимо ввести пароль
            AuthOK,        //Все данные введены
            AuthError      //Авторизация не удалась
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

                IsStatusPasswordNull();

                OnPropertyChanged("StatusColor");
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

                IsStatusPasswordNull();

                OnPropertyChanged("StatusColor");
                OnPropertyChanged("Login");
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
                    case AuthorizationStatus.AuthLogin:
                        return "Необходимо ввести логин";
                    case AuthorizationStatus.AuthPassword:
                        return "Необходимо ввести пароль";
                    case AuthorizationStatus.AuthOK:
                        return "Все данные введены";
                    case AuthorizationStatus.AuthError:
                        return "Авторизация не удалась";
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
            _status = AuthorizationStatus.AuthLogin;
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
