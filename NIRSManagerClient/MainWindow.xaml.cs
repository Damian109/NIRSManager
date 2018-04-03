﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NIRSCore.FileOperations;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AuthorizationViewModel();

            
            FileUsers file = new FileUsers();
            file.Open();
            file.AddNewUsersItem(new FileUsersItem() { Login = "1", Md5 = "3134221343232" });
            file.Save();
        }
    }
}
