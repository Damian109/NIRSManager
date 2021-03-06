﻿using System;
using NIRSCore;
using System.Linq;
using System.Windows;
using NIRSCore.DataBaseModels;
using NIRSManagerClient.Views;
using System.Collections.Generic;

namespace NIRSManagerClient.HelpfulModels
{
    public sealed class WorkHelper
    {
        /// <summary>
        /// Идентификатор работы
        /// </summary>
        public int WorkId { get; private set; }

        /// <summary>
        /// Название работы
        /// </summary>
        public string WorkName { get; private set; }

        /// <summary>
        /// Информация о руководителе
        /// </summary>
        public string HeadAuthor { get; private set; }

        /// <summary>
        /// Авторы работы
        /// </summary>
        public string Authors { get; private set; }

        /// <summary>
        /// Путь к фото
        /// </summary>
        public string PhotoPath { get; private set; }

        /// <summary>
        /// Направления работы
        /// </summary>
        public string DirectionsWork { get; private set; }

        /// <summary>
        /// Размер работы
        /// </summary>
        public string SizeWork { get; private set; }

        /// <summary>
        /// Оценка работы
        /// </summary>
        public string MarkWork { get; private set; }

        /// <summary>
        /// Журнал или конференция публикации
        /// </summary>
        public string JournalOrConference { get; private set; }

        public DateTime DateOfPublic { get; private set; }

        /// <summary>
        /// Команда - Изменить работу
        /// </summary>
        public RelayCommand CommandEdit
        {
            get => new RelayCommand(obj =>
            {
                ExtensionView window = Application.Current.Windows.OfType<ExtensionView>().FirstOrDefault();
                window.mainGrid.Children.Clear();
                window.mainGrid.Children.Add(new WorkView(WorkId));
            });
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="work">Работа</param>
        public WorkHelper(Work work)
        {
            WorkId = work.WorkId;
            WorkName = work.WorkName;
            PhotoPath = Environment.CurrentDirectory + "\\data\\work.png";
            DateOfPublic = DateTime.MinValue;

            if (work.HeadAuthorId != null)
            {
                Author author = (Author)NirsSystem.GetObject<Author>((int)work.HeadAuthorId);
                if(author != null)
                    HeadAuthor = "Руководитель: " + author.AuthorName;
            }
            else
                HeadAuthor = "(Руководитель отсутствует)";

            SizeWork = "Размер работы в печатных листах: " + work.WorkSize.ToString();

            MarkWork = "Оценка работы:" + work.WorkMark;

            if (work.ConferenceId != null)
            {
                Conference conference = (Conference)NirsSystem.GetObject<Conference>((int)work.ConferenceId);
                JournalOrConference = "Работа была представлена на конференции: " + conference.ConferenceName;
                if (conference.ConferenceDate != DateTime.MinValue && conference.ConferenceDate != null)
                    DateOfPublic = conference.ConferenceDate;
            }
            else if (work.JournalId != null)
            {
                Journal journal = (Journal)NirsSystem.GetObject<Journal>((int)work.JournalId);
                JournalOrConference = "Работа опубликована в журнале: " + journal.JournalName;
                if (journal.JournalDate != DateTime.MinValue && journal.JournalDate != null)
                    DateOfPublic = journal.JournalDate;
            }
            else
                JournalOrConference = "Работа не публиковалась";

            Authors = "Авторы: ";
            List<CoAuthor> coAuthors = (List<CoAuthor>)NirsSystem.GetListObject<CoAuthor>();
            if(coAuthors != null)
            {
                foreach (var elem in coAuthors)
                    if (elem.WorkId == work.WorkId)
                    {
                        Author author = (Author)NirsSystem.GetObject<Author>(elem.AuthorId);
                        if(author != null)
                            Authors += author.AuthorName + ";";
                    }
            }

            DirectionsWork = "";
            List<DirectionWork> directionWorks = (List<DirectionWork>)NirsSystem.GetListObject<DirectionWork>();
            if(directionWorks != null)
            {
                if(directionWorks.Count(u => u.WorkId == WorkId) > 0)
                {
                    DirectionsWork = "Направления работы: ";

                    foreach (var elem in directionWorks)
                        if (elem.WorkId == work.WorkId)
                        {
                            Direction direction = (Direction)NirsSystem.GetObject<Direction>(elem.DirectionId);
                            DirectionsWork += direction.DirectionName + ";";
                        }
                }
            }
        }
    }
}