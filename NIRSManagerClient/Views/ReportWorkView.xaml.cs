using System.Collections.Generic;
using System.Windows.Controls;
using NIRSManagerClient.HelpfulModels;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class ReportWorkView : UserControl
    {
        public ReportWorkView(List<WorkHelper> works, int filter, string search)
        {
            InitializeComponent();
            DataContext = new ReportWorkViewModel(works, filter, search);
        }
    }
}
