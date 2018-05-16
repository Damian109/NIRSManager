using NIRSManagerClient.ViewModels;
using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class AuthorizationView : UserControl
    {
        public AuthorizationView()
        {
            InitializeComponent();
            DataContext = new AuthorizationViewModel();
        }
    }
}
