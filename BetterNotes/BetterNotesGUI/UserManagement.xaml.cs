using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BetterNotes;

namespace BetterNotesGUI {
    public partial class UserManagement : Window {
        public UserManagement() {
            InitializeComponent();
            Back.Background = GlobalVars.MainBack;
            FillUsers();
        }
        private void UnHighlightButton(object sender, RoutedEventArgs e) {
            (sender as Button).Background = GlobalVars.ManageUnHighLight;
        }
        private void HighlightButton(object sender, RoutedEventArgs e) {
            (sender as Button).Background = GlobalVars.ButtonUnHighLight;
        }
        public void FillUsers() {
            EnterU.Children.Clear();
            UserHandler.AddAllUsersInMetadata();
            ListBox UserN = new ListBox {
                Name = "UserN",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            ListBox UserP = new ListBox {
                Name = "UserP",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            ListBox UserE = new ListBox {
                Name = "UserE",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            ListBox UserB = new ListBox {
                Name = "UserB",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            UserN.Items.Add(new ListBoxItem {
                Background = Brushes.Beige,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = "User Name"

            });
            UserP.Items.Add(new ListBoxItem {
                Background = Brushes.Beige,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = "User Phone"

            });
            UserE.Items.Add(new ListBoxItem {
                Background = Brushes.Beige,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = "User Email"

            });
            UserB.Items.Add(new ListBoxItem {
                Background = Brushes.Beige,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = "Actions",
            });
            if (UserHandler.UserList.Count <= 0) {
                UserN.Items.Add(new TextBlock {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2, 1, 2, 1),
                    Text = "No Current User are set"

                });
                return;
            }
            for (int i = 0; i < UserHandler.UserList.Count; i++) {
                Button delete = new Button {
                    Name = "Dbutton" + i,
                    Content = "Delete",
                    Margin = new Thickness(0, 2, 20, 0),
                    FlowDirection = FlowDirection.RightToLeft
                };
                delete.Click += new RoutedEventHandler((s, e) => DeleteUserGUI(s, e));
                Button update = new Button {
                    Name = "Ubutton" + i,
                    Content = "Update",
                    
                    Margin = new Thickness(3, 2, 3, 0),
                    FlowDirection = FlowDirection.RightToLeft
                };
                update.Click += new RoutedEventHandler((s, e) => UpdateUser(s, e));
                StackPanel ButtonPanel = new StackPanel {
                    Name = "NamePanel" + i,
                    Orientation = Orientation.Horizontal
                };
                ButtonPanel.Children.Add(update);
                ButtonPanel.Children.Add(delete);
                UserB.Items.Add(ButtonPanel);
                UserN.Items.Add(new TextBlock {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(0, 5, 0, 1),
                    Text = UserHandler.UserList[i].Name

                });
                UserP.Items.Add(new TextBlock {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(0, 5, 0, 1),
                    Text = UserHandler.UserList[i].PhoneNumber

                });
                UserE.Items.Add(new TextBlock {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(0, 5, 0, 1),
                    Text = UserHandler.UserList[i].Email

                });
            }
            EnterU.Children.Add(UserN);
            EnterU.Children.Add(UserP);
            EnterU.Children.Add(UserE);
            EnterU.Children.Add(UserB);
        }
        private void DeleteUserGUI(object sender, RoutedEventArgs e) {
            int rowIndex = 0;
            Int32.TryParse((sender as Button).Name[7].ToString(), out rowIndex);
            if (MessageBox.Show("Are you sure you want to delete this User?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                UserHandler.UserList[rowIndex].DeleteUserFromMetadata();
                FillUsers();
            }
        }
        private void CreateUser(object sender, RoutedEventArgs e) {
            UserDialog uDialog = new UserDialog(this);
            uDialog.Show();
        }
        private void UpdateUser(object sender, RoutedEventArgs e) {
            int rowIndex = 0;
            Int32.TryParse((sender as Button).Name[7].ToString(), out rowIndex);
            UserDialog uDialog = new UserDialog(UserHandler.UserList[rowIndex], this);
            uDialog.Show();
        }
    }
}
