using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class UserInterfaceSettingsView : UserControl
    {
        public UserInterfaceSettingsView()
        {
            InitializeComponent();
            DataContext = new ViewModels.SettingsViewModels.UserInterfaceSettingsViewModel();
        }
    }
}
