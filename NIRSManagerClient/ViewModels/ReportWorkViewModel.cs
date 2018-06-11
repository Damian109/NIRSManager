using System;
using NIRSCore;
using System.IO;
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
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;

namespace NIRSManagerClient.ViewModels
{
    /// <summary>
    /// Модель представления страницы отчетов
    /// </summary>
    public sealed class ReportWorkViewModel : ViewModel
    {
        //Отчет
        private ReportHelper _report;

        //Список работ
        private List<WorkHelper> _works;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="workHelpers">Список работ</param>
        /// <param name="typeOfFilter">Тип фильтра</param>
        /// <param name="search">Строка фильтра</param>
        public ReportWorkViewModel(List<WorkHelper> workHelpers, int typeOfFilter, string search) : base("Отчеты")
        {
            _works = workHelpers;
            if (search == null || search == "")
                _report = new ReportHelper(true);
            else
            {
                switch (typeOfFilter)
                {
                    case 1:
                        _report = new ReportHelper("Отчет по работе с названием: " + search);
                        break;
                    case 2:
                        _report = new ReportHelper("Отчет по руководителю: " + search);
                        break;
                    case 3:
                        _report = new ReportHelper("Отчет по автору: " + search);
                        break;
                    case 4:
                        _report = new ReportHelper("Отчет по направлению: " + search);
                        break;
                }
            }
        }

        /// <summary>
        /// Заголовок отчета
        /// </summary>
        public string Header
        {
            get => _report.Header;
            set => _report.Header = value;
        }

        /// <summary>
        /// Активно ли состояние без сортировки
        /// </summary>
        public bool IsOutSort { get; set; } = true;

        /// <summary>
        /// Активна ли сортировка по названию
        /// </summary>
        public bool IsName { get; set; } = false;

        /// <summary>
        /// Активна ли сортировка по дате публикации
        /// </summary>
        public bool IsDate { get; set; } = false;

        /// <summary>
        /// Выполняется ли какая-то операция (Видимость)
        /// </summary>
        public Visibility IsDone { get; private set; } = Visibility.Hidden;

        /// <summary>
        /// Статус выполнения операции
        /// </summary>
        public string StatusString { get; private set; }

        /// <summary>
        /// Активна ли опция выбора периодов публикации
        /// </summary>
        public bool IsDateFromDate { get; set; } = false;

        /// <summary>
        /// Дата начала отрезка
        /// </summary>
        public DateTime DateOfStart { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата окончания отрезка
        /// </summary>
        public DateTime DateOfEnd { get; set; } = DateTime.Now;

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
            _report.ReportElemHelpers.Clear();
            IsDone = Visibility.Visible;
            OnPropertyChanged("IsDone");
            StatusString = "Выполняется создание отчета";
            OnPropertyChanged("StatusString");
            ReportElemHelper helper = new ReportElemHelper(new AuthorHelper(new Author()), _works, 0);
            _report.ReportElemHelpers.Add(helper);

            //Убрать некоторые элементы из коллекции
            if(IsDateFromDate)
            {
                foreach(var elem in _report.ReportElemHelpers.First().Works)
                {
                    if (elem.DateOfPublic > DateOfEnd || elem.DateOfPublic < DateOfStart)
                        _report.ReportElemHelpers.First().Works.Remove(elem);
                }
            }

            //Начальная сортировка
            if (IsName)
            {
                List<WorkHelper> sorting = _report.ReportElemHelpers.First().Works.OrderBy(u => u.WorkName).ToList();
                _report.ReportElemHelpers.First().Works = sorting;
            }
            if (IsDate)
            {
                List<WorkHelper> sorting = _report.ReportElemHelpers.First().Works.OrderBy(u => u.DateOfPublic).ToList();
                _report.ReportElemHelpers.First().Works = sorting;
            }

            Thread.Sleep(1000);
        }

        private async void SaveAsDocx(string filename) => await Task.Run(() =>
        {
            CreateReport();
            StatusString = "Выполняется сохранение отчета";
            OnPropertyChanged("StatusString");

            using (var docx = DocX.Create(filename))
            {
                //Формирование заголовка
                docx.InsertParagraph(_report.Header).FontSize(25.0d).Bold().Alignment = Alignment.center;

                foreach (var elem in _report.ReportElemHelpers.First().Works)
                {
                    //Заполнение информации о работах
                    Paragraph workParagraph = docx.InsertParagraph();

                    workParagraph.AppendLine(elem.WorkName).Bold().FontSize(20.0d);
                    workParagraph.AppendLine(elem.Authors).FontSize(16.0d);
                    workParagraph.AppendLine(elem.HeadAuthor).FontSize(16.0d);
                    if(elem.DirectionsWork != "")
                        workParagraph.AppendLine(elem.DirectionsWork).FontSize(14.0d);
                    if (elem.JournalOrConference != "Работа не публиковалась")
                        workParagraph.AppendLine(elem.JournalOrConference + " " + elem.DateOfPublic.ToShortDateString()).FontSize(14.0d);
                }

                docx.Save();
            }
            Thread.Sleep(1000);
            IsDone = Visibility.Hidden;
            OnPropertyChanged("IsDone");
        });




        private async void SaveAsPdf(string filename) => await Task.Run(() =>
        {
            CreateReport();
            StatusString = "Выполняется сохранение отчета";
            OnPropertyChanged("StatusString");

            /*//Открываем документ
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
            document.Open();

            //Формирование шрифта для отображения русских букв
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont);
            font.Color = iTextSharp.text.BaseColor.BLACK;

            //Формирование заголовка
            iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph
            {
                Alignment = 1,
                SpacingAfter = 20.0f
            };
            font.Size = 25.0f;
            font.SetStyle("bold");
            iTextSharp.text.Phrase headerPhrase = new iTextSharp.text.Phrase(_report.Header, font);
            header.Add(headerPhrase);
            document.Add(header);

            foreach (var elem in _report.ReportElemHelpers)
            {
                //Формирование заполнения информацией об авторе
                iTextSharp.text.Paragraph authorParagraph = new iTextSharp.text.Paragraph
                {
                    Alignment = 2,
                    SpacingAfter = 20.0f,
                    Leading = 20.0f
                };

                //Заполнение ФИО и степени
                font = new iTextSharp.text.Font(baseFont, 20.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                iTextSharp.text.Chunk fio = new iTextSharp.text.Chunk(elem.Author.AuthorName, font);
                authorParagraph.Add(fio);
                font = new iTextSharp.text.Font(baseFont, 12.0f, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
                iTextSharp.text.Chunk acDegree = new iTextSharp.text.Chunk("  " + elem.Author.AcademicDegreeName + "\n", font);
                authorParagraph.Add(acDegree);

                //Заполнение Организации/Факультета/Кафедры
                font = new iTextSharp.text.Font(baseFont, 14.0f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                if (elem.Author.OrganizationName != "")
                {
                    iTextSharp.text.Chunk org = new iTextSharp.text.Chunk(elem.Author.OrganizationName + " / ", font);
                    authorParagraph.Add(org);
                }
                if (elem.Author.FacultyName != "")
                {
                    iTextSharp.text.Chunk fac = new iTextSharp.text.Chunk(elem.Author.FacultyName + " / ", font);
                    authorParagraph.Add(fac);
                }
                if (elem.Author.DepartmentName != "")
                {
                    iTextSharp.text.Chunk dep = new iTextSharp.text.Chunk(elem.Author.DepartmentName + " / ", font);
                    authorParagraph.Add(dep);
                }
                authorParagraph.Add("\n");

                //Заполнение должности или группы обучения
                font = new iTextSharp.text.Font(baseFont, 12.0f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                if (elem.Author.PositionName != "")
                {
                    iTextSharp.text.Chunk pos = new iTextSharp.text.Chunk(elem.Author.PositionName + "\n", font);
                    authorParagraph.Add(pos);
                }
                else if (elem.Author.GroupName != "")
                {
                    iTextSharp.text.Chunk gro = new iTextSharp.text.Chunk(elem.Author.GroupName + "\n", font);
                    authorParagraph.Add(gro);
                }

                //Заполнение статистики по работам
                int count = elem.Works.Count;
                font = new iTextSharp.text.Font(baseFont, 10.0f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                iTextSharp.text.Chunk cou = new iTextSharp.text.Chunk("Работ написано: " + count.ToString() + "\n", font);
                authorParagraph.Add(cou);

                if (elem.CountHeader > 0)
                {
                    iTextSharp.text.Chunk couh = new iTextSharp.text.Chunk("Является руководителем " + elem.CountHeader.ToString() + " работ" + "\n", font);
                    authorParagraph.Add(couh);
                }

                document.Add(authorParagraph);

                if (IsPrintListWorks && elem.Works.Count > 0)
                {
                    foreach (var work in elem.Works)
                    {
                        //Заполнение информации о работах
                        iTextSharp.text.Paragraph workParagraph = new iTextSharp.text.Paragraph
                        {
                            Alignment = 0,
                            SpacingAfter = 20.0f,
                            Leading = 20.0f
                        };

                        font = new iTextSharp.text.Font(baseFont, 14.0f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        iTextSharp.text.Chunk wn = new iTextSharp.text.Chunk(work.WorkName + "\n", font);
                        workParagraph.Add(wn);
                        font = new iTextSharp.text.Font(baseFont, 10.0f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        if (work.MarkWork != "")
                        {
                            iTextSharp.text.Chunk mw = new iTextSharp.text.Chunk(work.MarkWork + "\n", font);
                            workParagraph.Add(mw);
                        }
                        if (work.SizeWork != "")
                        {
                            iTextSharp.text.Chunk sw = new iTextSharp.text.Chunk(work.SizeWork + "\n", font);
                            workParagraph.Add(sw);
                        }

                        //Вывод подробной информации о работе
                        if (IsPrintFullWork)
                        {
                            iTextSharp.text.Chunk aw = new iTextSharp.text.Chunk(work.Authors + "\n", font);
                            workParagraph.Add(aw);
                            if (work.DirectionsWork != "")
                            {
                                iTextSharp.text.Chunk dw = new iTextSharp.text.Chunk(work.DirectionsWork + "\n", font);
                                workParagraph.Add(dw);
                            }
                            if (work.JournalOrConference != "")
                            {
                                iTextSharp.text.Chunk jw = new iTextSharp.text.Chunk(work.JournalOrConference + "\n", font);
                                workParagraph.Add(jw);
                            }
                        }
                        document.Add(workParagraph);
                    }
                }
            }*/

            //document.Close();
            Thread.Sleep(1000);
            IsDone = Visibility.Hidden;
            OnPropertyChanged("IsDone");
        });

        private async void SaveAsXlsx(string filename) => await Task.Run(() =>
        {
            CreateReport();
            StatusString = "Выполняется сохранение отчета";
            OnPropertyChanged("StatusString");

            /*//Открываем документ
            if (File.Exists(filename))
                File.Delete(filename);
            FileInfo file = new FileInfo(filename);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                //Формирование заголовка
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(_report.Header);

                //Формирование размеров колонок
                worksheet.Column(1).Width = 27;
                worksheet.Column(2).Width = 16;
                worksheet.Column(3).Width = 27;
                worksheet.Column(4).Width = 27;
                worksheet.Column(5).Width = 16;
                worksheet.Column(6).Width = 16;
                worksheet.Column(7).Width = 32;
                worksheet.Column(8).Width = 16;

                int rowCounter = 1;

                foreach (var elem in _report.ReportElemHelpers)
                {
                    //Формирование заполнения информацией об авторе
                    using (var range = worksheet.Cells[rowCounter, 1, rowCounter, 8])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                        range.Style.Font.Color.SetColor(Color.Black);
                    }

                    //Заполнение ФИО и степени
                    worksheet.Cells[rowCounter, 1].Value = elem.Author.AuthorName;
                    worksheet.Cells[rowCounter, 2].Value = elem.Author.AcademicDegreeName;

                    //Заполнение Организации/Факультета/Кафедры
                    if (elem.Author.OrganizationName != "")
                        worksheet.Cells[rowCounter, 3].Value = elem.Author.OrganizationName;
                    if (elem.Author.FacultyName != "")
                        worksheet.Cells[rowCounter, 4].Value = elem.Author.FacultyName;
                    if (elem.Author.DepartmentName != "")
                        worksheet.Cells[rowCounter, 5].Value = elem.Author.DepartmentName;

                    //Заполнение должности или группы обучения
                    if (elem.Author.PositionName != "")
                        worksheet.Cells[rowCounter, 6].Value = elem.Author.PositionName;
                    else if (elem.Author.GroupName != "")
                        worksheet.Cells[rowCounter, 6].Value = elem.Author.GroupName;

                    //Заполнение статистики по работам
                    int count = elem.Works.Count;
                    worksheet.Cells[rowCounter, 7].Value = "Работ написано: " + count.ToString();

                    if (elem.CountHeader > 0)
                        worksheet.Cells[rowCounter, 8].Value = "Является руководителем " + elem.CountHeader.ToString() + " работ";

                    rowCounter++;

                    if (IsPrintListWorks && elem.Works.Count > 0)
                    {
                        foreach (var work in elem.Works)
                        {
                            //Заполнение информации о работах

                            using (var range = worksheet.Cells[rowCounter, 1, rowCounter, 8])
                            {
                                range.Style.Font.Bold = false;
                                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                range.Style.Font.Color.SetColor(Color.Black);
                            }

                            worksheet.Cells[rowCounter, 1].Value = work.WorkName;
                            worksheet.Cells[rowCounter, 2].Value = work.MarkWork;
                            worksheet.Cells[rowCounter, 3].Value = work.HeadAuthor;

                            //Вывод подробной информации о работе
                            if (IsPrintFullWork)
                            {
                                worksheet.Cells[rowCounter, 4].Value = work.Authors;
                                worksheet.Cells[rowCounter, 5].Value = work.DirectionsWork;
                                worksheet.Cells[rowCounter, 6].Value = work.JournalOrConference;
                                worksheet.Cells[rowCounter, 7].Value = work.SizeWork;
                            }
                            rowCounter++;
                        }
                    }
                    rowCounter++;
                }
                package.Save();
            }
            */
            Thread.Sleep(1000);
            IsDone = Visibility.Hidden;
            OnPropertyChanged("IsDone");
        });

    }
}
