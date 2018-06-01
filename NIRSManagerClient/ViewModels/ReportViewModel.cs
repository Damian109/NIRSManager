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
            Thread.Sleep(5000);
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
