using NIRSCore;
using System.Linq;
using System.Windows;
using NIRSManagerClient.Views;

namespace NIRSManagerClient.ViewModels
{
    /// <summary>
    /// Модель представления для окна Авторизации / Регистрации
    /// </summary>
    public sealed class MainWindowViewModel : ViewModel
    {
        /// <summary>
        /// Конструктор класса. Производятся первоначальные настройки пользовательского интерфейса
        /// </summary>
        public MainWindowViewModel() : base("Авторизация / Регистрация")
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new AuthorizationView());
        }
    }
}
