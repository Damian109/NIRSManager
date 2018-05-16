using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class ExchangeView : UserControl
    {
        public ExchangeView()
        {
            InitializeComponent();

            group.Children.Clear();
            group.Children.Add(new ExchangeSuccessView());
        }
    }
}
