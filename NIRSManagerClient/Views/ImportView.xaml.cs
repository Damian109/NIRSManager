using NIRSManagerClient.ViewModels;
using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class ImportView : UserControl
    {
        public ImportView()
        {
            InitializeComponent();
            DataContext = new ImportViewModel();
        }
    }
}
