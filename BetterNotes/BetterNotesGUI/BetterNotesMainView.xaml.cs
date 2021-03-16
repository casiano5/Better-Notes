using System;
using System.Windows;
using BetterNotes;
using System.IO;
using Microsoft.Win32;

//TODO: Connect the buttons
//TODO: Add an event listener, on change of richtextbox, set bool to false and prompt user if they want to save on close event.

namespace BetterNotesGUI {
    public partial class BetterNotesMainView : Window {
        Note openNote;
        

        //THIS IS A TEMPORARY CONSTRUCTOR, REMOVE ON COMPLETION
        public BetterNotesMainView() {
            InitializeComponent();
            if (!WindowExists()) _ = new MinimizedView();
        }
        //THIS IS A TEMPORARY CONSTRUCTOR, REMOVE ON COMPLETION


        public BetterNotesMainView(Note openNote) {
            this.openNote = openNote;
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

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            UserManagement manageUserWindow = new UserManagement();
            manageUserWindow.Show();
        }


        //Integration
        private void ConvertToPDF(object sender, RoutedEventArgs e) {
        //    ConvertToPdf.Convert(RichNote);
        }

        private void NewNote(object sender, RoutedEventArgs e) {

        }

        private void SaveCurrentNote(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = GlobalVars.DocumentDir;
            saveFileDialog.Filter = "Better Notes Note (*.bnot)|*.bnot|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == true) {
                openNote.SaveNote(RichNote, Path.GetFullPath(saveFileDialog.FileName));
            }
        }

        private void OpenExistingNote(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = GlobalVars.DocumentDir;
            openFileDialog.Filter = "Better Notes Note (*.bnot)|*.bnot|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            BetterNotesMainView bnotView = null;
            if (openFileDialog.ShowDialog() == true) {
                bnotView = new BetterNotesMainView(new Note(openFileDialog.FileName));
                bnotView.Show();
            }
        }
    }
}
