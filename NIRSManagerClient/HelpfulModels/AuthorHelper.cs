using System;
using NIRSCore;
using System.Linq;
using System.Windows;
using NIRSCore.DataBaseModels;
using NIRSManagerClient.Views;
using System.IO;

namespace NIRSManagerClient.HelpfulModels
{
    public sealed class AuthorHelper
    {
        /// <summary>
        /// Идентификатор автора
        /// </summary>
        public int AuthorId { get; private set; }

        /// <summary>
        /// ФИО Автора
        /// </summary>
        public string AuthorName { get; private set; }

        /// <summary>
        /// Путь к личному фото
        /// </summary>
        public string PhotoPath { get; private set; }

        /// <summary>
        /// Организация
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Факультет
        /// </summary>
        public string FacultyName { get; set; }

        /// <summary>
        /// Кафедра
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Ученая степень
        /// </summary>
        public string AcademicDegreeName { get; set; }

        /// <summary>
        /// Команда - Изменить автора
        /// </summary>
        public RelayCommand CommandEdit
        {
            get => new RelayCommand(obj =>
            {
                ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
                window.mainGrid.Children.Clear();
                window.mainGrid.Children.Add(new AuthorView(AuthorId));
            });
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="author">Автор</param>
        public AuthorHelper(Author author)
        {
            AuthorId = author.AuthorId;
            AuthorName = author.AuthorName;

            if (File.Exists(Environment.CurrentDirectory + author.PhotoPath))
                PhotoPath = Environment.CurrentDirectory + author.PhotoPath;
            else
                PhotoPath = Environment.CurrentDirectory + "\\data\\author.png";

            if (author.OrganizationId != null)
            {
                Organization organization = (Organization)NirsSystem.GetObject<Organization>((int)author.OrganizationId);
                OrganizationName = organization.OrganizationName;
            }
            else
                OrganizationName = "(Без организации)";

            if (author.FacultyId != null)
            {
                Faculty faculty = (Faculty)NirsSystem.GetObject<Faculty>((int)author.FacultyId);
                FacultyName = "Факультет: " + faculty.FacultyName;
            }
            else
                FacultyName = "";

            if (author.DepartmentId != null)
            {
                Department department = (Department)NirsSystem.GetObject<Department>((int)author.DepartmentId);
                DepartmentName = "Кафедра: " + department.DepartmentName;
            }
            else
                DepartmentName = "";

            if (author.GroupId != null)
            {
                Group group = (Group)NirsSystem.GetObject<Group>((int)author.GroupId);
                GroupName = "Группа: " + group.GroupName;
            }
            else
                GroupName = "";

            if (author.PositionId != null)
            {
                Position position = (Position)NirsSystem.GetObject<Position>((int)author.PositionId);
                PositionName = "Должность: " + position.PositionName;
            }
            else
                PositionName = "";

            if (author.AcademicDegreeId != null)
            {
                AcademicDegree academicDegree = (AcademicDegree)NirsSystem.GetObject<AcademicDegree>((int)author.AcademicDegreeId);
                AcademicDegreeName = academicDegree.AcademicDegreeName;
            }
            else
                AcademicDegreeName = "";
        }
    }
}