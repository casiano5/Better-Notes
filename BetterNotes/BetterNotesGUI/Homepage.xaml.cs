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

namespace BetterNotesGUI {
    /// <summary>
    /// Interaction logic for Homepage.xaml
    /// </summary>
    public partial class Homepage : Window {
        BetterNotesMainView parentWindow;
        public Homepage() {
            InitializeComponent();
            start_process();
        }
        public Homepage(BetterNotesMainView parentWindow) {
            this.parentWindow = parentWindow;
            InitializeComponent();
            start_process();
        }
        private void start_process() {
            //startup process
        }
        private void OpenNotes(object sender, RoutedEventArgs e) {
            BetterNotesMainView bnotView = new BetterNotesMainView(this);
            bnotView.Show();
            this.Hide();
        }
        private void highlightButton(object sender, System.Windows.Input.MouseEventArgs e) {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonHighLight;
        }
        private void unHighlightButton(object sender, System.Windows.Input.MouseEventArgs e) {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonUnHighLight;
        }
    }
}