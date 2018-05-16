using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class AuthorView : UserControl
    {
        public AuthorView(int id)
        {
            InitializeComponent();
            DataContext = new AuthorViewModel(id);
        }
    }
}
