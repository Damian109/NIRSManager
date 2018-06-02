using System.Diagnostics;
using System.Windows.Controls;

namespace NIRSManagerClient.Views
{
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("https://github.com/Damian109");
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("https://vk.com/damian01");
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("mailto://damian25091995@gmail.com");
        }
    }
}
