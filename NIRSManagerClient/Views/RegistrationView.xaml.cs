using NIRSManagerClient.ViewModels;
using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class RegistrationView : UserControl
    {
        public RegistrationView()
        {
            InitializeComponent();
            DataContext = new RegistrationViewModel();
        }
    }
}
