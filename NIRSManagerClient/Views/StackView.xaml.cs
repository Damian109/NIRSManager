using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class StackView : UserControl
    {
        public StackView()
        {
            InitializeComponent();
            DataContext = new StackViewModel();
        }
    }
}
