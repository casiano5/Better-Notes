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
using System.IO;

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

        private void MainWindowResolution_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainWindowResolution.Width <= 70) RichNote.Width = 0;
            else RichNote.Width = MainWindowResolution.Width - 70;

            if (MainWindowResolution.Height <= 130) RichNote.Height = 0;
            else RichNote.Height = MainWindowResolution.Height - 130;

            Menubar.Width = MainWindowResolution.Width;

            /*StreamWriter sw = new StreamWriter(@"C:\Users\Stickers\Stickers\[Downloads]\\text.txt");
            sw.WriteLine(MainWindowResolution.Height.ToString());
            sw.Close();*/
        }
    }
}
