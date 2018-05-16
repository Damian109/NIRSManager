using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class ErrorsView : UserControl
    {
        public ErrorsView()
        {
            InitializeComponent();
            DataContext = new ErrorsViewModel();
        }
    }
}
