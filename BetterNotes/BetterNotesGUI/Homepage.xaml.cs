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
using System.ComponentModel;

namespace BetterNotesGUI {
    public partial class Homepage : Window {
        public Homepage() {
            InitializeComponent();
            if (!WindowExists()) _ = new MinimizedView();
        }
        private bool WindowExists() {
            foreach (Window element in System.Windows.Application.Current.Windows) if (element.GetType() == typeof(MinimizedView)) return true;
            return false;
        }
        private void OpenNotes(object sender, RoutedEventArgs e) {
            BetterNotesMainView bnotView = new BetterNotesMainView();
            bnotView.Show();
            this.Close();
        }
        private void highlightButton(object sender, System.Windows.Input.MouseEventArgs e) {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonHighLight;
        }
        private void unHighlightButton(object sender, System.Windows.Input.MouseEventArgs e) {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonUnHighLight;
        }
    }
}