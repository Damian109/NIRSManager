using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class AuthorsView : UserControl
    {
        public AuthorsView()
        {
            InitializeComponent();
            DataContext = new AuthorsViewModel();
        }
    }
}
