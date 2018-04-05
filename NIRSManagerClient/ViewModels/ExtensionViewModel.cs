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
            ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new MenuPanelView(user));
            _user = user;
        }
    }
}