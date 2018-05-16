using System.Windows;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
