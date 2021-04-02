using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BetterNotes;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;

namespace BetterNotesGUI {
    public partial class Homepage : Window {
        List<System.Windows.Controls.Button> RecentNotesButtons = new List<System.Windows.Controls.Button>();

        public Homepage() {
            InitializeComponent();
            if (!WindowExists()) _ = new MinimizedView();
            GenerateRecentNotes();
            this.Show();
            long fileLen = new FileInfo(GlobalVars.BnotUsersCsv).Length;
            if (fileLen == 0 || (fileLen == 3 && File.ReadAllBytes("file").SequenceEqual(new byte[] { 239, 187, 191 }))) {
                FirstLaunch();
            }
        }
        private void FirstLaunch() {
            MessageBox.Show("Welcome to Better Notes!\nPlease set up a user in the following dialog","", MessageBoxButton.OK);
            UserManagement manageUserWindow = new UserManagement();
            manageUserWindow.Show();
        }
        private bool WindowExists() {
            foreach (Window element in System.Windows.Application.Current.Windows) if (element.GetType() == typeof(MinimizedView)) return true;
            return false;
        }
        private void GenerateRecentNotes() {
            List<string[]> RecentNotesList = new List<string[]>();
            using (var reader = new StreamReader(GlobalVars.BnotRecentNoteCsv)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    RecentNotesList.Add(line.Split(','));
                }
                reader.Close();
            }
            RecentNotesList = SortDescending(RecentNotesList);
            for (int i = 0; i < RecentNotesList.Count && i < 6; i++) {
                RecentNotesButtons.Add(new System.Windows.Controls.Button() {
                    Name = "OpenRecent" + i,
                    Content = "Recent",
                    Visibility = Visibility.Visible,
                    Width = 300,
                    Height = 50,
                    Margin = new Thickness(0, 0, 0, 0)
                });
                RecentNotesButtons[i].Style = System.Windows.Application.Current.Resources["SquareButtonTemplate"] as Style;
                Grid.SetColumn(RecentNotesButtons[i], 1);
                Grid.SetRow(RecentNotesButtons[i], i + 2);
                string filePath = RecentNotesList[i][3];
                RecentNotesButtons[i].AddHandler(System.Windows.Controls.Button.MouseEnterEvent, new RoutedEventHandler(HighlightButton));
                RecentNotesButtons[i].AddHandler(System.Windows.Controls.Button.MouseLeaveEvent, new RoutedEventHandler(UnHighlightButton));
                RecentNotesButtons[i].Click += new RoutedEventHandler((s, e) => OpenNotes(s, e, filePath));
                Run nameRun = new Run(RecentNotesList[i][2] + "\n");
                Run spacingRun = new Run(".\n");
                Run pathRun = null;
                if (filePath.Length > 40) pathRun = new Run(filePath.Substring(0, 40) + "...");
                else { pathRun = new Run(filePath); }
                nameRun.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE0F500");
                pathRun.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#AAAAAAAA");
                pathRun.FontSize = 11;
                spacingRun.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#00AAAAAA");
                spacingRun.FontSize = 3;
                TextBlock nameFilePath = new TextBlock();
                nameFilePath.Inlines.Add(nameRun);
                nameFilePath.Inlines.Add(spacingRun);
                nameFilePath.Inlines.Add(pathRun);
                RecentNotesButtons[i].Content = nameFilePath;
                RecentNotesGrid.Children.Add(RecentNotesButtons[i]);
            }
        }
        private List<string[]> SortDescending(List<string[]> list) {
            list.Sort((a, b) => b[0].CompareTo(a[0]));
            return list;
        }
        private void OpenNotes(object sender, RoutedEventArgs e, string filePath) {
            BetterNotesMainView bnotView = new BetterNotesMainView(new Note(filePath));
            bnotView.Show();
            this.Close();
        }
        private void HighlightButton(object sender, RoutedEventArgs e) {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonHighLight;
        }
        private void UnHighlightButton(object sender, RoutedEventArgs e) {
            (sender as System.Windows.Controls.Button).Background = GlobalVars.ButtonUnHighLight;
        }
        private void OpenNewClick(object sender, RoutedEventArgs e) {
            NewNoteDialog newView = new NewNoteDialog(this);
            newView.Show();
        }
        private void OpenExistingClick(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = GlobalVars.DocumentDir;
            openFileDialog.Filter = "Better Notes Note (*.bnot)|*.bnot|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            BetterNotesMainView bnotView = null;
            if (openFileDialog.ShowDialog() == true) {
                bnotView = new BetterNotesMainView(new Note(openFileDialog.FileName));
                bnotView.Show();
                this.Close();
            }
        }
        private void UserManagementClick(object sender, RoutedEventArgs e) {
            UserManagement manageUserWindow = new UserManagement();
            manageUserWindow.Show();
        }
        private void NotificationClick(object sender, RoutedEventArgs e) {
            NotificationManagement notifyView = new NotificationManagement();
            notifyView.Show();
        }
    }
}