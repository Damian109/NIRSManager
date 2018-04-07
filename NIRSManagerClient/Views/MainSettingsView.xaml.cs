using NIRSCore;
using System.Windows.Controls;
using NIRSManagerClient.ViewModels.SettingsViewModels;

namespace NIRSManagerClient.Views
{
    /// <summary>
    /// Логика взаимодействия для MainSettingsView.xaml
    /// </summary>
    public partial class MainSettingsView : UserControl
    {
        public MainSettingsView(User user)
        {
            InitializeComponent();
            DataContext = new MainSettingsViewModel(user);
        }
    }
}
