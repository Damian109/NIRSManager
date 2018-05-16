using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorsView.xaml
    /// </summary>
    public partial class AuthorsView : UserControl
    {
        public AuthorsView()
        {
            InitializeComponent();
            DataContext = new AuthorsViewModel();
        }
    }
}
