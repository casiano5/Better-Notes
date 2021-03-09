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
using BetterNotes;

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

        private void highlightButton(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonHighLight;
        }

        private void unHighlightButton(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonUnHighLight;
        }
    }
}
