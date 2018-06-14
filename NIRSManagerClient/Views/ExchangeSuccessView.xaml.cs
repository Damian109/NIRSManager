using NIRSManagerClient.ViewModels;
using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class ExchangeSuccessView : UserControl
    {
        public ExchangeSuccessView()
        {
            InitializeComponent();
            DataContext = new ExchangeViewModel();
        }
    }
}
