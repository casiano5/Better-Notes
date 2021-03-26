using System;
using System.Windows;
using BetterNotes;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Documents;

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
            LoadXamlPackage(openNote.FilePath + "\\note\\note");
        }
        void LoadXamlPackage(string _fileName) {
            TextRange range;
            FileStream fStream;
            if (File.Exists(_fileName)) {
                range = new TextRange(RichNote.Document.ContentStart, RichNote.Document.ContentEnd);
                fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                fStream.Close();
            }
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
            FlowDocument tempFlow = new FlowDocument();
            AddDocument(RichNote.Document, tempFlow);
            RichTextBox tempRTB = new RichTextBox(tempFlow);
            ConvertToPdf.Convert(tempRTB);
        }
        private static void AddDocument(FlowDocument from, FlowDocument to) {
            TextRange range = new TextRange(from.ContentStart, from.ContentEnd);
            MemoryStream stream = new MemoryStream();
            System.Windows.Markup.XamlWriter.Save(range, stream);
            range.Save(stream, DataFormats.XamlPackage);
            TextRange range2 = new TextRange(to.ContentEnd, to.ContentEnd);
            range2.Load(stream, DataFormats.XamlPackage);
        }

        private void NewNote(object sender, RoutedEventArgs e) {
            NewNoteDialog newNoteView = new NewNoteDialog();
            if (MessageBox.Show("Would you like to open this note in a new window?", "Open in New Window?", MessageBoxButton.YesNoCancel) == MessageBoxResult.No) this.Close();
            newNoteView.Show();
            //As part of the newnotedialog, create a new BetterNotesMainView with a new object passed based on parameters given
        }

        private void SaveCurrentNote(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = GlobalVars.DocumentDir;
            saveFileDialog.Filter = "Better Notes Note (*.bnot)|*.bnot|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = openNote.Name;
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

        private void DeleteCurrentNote(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete this note", "Delete Note?", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes) {
                openNote.DeleteNote();
                openNote = null;
                this.Close();
            }
        }
    }
}
