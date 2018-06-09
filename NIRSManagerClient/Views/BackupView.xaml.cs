using NIRSManagerClient.ViewModels;
using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class BackupView : UserControl
    {
        public BackupView()
        {
            InitializeComponent();
            DataContext = new BackupViewModel();
        }
    }
}
