using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public class TR
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public TR(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }
    }

    /// <summary>
    /// Логика взаимодействия для BackupView.xaml
    /// </summary>
    public partial class BackupView : UserControl
    {
        public BackupView()
        {
            InitializeComponent();

            Backups = new List<TR>
            {
                new TR("backup-13-04-2018", DateTime.Now),
                new TR("backup-13-04-2018", DateTime.Now),
                new TR("backup-13-04-2018", DateTime.Now),
                new TR("backup-13-04-2018", DateTime.Now),
            };

            t.ItemsSource = Backups;
        }

        public List<TR> Backups { get; set; }
    }
}
