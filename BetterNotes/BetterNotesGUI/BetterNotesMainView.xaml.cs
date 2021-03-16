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
using System.ComponentModel;

//TODO: Connect the buttons
//TODO: Add an event listener, on change of richtextbox, set bool to false and prompt user if they want to save on close event.

namespace BetterNotesGUI {
    public partial class BetterNotesMainView : Window {
        Note openNote;
        public BetterNotesMainView() {
            InitializeComponent();
            if (!WindowExists()) _ = new MinimizedView();
        }
        private bool WindowExists() {
            foreach (Window element in System.Windows.Application.Current.Windows) if (element.GetType() == typeof(MinimizedView)) return true;
            return false;
        }
        private void ExitToOpen(object sender, RoutedEventArgs e) {
            Homepage homepageView = new Homepage();
            homepageView.Show();
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
            NotesReminder.SendWindowsToastNotification("Test Notification", "Here is some content");
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
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            UserManagement manageUserWindow = new UserManagement();
            manageUserWindow.Show();
        }

        //Integration
        private void ConvertToPDF(object sender, RoutedEventArgs e) {
        //    ConvertToPdf.Convert(RichNote);
        }
    }
}
