using System;
using System.Windows;
using System.Drawing;
using BetterNotes;

namespace BetterNotesGUI {
    public partial class MinimizedView : Window {
        public MinimizedView() {
            InitializeComponent();
            NotificationHandler.RefreshList();
            CreateTaskBarIcon();
            this.Hide();
        }
        private void CreateTaskBarIcon() {
            NotesReminder.notifyIcon.Icon = Properties.Resources.BetterNotes;
            NotesReminder.notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            NotesReminder.notifyIcon.ContextMenuStrip.Items.Add("Create New File").Click += (s, e) => ShowNew();
            NotesReminder.notifyIcon.ContextMenuStrip.Items.Add("Open New File").Click += (s, e) => ShowOpen();
            NotesReminder.notifyIcon.ContextMenuStrip.Items.Add("User Management").Click += (s, e) => ShowUser();
            NotesReminder.notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => EndApp();
            NotesReminder.notifyIcon.Visible = true;
        }
        private void ShowNew() {
            NewNoteDialog newView = new NewNoteDialog();
            newView.Show();
        }
        private void ShowOpen() {
            Homepage homeView = new Homepage();
            homeView.Show();
        }
        private void ShowUser() {
            UserManagement manageUserWindow = new UserManagement();
            manageUserWindow.Show();
        }
        private void EndApp() {
            CloseNotifyIcon();
            Application.Current.Shutdown();
        }
        public void CloseNotifyIcon() {
            NotesReminder.notifyIcon.Dispose();
            NotesReminder.notifyIcon = null;
        }
    }
}
