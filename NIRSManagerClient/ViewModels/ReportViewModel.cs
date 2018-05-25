using NIRSCore;
using System.Collections.Generic;
using NIRSManagerClient.HelpfulModels;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIRSCore.StackOperations;
using System.Threading;
using Microsoft.Win32;

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
                switch(typeOfFilter)
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

        /// <summary>
        /// Выводить ли список работ
        /// </summary>
        public bool IsPrintListWorks { get; set; } = false;

        /// <summary>
        /// Выводить ли полную информацию о работе
        /// </summary>
        public bool IsPrintFullWork { get; set; } = false;

        /// <summary>
        /// Выполняется ли какая-то операция
        /// </summary>
        public bool IsDone { get; private set; } = false;

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

                if(result == true)
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
                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => SaveAsPdf(""), null);

                //Создание операции
                Operation operation = new Operation("Экспорт в формате pdf", done, null);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }






        /// <summary>
        /// Команда экспорта в xlsx
        /// </summary>
        public RelayCommand CommandXlsx
        {
            get => new RelayCommand(obj =>
            {
                //Создание команды выполнения операции
                RelayCommand done = new RelayCommand(objDone => SaveAsXlsx(""), null);

                //Создание операции
                Operation operation = new Operation("Экспорт в формате xlsx", done, null);

                NirsSystem.StackOperations.AddOperation(operation);
                operation.DoneCommand.Execute(null);
            });
        }

        private void CreateReport()
        {
            Thread.Sleep(5000);
        }

        private async void SaveAsDocx(string filename) => await Task.Run(() =>
        {
            IsDone = true;
            StatusString = "Выполняется создание отчета";
            CreateReport();
            StatusString = "Выполняется сохранения отчета";
            IsDone = false;
        });


        private void SaveAsPdf(string filename)
        {

        }

        private void SaveAsXlsx(string filename)
        {

        }

    }
}
