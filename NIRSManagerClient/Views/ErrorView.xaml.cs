using NIRSCore;
using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class ErrorView : UserControl
    {
        public ErrorView(string head, string message, RelayCommand command)
        {
            InitializeComponent();
            DataContext = new ErrorViewModel(head, message, command);
        }
    }
}
