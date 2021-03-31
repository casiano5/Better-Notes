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
using System.IO;
using BetterNotes;

namespace BetterNotesGUI
{
    public partial class UserManagement : Window
    {
        public UserManagement()
        {
            InitializeComponent();
            FillUsers();
        }
        private void FillUsers()
        {
            UserHandler.AddAllUsersInMetadata();
            if (UserHandler.UserList.Count <= 0)
            {
                UserN.Items.Add(new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2, 1, 2, 1),
                    Text = "No Current User are set"

                });
                return;
            }
            for (int i = 0; i < UserHandler.UserList.Count; i++)
            {
                Button delete = new Button
                {
                    Name = "Dbutton" + i,
                    Content = "Delete",
                    Margin = new Thickness(10, 2, 3, 0)
                };
                //delete.Click += new RoutedEventHandler((s, e) => DeleteUserMetadata(s, e));
                Button update = new Button
                {
                    Name = "Ubutton" + i,
                    Content = "Update",
                    Margin = new Thickness(0, 2, 3, 0)
                };
                // update.Click += new RoutedEventHandler((s, e) => UpdateUserMetadata(s, e));
                UserN.Items.Add(new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2, 1, 2, 1),
                    Text = UserHandler.UserList[i].Name///Need Help

                });
                UserP.Items.Add(new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2, 1, 2, 1),
                    Text = UserHandler.UserList[i].PhoneNumber///Need Help

                });

                StackPanel EmailPanel = new StackPanel
                {
                    Name = "EmailPanel" + i,
                    Orientation = Orientation.Horizontal
                };
                EmailPanel.Children.Add(new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2, 1, 2, 1),
                    Text = UserHandler.UserList[i].Email///Need Help

                });
                EmailPanel.Children.Add(delete);
                EmailPanel.Children.Add(update);
                UserE.Items.Add(EmailPanel);
            }
        }
    }
}
