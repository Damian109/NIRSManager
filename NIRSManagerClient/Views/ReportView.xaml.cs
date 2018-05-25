using System.Collections.Generic;
using System.Windows.Controls;
using NIRSManagerClient.HelpfulModels;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient.Views
{
    public partial class ReportView : UserControl
    {
        public ReportView(List<AuthorHelper> authorHelpers, int filter, string search)
        {
            InitializeComponent();
            DataContext = new ReportViewModel(authorHelpers, filter, search);
        }
    }
}
