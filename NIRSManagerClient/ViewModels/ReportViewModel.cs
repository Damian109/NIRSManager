using NIRSCore;
using NIRSManagerClient.HelpfulModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSManagerClient.ViewModels
{
    public sealed class ReportViewModel : ViewModel
    {



        public ReportViewModel(List<AuthorHelper> authorHelpers, int typeOfFilter, string search, bool isAccuracy) : base("Отчеты")
        {

        }
    }
}
