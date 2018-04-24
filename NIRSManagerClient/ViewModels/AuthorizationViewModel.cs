using System;
using NIRSCore;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using NIRSCore.ErrorManager;
using NIRSCore.StackOperations;
using NIRSCore.HelpfulEnumsStructs;

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
            if (_status != AuthorizationStatus.AuthOK)
                return;
            try
            {
                NirsSystem.Authorization(Login, Password, true);
                NirsSystem.OpenUserSettings();
                NirsSystem.StackOperations.AddOperation(new Operation("Вход в систему", null, null));

                ExtensionView extensionView = new ExtensionView();
                extensionView.Show();
            }
            catch (NirsException exception)
            {
                NirsSystem.ErrorManager.ExecuteException(exception);
            }
            finally
            {
                MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                window.Close();
            }
        }

        #endregion

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                _status = NirsSystem.Authorization(Login, Password, false);
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
                _status = NirsSystem.Authorization(Login, Password, false);
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
                    case AuthorizationStatus.AuthLogin:
                        _color = Brushes.PaleVioletRed;
                        return "Необходимо ввести логин";
                    case AuthorizationStatus.AuthPassword:
                        _color = Brushes.PaleVioletRed;
                        return "Необходимо ввести пароль";
                    case AuthorizationStatus.AuthOK:
                        _color = Brushes.LimeGreen;
                        return "Все данные введены";
                    case AuthorizationStatus.AuthError:
                        _color = Brushes.PaleVioletRed;
                        return "Авторизация не удалась";
                    default:
                        _color = Brushes.PaleVioletRed;
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
