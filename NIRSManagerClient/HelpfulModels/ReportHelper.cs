using System.Collections.Generic;

namespace NIRSManagerClient.HelpfulModels
{
    /// <summary>
    /// Отчет
    /// </summary>
    public sealed class ReportHelper
    {
        /// <summary>
        /// Заголовок отчета
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Список элементов отчета
        /// </summary>
        public List<ReportElemHelper> ReportElemHelpers { get; set; }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public ReportHelper() : this("Отчет по авторам") { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="header">Заголовок отчета</param>
        public ReportHelper(string header)
        {
            Header = header;
            ReportElemHelpers = new List<ReportElemHelper>();
        }
    }
}
