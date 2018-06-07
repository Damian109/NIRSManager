using System.Windows.Controls;
using NIRSManagerClient.ViewModels.SettingsViewModels;

namespace NIRSManagerClient.Views
{
    public partial class ServerSettingsView : UserControl
    {
        public ServerSettingsView()
        {
            InitializeComponent();
            DataContext = new ServerSettingsViewModel();
        }
    }
}
