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
        public List<WorkHelper> Works { get; set; }

        /// <summary>
        /// Сколькими работами автор руководил
        /// </summary>
        public int CountHeader { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="author">Автор</param>
        /// <param name="works">Список работ данного автора</param>
        /// <param name="countHeader">Сколько работ написано под руководством автора</param>
        public ReportElemHelper(AuthorHelper author, List<WorkHelper> works, int countHeader)
        {
            Author = author;
            Works = works;
            CountHeader = countHeader;
        }
    }
}
