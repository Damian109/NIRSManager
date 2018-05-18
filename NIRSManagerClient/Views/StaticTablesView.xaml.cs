using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class StaticTablesView : UserControl
    {
        public StaticTablesView()
        {
            InitializeComponent();
            DataContext = new StaticTablesViewModel(); 
        }
    }
}
