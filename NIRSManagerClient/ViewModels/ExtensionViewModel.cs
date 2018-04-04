using NIRSCore;
using NIRSManagerClient.Views;
using System.Linq;
using System.Windows;

namespace NIRSManagerClient.ViewModels
{
    public sealed class ExtensionViewModel : ViewModel
    {
        User _user;

        public ExtensionViewModel(User user) : base("Главная форма")
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new AuthorizationView());
            _user = user;
        }
    }
}