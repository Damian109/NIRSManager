using NIRSCore;
using System.Linq;
using System.Windows;
using MaterialDesignColors;
using System.ComponentModel;
using NIRSManagerClient.Views;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using NIRSCore.StackOperations;
using System.Collections.Generic;

namespace NIRSManagerClient.ViewModels
{
    public sealed class ExtensionViewModel : ViewModel
    {
        #region Private

        /// <summary>
        /// Получение последней выполненной операции
        /// </summary>
        private void GetLastOperation() =>
            LastOperation = NirsSystem.StackOperations.Operations.FirstOrDefault().Name;

        /// <summary>
        /// Обработка закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if(NirsSystem.ProgramSettings.IsMinimizeToTray)
            {
                e.Cancel = true;
                return;
            }
            NirsSystem.Close();
        }

        /// <summary>
        /// Обработка события изменения ФИО
        /// </summary>
        private void _user_ChangeFIOEvent() => GetFio();

        /// <summary>
        /// Обработка события - изменение статуса доступности сервера
        /// </summary>
        private void StatusServer_changed()
        {
            if (NirsSystem.IsServer)
                ServerStatus = "Подключение к серверу успешно";
            else
                ServerStatus = "Сервер недоступен";
            OnPropertyChanged("ServerStatus");
        }

        //
        private void StackOperations_ChangeStatusEvent() => OnPropertyChanged("Operations");

        /// <summary>
        /// Получение ФИО пользователя
        /// </summary>
        private void GetFio()
        {
            if (NirsSystem.User.SurName == string.Empty && NirsSystem.User.Name == string.Empty &&
                NirsSystem.User.SecondName == string.Empty)
                FIO = "(ФИО не указано)";
            else
                FIO = $"{NirsSystem.User.SurName} {NirsSystem.User.Name} {NirsSystem.User.SecondName}";
            OnPropertyChanged("FIO");
        }

        /// <summary>
        /// Загрузка нового элемента в основное окно
        /// </summary>
        /// <param name="view"></param>
        private void LoadChild(UserControl view)
        {
            ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(view);
        }

        #endregion

        /// <summary>
        /// Фамилия, Имя, Отчество пользователя
        /// </summary>
        public string FIO { get; private set; }

        /// <summary>
        /// Позиция основного меню
        /// </summary>
        public bool IsLeftPosition { get; private set; }

        /// <summary>
        /// Последняя операция
        /// </summary>
        public string LastOperation { get; private set; }

        /// <summary>
        /// Статус доступности сервера
        /// </summary>
        public string ServerStatus { get; private set; }

        /// <summary>
        /// Список операций
        /// </summary>
        public List<Operation> Operations
        {
            get => NirsSystem.StackOperations.Operations;
        }

        public ExtensionViewModel(bool status) : base("Главная форма")
        {
            NirsSystem.User.ChangeFIOEvent += _user_ChangeFIOEvent;
            NirsSystem.StackOperations.ChangeStatusEvent += StackOperations_ChangeStatusEvent;
            GetFio();
            IsLeftPosition = NirsSystem.User.IsLeftPosition;
            GetLastOperation();

            //Настройки интерфейса
            new PaletteHelper().SetLightDark(NirsSystem.User.IsDarkTheme);
            Swatch swatch = new SwatchesProvider().Swatches.Where(u => u.Name == NirsSystem.User.MainColors).FirstOrDefault();
            if(swatch != null)
                new PaletteHelper().ReplacePrimaryColor(swatch);
            Swatch swatchAc = new SwatchesProvider().Swatches.Where(u => u.Name == NirsSystem.User.AdditionalColors).FirstOrDefault();
            if(swatchAc != null)
                new PaletteHelper().ReplaceAccentColor(swatchAc);

            //Загрузка статуса сервера
            StatusServer_changed();
            NirsSystem.ChangeStatusServer += StatusServer_changed;

            if(!status)
            {
                //LoadChild();
                return;
            }

            if (NirsSystem.IsDatabaseContextCreated)
                LoadChild(new AuthorsView());
            else
                LoadChild(new ConnectionSettingsView());
        }

        //Команды переходов по меню

        /// <summary>
        /// Команда Настройки профиля
        /// </summary>
        public RelayCommand CommandMainSettingsLoad
        {
            get => new RelayCommand(obj => LoadChild(new MainSettingsView()));
        }

        /// <summary>
        /// Команда Настройки графического интерфейса
        /// </summary>
        public RelayCommand CommandUserInterfaceSettingsLoad
        {
            get => new RelayCommand(obj => LoadChild(new UserInterfaceSettingsView()));
        }

        /// <summary>
        /// Команда Настройки подключения
        /// </summary>
        public RelayCommand CommandConnectionSettingsLoad
        {
            get => new RelayCommand(obj => LoadChild(new ConnectionSettingsView()));
        }

        /// <summary>
        /// Команда Настройки редактора кода
        /// </summary>
        public RelayCommand CommandEditorSettingsLoad
        {
            get => new RelayCommand(obj => LoadChild(new EditorSettingsView()));
        }

        /// <summary>
        /// Команда Настройки синхронизации
        /// </summary>
        public RelayCommand CommandServerSettingsLoad
        {
            get => new RelayCommand(obj => LoadChild(new ServerSettingsView()));
        }

        /// <summary>
        /// Команда Настройки поведения программы
        /// </summary>
        public RelayCommand CommandProgramSettingsLoad
        {
            get => new RelayCommand(obj => LoadChild(new ProgramSettingsView()));
        }

        /// <summary>
        /// Команда Диспетчер ошибок
        /// </summary>
        public RelayCommand CommandErrorsLoad
        {
            get => new RelayCommand(obj => LoadChild(new ErrorsView()));
        }

        /// <summary>
        /// Команда О программе
        /// </summary>
        public RelayCommand CommandAboutLoad
        {
            get => new RelayCommand(obj => LoadChild(new AboutView()));
        }

        /// <summary>
        /// Команда Резервные копии
        /// </summary>
        public RelayCommand CommandBackupsLoad
        {
            get => new RelayCommand(obj => LoadChild(new BackupView()));
        }

        /// <summary>
        /// Команда Обмен БД
        /// </summary>
        public RelayCommand CommandExchangeLoad
        {
            get => new RelayCommand(obj => LoadChild(new ExchangeView()));
        }
    }
}