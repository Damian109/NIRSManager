using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using NIRSManagerClient.ViewModels;

namespace NIRSManagerClient
{
    /// <summary>
    /// Логика взаимодействия для ExtensionView.xaml
    /// </summary>
    public partial class ExtensionView : Window
    {
        public ExtensionView(bool status)
        {
            InitializeComponent();
            ExtensionViewModel viewModel = new ExtensionViewModel(status);
            DataContext = viewModel;
            Closing += viewModel.OnWindowClosing;
        }

        /// <summary>
        /// Происходит в промежутке между нажатием и отпусканием мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DemoItemsListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is Expander)
                    return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            MenuToggleButton.IsChecked = false;
        }
    }
}
