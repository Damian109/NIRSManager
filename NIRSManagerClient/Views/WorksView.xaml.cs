using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class WorksView : UserControl
    {
        public WorksView()
        {
            InitializeComponent();
            DataContext = new WorksViewModel();
        }
    }
}
