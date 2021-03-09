using System;
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
using System.Windows.Forms;

namespace Homepage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //homepage
            InitializeComponent();
            start_process();
        }
        private void start_process()
        {
            //startup process
        }

        private void OpenNotes(object sender, RoutedEventArgs e)
        {
            //open notes
            System.Windows.Forms.MessageBox.Show("Note: " + sender.ToString().Substring(32));
            
        }

        private void OpenRecent1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent1.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#50612787");
        }

        private void OpenRecent2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent2.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#50612787");
        }

        private void OpenRecent3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent3.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#50612787");
        }

        private void OpenRecent4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent4.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#50612787");
        }

        private void OpenRecent5_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent5.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#50612787");
        }

        private void OpenRecent6_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent6.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#50612787");
        }

        private void OpenRecent1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent1.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#10AAB1BB");
        }

        private void OpenRecent2_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent2.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#10AAB1BB");
        }

        private void OpenRecent3_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent3.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#10AAB1BB");
        }

        private void OpenRecent4_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent4.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#10AAB1BB");
        }

        private void OpenRecent5_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent5.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#10AAB1BB");
        }

        private void OpenRecent6_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenRecent6.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#10AAB1BB");
        }
    }
}
