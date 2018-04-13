using System;
using NIRSCore;
using System.Linq;
using System.Windows;
using System.Net.Http;
using System.Windows.Media;
using NIRSCore.FileOperations;
using NIRSCore.Syncronization;
using NIRSCore.StackOperations;

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

        //Регистрация пользователя на сервере
        private bool RegistrationToServer()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage message = client.PostAsJsonAsync("http://localhost:61096/Registration/RegistrationUser",
                        new RegistrationData(_login, HashForSecurity.GetMd5Hash(_login + _password))).Result;
                    string resultString = message.Content.ReadAsStringAsync().Result;
                    bool result = Convert.ToBoolean(resultString);
                    if (result)
                        return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        //Завершение регистрации
        private void DoneRegistration()
        {
            if (_status != RegistrationStatus.RegOK)
                return;

            //Проверка, что регистрация идет через сервер
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
            
            //При регистрации через сервер нужно сохранить данные для входа
            if(_isServer)
            {
                NirsSystem.User.LoginToServer = _login;
                NirsSystem.User.PasswordToServer = _password;
            }

            _color = Brushes.LimeGreen;
            _status = RegistrationStatus.RegGood;
            OnPropertyChanged("StatusColor");
            OnPropertyChanged("Status");
            NirsSystem.StackOperations.AddOperation(new Operation("Регистрация", null, null));
        }

        //Добавление пользователя в файл учетных записей
        private bool AddUserToUserFile(string hash)
        {
            if (NirsSystem.FileUsers.GetFileName(hash) != string.Empty)
                return false;
            NirsSystem.FileUsers.AddNewUsersItem(new FileUsersItem() { Login = _login, Md5 = hash });
            return true;
        }

        //Проверка Логина на пустое значение
        private bool IsLoginNull()
        {
            if (_login == string.Empty)
            {
                _status = RegistrationStatus.RegLogin;
                _color = Brushes.PaleVioletRed;
                return true;
            }
            return false;
        }

        //Проверка правильности Логина на сервере
        private bool IsLoginCorrectServer(HttpResponseMessage message)
        {
            string resultString = message.Content.ReadAsStringAsync().Result;
            bool result = Convert.ToBoolean(resultString);
            if (result)
            {
                return false;
            }
            else
            {
                _status = RegistrationStatus.RegError;
                _color = Brushes.PaleVioletRed;
                return true;
            }
        }

        //Проверка Логина на сервере
        private bool IsLoginServer()
        {
            if (IsLoginNull())
                return true;
            if (!_isServer)
                return false;
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage message = client.PostAsJsonAsync("http://localhost:61096/Registration/IsLogin",
                    new LoginData(_login)).Result;
                    bool success = message.IsSuccessStatusCode;
                    if (success)
                    {
                        return IsLoginCorrectServer(message);
                    }
                    else
                    {
                        _status = RegistrationStatus.RegServerErr;
                        _color = Brushes.PaleVioletRed;
                        return true;
                    }
                }
                catch(Exception)
                {
                    _status = RegistrationStatus.RegServerErr;
                    _color = Brushes.PaleVioletRed;
                    return true;
                }
            }
        }

        //Проверка Логина на клиенте
        private bool IsLoginClient()
        {
            if (IsLoginServer())
                return true;
            if (NirsSystem.FileUsers.GetKey(_login) != string.Empty)
            {
                _status = RegistrationStatus.RegError;
                _color = Brushes.PaleVioletRed;
                return true;
            }
            return false;
        }

        //Проверка Пароля
        private bool IsPasswordNull()
        {
            if (!IsLoginClient())
            {
                if (_password == string.Empty)
                {
                    _status = RegistrationStatus.RegPassword;
                    _color = Brushes.PaleVioletRed;
                    return true;
                }
                _status = RegistrationStatus.RegOK;
                _color = Brushes.LimeGreen;
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Перечисление, описывающее статус регистрации
        /// </summary>
        enum RegistrationStatus
        {
            RegLogin,     //Ввести логин
            RegPassword,  //Ввести пароль
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

                IsPasswordNull();

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

                IsPasswordNull();

                OnPropertyChanged("StatusColor");
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
                    case RegistrationStatus.RegLogin:
                        return "Введите Логин";
                    case RegistrationStatus.RegError:
                        return "Такой логин уже существует";
                    case RegistrationStatus.RegPassword:
                        return "Введите пароль";
                    case RegistrationStatus.RegOK:
                        return "Все данные введены";
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