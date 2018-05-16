using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using NIRSManagerClient.ViewModels.SettingsViewModels;

namespace NIRSManagerClient.Views
{
    public partial class ProgramSettingsView : UserControl
    {
        public ProgramSettingsView()
        {
            InitializeComponent();
            DataContext = new ProgramSettingsViewModel();
        }
    }
}
