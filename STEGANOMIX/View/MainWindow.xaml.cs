using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STEGANOMIX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Toggle_btn.IsChecked == true)
            {
                tt_ycbcr.Visibility = Visibility.Collapsed;
                tt_polish.Visibility = Visibility.Collapsed;
                tt_whites.Visibility = Visibility.Collapsed;
                tt_homografy.Visibility = Visibility.Collapsed;
                tt_emptycolors.Visibility = Visibility.Collapsed;
                tt_spojniki1.Visibility = Visibility.Collapsed;
                tt_spojniki2.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_ycbcr.Visibility = Visibility.Visible;
                tt_polish.Visibility = Visibility.Visible;
                tt_whites.Visibility = Visibility.Visible;
                tt_homografy.Visibility = Visibility.Visible;
                tt_emptycolors.Visibility = Visibility.Visible;
                tt_spojniki1.Visibility = Visibility.Visible;
                tt_spojniki2.Visibility = Visibility.Visible;
            }
        }

        private void Toggle_btn_Unchecked(object sender, RoutedEventArgs e)
        {
            BG.Opacity = 1;
        }

        private void Toggle_btn_Checked(object sender, RoutedEventArgs e)
        {
            BG.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Toggle_btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}