using System.Windows.Controls;
using NIRSManagerClient.ViewModels.SettingsViewModels;

namespace NIRSManagerClient.Views
{
    public partial class MainSettingsView : UserControl
    {
        public MainSettingsView()
        {
            InitializeComponent();
            DataContext = new MainSettingsViewModel();
        }
    }
}
