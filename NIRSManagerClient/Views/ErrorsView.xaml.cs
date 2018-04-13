using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    /// <summary>
    /// Логика взаимодействия для ErrorsView.xaml
    /// </summary>
    public partial class ErrorsView : UserControl
    {
        public ErrorsView()
        {
            InitializeComponent();
            DataContext = new ErrorsViewModel();
        }
    }
}
