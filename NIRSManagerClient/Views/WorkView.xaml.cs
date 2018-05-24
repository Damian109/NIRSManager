using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class WorkView : UserControl
    {
        public WorkView(int id)
        {
            InitializeComponent();
            DataContext = new WorkViewModel(id);
        }
    }
}
