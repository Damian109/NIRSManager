using NIRSCore;
using NIRSCore.BackupManager;
using NIRSCore.Syncronization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSManagerClient.ViewModels
{
    /// <summary>
    /// Модель представления для страницы обмена БД
    /// </summary>
    public sealed class ExchangeViewModel : ViewModel
    {
        private List<string> _logins = null;
        private List<ListExchangesData> _exchanges = null;
        private string _loginSelected;
        private ListExchangesData _exchangeSelected;

        /// <summary>
        /// Получение необходимых данных
        /// </summary>
        private async void GetAll() => await Task.Run(() =>
        {
            _logins = new List<string>();
            _exchanges = new List<ListExchangesData>();

            var exchanges = NirsSystem.GetExchanges();

            if (exchanges != null)
                foreach (var elem in exchanges.Datas)
                    _exchanges.Add(elem);

            var users = NirsSystem.GetUsers();

            if (users != null)
                foreach (var elem in users.ListLogins)
                    _logins.Add(elem);

            OnPropertyChanged("Logins");
            OnPropertyChanged("Exchanges");
        });

        /// <summary>
        /// Список логинов пользователей
        /// </summary>
        public ObservableCollection<string> Logins
        {
            get
            {
                ObservableCollection<string> logins = new ObservableCollection<string>();
                if (_logins != null)
                    foreach (var elem in _logins)
                        logins.Add(elem);
                return logins;
            }
        }

        /// <summary>
        /// Выбранный логин
        /// </summary>
        public string LoginSelected
        {
            get => _loginSelected;
            set
            {
                _loginSelected = value;
                if (_loginSelected != null)
                    IsAddEnabled = true;
                else
                    IsAddEnabled = false;
                OnPropertyChanged("IsAddEnabled");
            }
        }

        /// <summary>
        /// Возможно ли добавление обмена
        /// </summary>
        public bool IsAddEnabled { get; set; }

        /// <summary>
        /// Список обменов пользователю
        /// </summary>
        public ObservableCollection<ListExchangesData> Exchanges
        {
            get
            {
                ObservableCollection<ListExchangesData> exchanges = new ObservableCollection<ListExchangesData>();
                if (_exchanges != null)
                    foreach (var elem in _exchanges)
                        exchanges.Add(elem);
                return exchanges;
            }
        }

        /// <summary>
        /// Выбранный обмен
        /// </summary>
        public ListExchangesData ExchangeSelected
        {
            get => _exchangeSelected;
            set
            {
                _exchangeSelected = value;
                if(_exchangeSelected != null)
                {
                    IsTrueEnabled = true;
                    IsFalseEnabled = true;
                    if (_exchangeSelected.IsIAmCreator && !_exchangeSelected.IsCreatorDone && _exchangeSelected.IsSenderAccept == true)
                        IsDoneEnabled = true;
                    if (!_exchangeSelected.IsIAmCreator && !_exchangeSelected.IsSenderDone && _exchangeSelected.IsSenderAccept == true)
                        IsDoneEnabled = true;
                }
                else
                {
                    IsTrueEnabled = false;
                    IsFalseEnabled = false;
                    IsDoneEnabled = false;
                }
                OnPropertyChanged("IsTrueEnabled");
                OnPropertyChanged("IsFalseEnabled");
                OnPropertyChanged("IsDoneEnabled");
            }
        }

        /// <summary>
        /// Возможно ли принять обмен
        /// </summary>
        public bool IsTrueEnabled { get; set; }

        /// <summary>
        /// Возможно ли отклонить обмен
        /// </summary>
        public bool IsFalseEnabled { get; set; }

        /// <summary>
        /// Возможно ли выполнить обмен
        /// </summary>
        public bool IsDoneEnabled { get; set; }

        /// <summary>
        /// Односторонний ли обмен
        /// </summary>
        public bool IsOneWay { get; set; } = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ExchangeViewModel() : base("Backups") => GetAll();

        /// <summary>
        /// Команда создания нового обмена
        /// </summary>
        public RelayCommand CommandAddExchange
        {
            get => new RelayCommand(obj =>
            {
                NirsSystem.AddExchange(LoginSelected, IsOneWay);
            });
        }

    }
}
