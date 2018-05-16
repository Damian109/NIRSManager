using NIRSManagerClient.ViewModels.SettingsViewModels;
using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class ConnectionSettingsView : UserControl
    {
        public ConnectionSettingsView()
        {
            InitializeComponent();
            DataContext = new ConnectionSettingsViewModel();
        }
    }
}

