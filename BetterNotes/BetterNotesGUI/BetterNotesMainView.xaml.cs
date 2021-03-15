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
    public partial class BetterNotesMainView : Window {
        private Homepage parentWindow;
        public BetterNotesMainView() {
            this.parentWindow = new Homepage();
            InitializeComponent();
        }
        public BetterNotesMainView(Homepage parentWindow) {
            this.parentWindow = parentWindow;
            InitializeComponent();
        }
        private void ExitToOpen(object sender, RoutedEventArgs e) {
            parentWindow.Show();
            this.Close();
        }
        // Add a button to a form and set some of its common properties.
        public virtual System.Windows.Forms.AnchorStyles Anchor { get; set; }
        public object Controls { get; private set; }

        private void unHighlightButton(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonUnHighLight;
        }
        private void testWTN(object sender, RoutedEventArgs e) {
            NotesReminder.SendWindowsToastNotification("Test Notification");
        }
        private void testPN(object sender, RoutedEventArgs e) {
            NotesReminder.SendPhoneEmailNotification("", "Test Reminder", "Test Reminder Body");
        }
        private void testEM(object sender, RoutedEventArgs e) {
            NotesReminder.SendPhoneEmailNotification("", "Test Reminder", "Test Reminder Body");
        }
    }
}
