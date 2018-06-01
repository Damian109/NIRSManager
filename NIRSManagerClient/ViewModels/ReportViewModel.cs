using NIRSCore;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.Threading;
using System.Threading.Tasks;
using NIRSCore.DataBaseModels;
using NIRSCore.StackOperations;
using System.Collections.Generic;
using NIRSManagerClient.HelpfulModels;

using Xceed.Words.NET;

namespace NIRSManagerClient.ViewModels
{
    /// <summary>
    /// Модель представления страницы отчетов
    /// </summary>
    public sealed class ReportViewModel : ViewModel
    {
        //Отчет
        private ReportHelper _report;

        //Список авторов
        private List<AuthorHelper> _authors;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="authorHelpers">Список авторов</param>
        /// <param name="typeOfFilter">Тип фильтра</param>
        /// <param name="search">Строка фильтра</param>
        public ReportViewModel(List<AuthorHelper> authorHelpers, int typeOfFilter, string search) : base("Отчеты")
        {
            _authors = authorHelpers;
            if (search == null || search == "")
                _report = new ReportHelper();
            else
            {
                switch (typeOfFilter)
                {
                    case 1:
                        _report = new ReportHelper("Отчет по определенным авторам: " + search);
                        break;
                    case 2:
                        _report = new ReportHelper("Отчет по организации: " + search);
                        break;
                    case 3:
                        _report = new ReportHelper("Отчет по факультету: " + search);
                        break;
                    case 4:
                        _report = new ReportHelper("Отчет по кафедре: " + search);
                        break;
                    case 5:
                        _report = new ReportHelper("Отчет по группе: " + search);
                        break;
                }
            }
        }

        /// <summary>
        /// Активно ли состояние без сортировки
        /// </summary>
        public bool IsOutSort { get; set; } = true;

        /// <summary>
        /// Активна ли сортировка по имени
        /// </summary>
        public bool IsName { get; set; } = false;

        /// <summary>
        /// Активна ли сортировка по количеству работ
        /// </summary>
        public bool IsCountWork { get; set; } = false;

        /// <summary>
        /// Заголовок отчета
        /// </summary>
        public string Header
        {
            get => _report.Header;
            set => _report.Header = value;
        }

        private bool _isPrintListWorks = false;

        /// <summary>
        /// Выводить ли список работ
        /// </summary>
        public bool IsPrintListWorks
        {
            get => _isPrintListWorks;
            set
            {
                _isPrintListWorks = value;
                if (!_isPrintListWorks)
                    IsPrintFullWork = false;
                OnPropertyChanged("IsPrintListWorks");
                OnPropertyChanged("IsPrintFullWork");
            }
        }

        /// <summary>
        /// Выводить ли полную информацию о работе
        /// </summary>
        public bool IsPrintFullWork { get; set; } = false;

        /// <summary>
        /// Выполняется ли какая-то операция (Видимость)
        /// </summary>
        public Visibility IsDone { get; private set; } = Visibility.Hidden;

        /// <summary>
        /// Статус выполнения операции
        /// </summary>
        public string StatusString { get; private set; }

        /// <summary>
        /// Команда экспорта в docx
        /// </summary>
        public RelayCommand CommandDocx
        {
            get => new RelayCommand(obj =>
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    FileName = "Report",
                    DefaultExt = ".docx",
                    Filter = "DocX (.docx)|*.docx"
                };
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objDone => SaveAsDocx(dialog.FileName), null);

                    //Создание операции
                    Operation operation = new Operation("Экспорт в формате docx", done, null);

                    NirsSystem.StackOperations.AddOperation(operation);
                    operation.DoneCommand.Execute(null);
                }
            });
        }

        /// <summary>
        /// Команда экспорта в pdf
        /// </summary>
        public RelayCommand CommandPdf
        {
            get => new RelayCommand(obj =>
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    FileName = "Report",
                    DefaultExt = ".pdf",
                    Filter = "PDF (pdf)|*.pdf"
                };

                bool? result = dialog.ShowDialog();

                if (result == true)
                {

                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objDone => SaveAsPdf(dialog.FileName), null);

                    //Создание операции
                    Operation operation = new Operation("Экспорт в формате pdf", done, null);

                    NirsSystem.StackOperations.AddOperation(operation);
                    operation.DoneCommand.Execute(null);
                }
            });
        }

        /// <summary>
        /// Команда экспорта в xlsx
        /// </summary>
        public RelayCommand CommandXlsx
        {
            get => new RelayCommand(obj =>
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    FileName = "Report",
                    DefaultExt = ".xlsx",
                    Filter = "XLSX (xlsx)|*.xlsx"
                };

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    //Создание команды выполнения операции
                    RelayCommand done = new RelayCommand(objDone => SaveAsXlsx(dialog.FileName), null);

                    //Создание операции
                    Operation operation = new Operation("Экспорт в формате xlsx", done, null);

                    NirsSystem.StackOperations.AddOperation(operation);
                    operation.DoneCommand.Execute(null);
                }
            });
        }

        //Создание отчета
        private void CreateReport()
        {
            foreach (var elem in _authors)
            {
                IsDone = Visibility.Visible;
                OnPropertyChanged("IsDone");
                StatusString = "Выполняется создание отчета";
                OnPropertyChanged("StatusString");

                //Получаем список работ
                List<Work> works = (List<Work>)NirsSystem.GetListObject<Work>();

                //Получаем количество работ под руководством автора
                int countHeader = 0;
                if (works != null)
                    countHeader = works.Count(u => u.HeadAuthorId == elem.AuthorId);

                //Получаем список соавторов
                List<CoAuthor> coAuthors = (List<CoAuthor>)NirsSystem.GetListObject<CoAuthor>();

                //Заполняем список работ автора
                List<WorkHelper> workHelpers = new List<WorkHelper>();
                if (coAuthors != null)
                {
                    foreach (var query in coAuthors)
                        if (query.AuthorId == elem.AuthorId)
                        {
                            Work work = works.FirstOrDefault(u => u.WorkId == query.WorkId);
                            if (work != null)
                                workHelpers.Add(new WorkHelper(work));
                        }
                }

                ReportElemHelper reportElemHelper = new ReportElemHelper(elem, workHelpers, countHeader);
                _report.ReportElemHelpers.Add(reportElemHelper);
            }
            Thread.Sleep(3000);
        }

        private async void SaveAsDocx(string filename) => await Task.Run(() =>
        {
            CreateReport();
            StatusString = "Выполняется сохранения отчета";
            OnPropertyChanged("StatusString");

            using (var docx = DocX.Create(filename))
            {
                //Формирование заголовка
                docx.InsertParagraph(_report.Header).FontSize(25.0d).Bold().Alignment = Alignment.center;

                foreach(var elem in _report.ReportElemHelpers)
                {
                    //Формирование заполнения информацией об авторе
                    Paragraph authorParagraph = docx.InsertParagraph();
                    authorParagraph.Alignment = Alignment.right;

                    //Заполнение ФИО и степени
                    authorParagraph.AppendLine(elem.Author.AuthorName).Bold().FontSize(20.0d).Append("  " + 
                        elem.Author.AcademicDegreeName).Italic().FontSize(12.0d);

                    //Заполнение Организации/Факультета/Кафедры
                    authorParagraph.AppendLine();
                    if (elem.Author.OrganizationName != "")
                        authorParagraph.Append(elem.Author.OrganizationName + " / ").FontSize(14.0d);
                    if (elem.Author.FacultyName != "")
                        authorParagraph.Append(elem.Author.FacultyName + " / ").FontSize(14.0d);
                    if (elem.Author.DepartmentName != "")
                        authorParagraph.Append(elem.Author.DepartmentName + " / ").FontSize(14.0d);

                    //Заполнение должности или группы обучения
                    authorParagraph.AppendLine();
                    if (elem.Author.PositionName != "")
                        authorParagraph.Append(elem.Author.PositionName).FontSize(12.0d);
                    else if (elem.Author.GroupName != "")
                        authorParagraph.Append(elem.Author.GroupName).FontSize(12.0d);

                    //Заполнение статистики по работам
                    int count = elem.Works.Count;
                    authorParagraph.AppendLine("Работ написано: " + count.ToString()).FontSize(10.0d);
                    if(elem.CountHeader > 0)
                        authorParagraph.AppendLine("Является руководителем " + elem.CountHeader.ToString() + " работ").FontSize(10.0d);

                    authorParagraph.InsertHorizontalLine(size:3);

                    if(IsPrintListWorks && elem.Works.Count > 0)
                    {
                        foreach(var work in elem.Works)
                        {
                            //Заполнение информации о работах
                            Paragraph workParagraph = docx.InsertParagraph();

                            workParagraph.AppendLine(work.WorkName).FontSize(14.0d);
                            if(work.MarkWork != "")
                                workParagraph.AppendLine(work.MarkWork).FontSize(10.0d);
                            if (work.SizeWork != "")
                                workParagraph.AppendLine(work.SizeWork).FontSize(10.0d);

                            //Вывод подробной информации о работе
                            if (IsPrintFullWork)
                            {
                                workParagraph.AppendLine(work.Authors).FontSize(10.0d);
                                if (work.DirectionsWork != "")
                                    workParagraph.AppendLine(work.DirectionsWork).FontSize(10.0d);
                                if (work.JournalOrConference != "")
                                    workParagraph.AppendLine(work.JournalOrConference).FontSize(10.0d);
                            }
                        }
                    }
                }

                docx.Save();
            }
            Thread.Sleep(2000);
            IsDone = Visibility.Hidden;
            OnPropertyChanged("IsDone");
        });


        private void SaveAsPdf(string filename)
        {
            CreateReport();
            StatusString = "Выполняется сохранения отчета";
            OnPropertyChanged("StatusString");
            Thread.Sleep(5000);
            IsDone = Visibility.Hidden;
            OnPropertyChanged("IsDone");
        }

        private void SaveAsXlsx(string filename)
        {
            CreateReport();
            StatusString = "Выполняется сохранения отчета";
            OnPropertyChanged("StatusString");
            Thread.Sleep(5000);
            IsDone = Visibility.Hidden;
            OnPropertyChanged("IsDone");
        }

    }
}
