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
using BetterNotes;

namespace BetterNotesGUI {
    public partial class UserDialog : Window {
        private bool newUser;
        private UserManagement parentWindow;
        public UserDialog(UserManagement parentWindow) {
            InitializeComponent();
            FillCarriers();
            this.parentWindow = parentWindow;
            newUser = true;
        }
        public UserDialog(User user, UserManagement parentWindow) {
            InitializeComponent();
            FillCarriers();
            this.parentWindow = parentWindow;
            newUser = false;
            NameBox.Text = user.Name;
            EmailBox.Text = user.Email;
            if (user.PhoneNumber.Contains("VZW")) CarrierBox.SelectedItem = "Verizon";
            if (user.PhoneNumber.Contains("ATT")) CarrierBox.SelectedItem = "AT&T";
            if (user.PhoneNumber.Contains("TMO")) CarrierBox.SelectedItem = "T-Mobile";
            PhoneBox.Text = user.PhoneNumber.Substring(3);
        } 
        private void FillCarriers() {
            CarrierBox.Items.Add("AT&T");
            CarrierBox.Items.Add("T-Mobile");
            CarrierBox.Items.Add("Verizon");
        }
        private void SaveUser(object sender, RoutedEventArgs e) {
            if (NameBox.Text.Equals("")) {
                MessageBox.Show("Please enter a name", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (EmailBox.Text.Equals("")) {
                MessageBox.Show("Please enter an email address", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!NotesReminder.IsValidEmail(EmailBox.Text)) {
                MessageBox.Show("Email is not valid", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (PhoneBox.Text.Equals("")) {
                MessageBox.Show("Please enter a phone number", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CarrierBox.SelectedItem == null || CarrierBox.SelectedItem.Equals("")) {
                MessageBox.Show("Please select a carrier", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!NotesReminder.IsValidPhoneNumber(PhoneBox.Text)) {
                MessageBox.Show("Phone number is not valid, please enter only numbers", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string phoneNumber = "";
            if (CarrierBox.SelectedValue.Equals("AT&T")) phoneNumber = "ATT";
            if (CarrierBox.SelectedValue.Equals("T-Mobile")) phoneNumber = "TMO";
            if (CarrierBox.SelectedValue.Equals("Verizon")) phoneNumber = "VZW";
            phoneNumber += PhoneBox.Text;
            User tempUser = new User(NameBox.Text, phoneNumber, EmailBox.Text);
            if (newUser) tempUser.AddUserToMetadata();
            else { tempUser.SaveUserToMetadata(); }
            parentWindow.FillUsers();
            this.Close();
        }
        private void CancelUser(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
