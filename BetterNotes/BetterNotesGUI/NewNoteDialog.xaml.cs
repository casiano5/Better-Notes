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
using System.IO;
using System.ComponentModel;

namespace BetterNotesGUI {
    public partial class NewNoteDialog : Window {
        Window parentWindow = null;
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
        public NewNoteDialog(Window parentWindow) {
            InitializeComponent();
            FillUsers();
            FillCarriers();
            this.parentWindow = parentWindow;
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
            if (PhoneNotification.IsChecked == true) {
                DontSendPhone(sender, new RoutedEventArgs());
                PhoneNotification.IsChecked = false;
            }
            if (EmailNotification.IsChecked == true) {
                DontSendEmail(sender, new RoutedEventArgs());
                EmailNotification.IsChecked = false;
            }
        }
        private void IsReminder(object sender, RoutedEventArgs e) {
            TimeToRemindBox.Visibility = Visibility.Visible;
            ReminderTypeBox.Visibility = Visibility.Visible;
        }
        private void SendEmail(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Add(EmailRemindBox);
        }
        private void SendPhone(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Add(CarrierBox);
            ParentPanel.Children.Add(PhoneRemindBox);
        }
        private void DontSendEmail(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Remove(EmailRemindBox);
        }
        private void DontSendPhone(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Remove(CarrierBox);
            ParentPanel.Children.Remove(PhoneRemindBox);
        }
        private void FillUserInPhoneEmail(object sender, RoutedEventArgs e) {
            foreach (User user in UserHandler.UserList) {
                if (UserComboBox.SelectedValue.Equals(user.Name)) {
                    EmailToSend.Text = user.Email;
                    PhoneToSend.Text = user.PhoneNumber.Substring(3);
                    if (user.PhoneNumber.Contains("VZW")) CarrierToSend.SelectedItem = "Verizon";
                    if (user.PhoneNumber.Contains("ATT")) CarrierToSend.SelectedItem = "AT&T";
                    if (user.PhoneNumber.Contains("TMO")) CarrierToSend.SelectedItem = "T-Mobile";
                }
            }
        }
        private void CreateNote(object sender, RoutedEventArgs e) {
            if (!ErrorCheckNoteCreate()) return;
            User noteUser = null;
            BetterNotesMainView bnotView = null;
            string userSelected = (string)UserComboBox.SelectedValue;
            foreach (User user in UserHandler.UserList) if (user.Name.Equals(userSelected)) noteUser = user;
            if (isNote.IsChecked == true) bnotView = new BetterNotesMainView(new Note(noteName.Text, noteUser));
            else if (isReminder.IsChecked == true) {
                if (!ErrorCheckReminderCreate()) return;
                string phoneToRemind = "";
                string emailToRemind = "";
                //TODO: Error check phonenumbers


                DateTime tryTimeToRemind = DateTime.Now;
                DateTime.TryParse(TimeToRemind.Text + ":00", out tryTimeToRemind);
                if (DateTime.Now >= tryTimeToRemind) {
                    System.Windows.MessageBox.Show("Please select a time in the future for time to remind", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (PhoneNotification.IsChecked == true) {
                    if (CarrierToSend.SelectedValue.Equals("AT&T")) phoneToRemind = "ATT";
                    if (CarrierToSend.SelectedValue.Equals("T-Mobile")) phoneToRemind = "TMO";
                    if (CarrierToSend.SelectedValue.Equals("Verizon")) phoneToRemind = "VZW";
                    phoneToRemind += PhoneToSend.Text;
                    if (!NotesReminder.IsValidPhoneNumber(phoneToRemind)) {
                        System.Windows.MessageBox.Show("Phone number is not valid, please enter only numbers", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                if (PhoneNotification.IsChecked == true) {
                    emailToRemind = EmailToSend.Text;
                    if (!NotesReminder.IsValidEmail(emailToRemind)) {
                        System.Windows.MessageBox.Show("Email is not valid, please use the following format \n example@domain.com", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                bnotView = new BetterNotesMainView(new Note(noteName.Text, noteUser, tryTimeToRemind, (bool)ToastNotification.IsChecked, emailToRemind, phoneToRemind));
            }
            if (parentWindow != null) parentWindow.Close();
            bnotView.Show();
            this.Close();
        }
        private void CancelCreate(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private bool ErrorCheckNoteCreate() {
            if (noteName.Text.Equals("")) {
                System.Windows.MessageBox.Show("Please choose a Note Name", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Directory.Exists(GlobalVars.BnotWorkDir + "\\" + noteName.Text)) {
                System.Windows.MessageBox.Show("Note already exists (is it already open?)", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (UserComboBox.SelectedValue == null || UserComboBox.SelectedValue.Equals("")) {
                System.Windows.MessageBox.Show("Please choose a Note Author", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if ((isNote.IsChecked == true && isReminder.IsChecked == true) || (isNote.IsChecked == false && isReminder.IsChecked == false)) {
                System.Windows.MessageBox.Show("Please indicate if creating item is a note or a reminder", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private bool ErrorCheckReminderCreate() {
            if (TimeToRemind.Text == null) {
                System.Windows.MessageBox.Show("Please choose a time to remind", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (ToastNotification.IsChecked == false && PhoneNotification.IsChecked == false && EmailNotification.IsChecked == false) {
                System.Windows.MessageBox.Show("Please select at least one notification method", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (PhoneNotification.IsChecked == true && (CarrierToSend.SelectedItem == null || CarrierToSend.SelectedItem.Equals("") || PhoneToSend.Text.Equals(""))) {
                System.Windows.MessageBox.Show("Please input phone contact information", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (EmailNotification.IsChecked == true && EmailToSend.Text.Equals("")) {
                System.Windows.MessageBox.Show("Please input email contact information", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void OnCloseNew(object sender, CancelEventArgs e) {
            if (PhoneNotification.IsChecked == true) DontSendPhone(sender, new RoutedEventArgs());
            if (EmailNotification.IsChecked == true) DontSendEmail(sender, new RoutedEventArgs());
        }
    }
}
