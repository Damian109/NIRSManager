using NIRSCore;

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
        private AuthorizationStatus _status;
        private bool _isServer;
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
                    _status = AuthorizationStatus.AuthOK;
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
                    _status = AuthorizationStatus.AuthOK;
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
                        return "Такой связки Логин-Пароль не существует";
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
        /// Конструктор класса
        /// </summary>
        public AuthorizationViewModel() : base("Форма авторизации")
        {
            _login = _password = string.Empty;
        }



        //Кнопка ОК
        //Кнопка выход
        //Перейти к регистрации

        //Свойство подключения к серверу

        //Тогда должна начаться синхронизация
    }
}
