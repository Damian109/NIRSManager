using System.Windows;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class StackWindowView : Window
    {
        public StackWindowView()
        {
            InitializeComponent();
            DataContext = new StackViewModel();
        }
    }
}
