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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using BetterNotes;

namespace BetterNotesGUI {
    public partial class NewNoteDialog : Window {
        private static readonly TextBox EmailToSend = new TextBox {
            TextWrapping = TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            Height = Double.NaN,
            Width = Double.NaN
        };
        private static readonly GroupBox EmailRemindBox = new GroupBox {
            Name = "EmailRemindBox",
            Header = "Email to Send Reminder",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Height = Double.NaN,
            Width = Double.NaN,
            Content = EmailToSend
        };
        private static readonly TextBox PhoneToSend = new TextBox {
            TextWrapping = TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            Height = Double.NaN,
            Width = Double.NaN
        };
        private static readonly GroupBox PhoneRemindBox = new GroupBox {
            Name = "PhoneRemindBox",
            Header = "Phone to Send Reminder",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Height = Double.NaN,
            Width = Double.NaN,
            Content = PhoneToSend
        };

        public NewNoteDialog() {
            InitializeComponent();
            FillUsers();
        }

        private void FillUsers() {
            UserHandler.AddAllUsersInMetadata();
            foreach (User user in UserHandler.UserList) UserComboBox.Items.Add(user.Name);
        }

        private void IsNote(object sender, RoutedEventArgs e) {
            TimeToRemindBox.Visibility = Visibility.Hidden;
            ReminderTypeBox.Visibility = Visibility.Hidden;
        }

        private void IsReminder(object sender, RoutedEventArgs e) {
            TimeToRemindBox.Visibility = Visibility.Visible;
            ReminderTypeBox.Visibility = Visibility.Visible;
        }

        private void SendEmail(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Add(EmailRemindBox);
        }

        private void SendPhone(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Add(PhoneRemindBox);
        }

        private void DontSendEmail(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Remove(EmailRemindBox);
        }

        private void DontSendPhone(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Remove(PhoneRemindBox);
        }

        private void CreateNote(object sender, RoutedEventArgs e) {
            User noteUser = null;
            BetterNotesMainView bnotView = null;
            string userSelected = (string)UserComboBox.SelectedValue;
            foreach (User user in UserHandler.UserList) if (user.Name.Equals(userSelected)) noteUser = user;
            if (isNote.IsChecked == true) bnotView = new BetterNotesMainView(new Note(noteName.Text, noteUser));
            else if (isReminder.IsChecked == true) {
                DateTime tryTimeToRemind = DateTime.Now;
                DateTime.TryParse(TimeToRemind.Text + ":00" , out tryTimeToRemind);
                bnotView = new BetterNotesMainView(new Note(noteName.Text, noteUser, tryTimeToRemind, (bool)ToastNotification.IsChecked, EmailToSend.Text, PhoneToSend.Text));
            }
            bnotView.Show();
            this.Close();
        }

        private void CancelCreate(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
