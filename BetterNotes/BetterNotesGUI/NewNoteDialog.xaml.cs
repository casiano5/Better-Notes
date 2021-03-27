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
        private static TextBox EmailToSend = new TextBox {
            TextWrapping = TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            Height = Double.NaN,
            Width = Double.NaN
        };
        private static GroupBox EmailRemindBox = new GroupBox {
            Name = "EmailRemindBox",
            Header = "Email to Send Reminder",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Height = Double.NaN,
            Width = Double.NaN,
            Content = EmailToSend
        };
        private static TextBox PhoneToSend = new TextBox {
            TextWrapping = TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            Height = Double.NaN,
            Width = Double.NaN
        };
        private static GroupBox PhoneRemindBox = new GroupBox {
            Name = "PhoneRemindBox",
            Header = "Phone to Send Reminder",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Height = Double.NaN,
            Width = Double.NaN,
            Content = PhoneToSend
        };
        private static ComboBox CarrierToSend = new ComboBox {
            Name = "CarrierToSend",
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        private static GroupBox CarrierBox = new GroupBox {
            Name = "CarrierBox",
            Header = "Carrier",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Height = Double.NaN,
            Width = Double.NaN,
            Content = CarrierToSend
        };

        public NewNoteDialog() {
            InitializeComponent();
            FillUsers();
            FillCarriers();
        }

        private void FillCarriers() {
            CarrierToSend.Items.Add("AT&T");
            CarrierToSend.Items.Add("T-Mobile");
            CarrierToSend.Items.Add("Verizon");           
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
            ParentPanel.Children.Add(CarrierBox);
        }

        private void DontSendEmail(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Remove(EmailRemindBox);
        }

        private void DontSendPhone(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Remove(PhoneRemindBox);
            ParentPanel.Children.Remove(CarrierBox);
        }

        private void CreateNote(object sender, RoutedEventArgs e) {
            User noteUser = null;
            BetterNotesMainView bnotView = null;
            string userSelected = (string)UserComboBox.SelectedValue;
            foreach (User user in UserHandler.UserList) if (user.Name.Equals(userSelected)) noteUser = user;
            if (isNote.IsChecked == true) bnotView = new BetterNotesMainView(new Note(noteName.Text, noteUser));
            else if (isReminder.IsChecked == true) {
                string phoneToRemind = "";
                //TODO: Error check phonenumbers
                //TODO: At least one notification options should be selected
                //TODO: Time to remind must be in the future
                if (CarrierToSend.SelectedValue.Equals("AT&T")) phoneToRemind = "ATT";
                if (CarrierToSend.SelectedValue.Equals("T-Mobile")) phoneToRemind = "TMO";
                if (CarrierToSend.SelectedValue.Equals("Verizon")) phoneToRemind = "VZW";
                phoneToRemind += PhoneToSend.Text;
                DateTime tryTimeToRemind = DateTime.Now;
                DateTime.TryParse(TimeToRemind.Text + ":00", out tryTimeToRemind);
                bnotView = new BetterNotesMainView(new Note(noteName.Text, noteUser, tryTimeToRemind, (bool)ToastNotification.IsChecked, EmailToSend.Text, phoneToRemind));
            }
            bnotView.Show();
            this.Close();
        }

        private void CancelCreate(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
