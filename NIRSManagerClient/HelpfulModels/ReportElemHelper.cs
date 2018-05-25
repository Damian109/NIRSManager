using System.Collections.Generic;

namespace NIRSManagerClient.HelpfulModels
{
    /// <summary>
    /// Предназначен для хранения элемента отчета
    /// </summary>
    public sealed class ReportElemHelper
    {
        /// <summary>
        /// Автор
        /// </summary>
        public AuthorHelper Author { get; private set; }

        /// <summary>
        /// Работы автора
        /// </summary>
        public List<WorkHelper> Works { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="author">Автор</param>
        /// <param name="works">Список работ данного автора</param>
        public ReportElemHelper(AuthorHelper author, List<WorkHelper> works)
        {
            Author = author;
            Works = works;
        }
    }
}
