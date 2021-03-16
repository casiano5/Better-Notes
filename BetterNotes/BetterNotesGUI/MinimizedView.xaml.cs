using System;
using System.Windows;
using System.Drawing;
using BetterNotes;

namespace BetterNotesGUI {
    public partial class MinimizedView : Window {
        public MinimizedView() {
            InitializeComponent();
            CreateTaskBarIcon();
            this.Hide();
        }

        private void CreateTaskBarIcon() {
            NotesReminder.notifyIcon.Icon = SystemIcons.Application;
            NotesReminder.notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            NotesReminder.notifyIcon.ContextMenuStrip.Items.Add("Open New File").Click += (s, e) => ShowOpen();
            NotesReminder.notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => EndApp();
            NotesReminder.notifyIcon.Visible = true;
        }

        private void ShowOpen() {
            Homepage homeView = new Homepage();
            homeView.Show();
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
