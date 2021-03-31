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
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Window
    {
        private List<List<string>> UserList = new List<List<string>>();
        public UserManagement()
        {
            InitializeComponent();
            FillUsers();
        }
        private void FillUsers()
        {
            List<List<string>> tempUserList = new List<List<string>>();
            using (var reader = new System.IO.StreamReader(GlobalVars.BnotUsersCsv))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    tempUserList.Add(line.Split(',').ToList<string>());
                }
                reader.Close();
            }
            if (tempUserList.Count <= 0)
            {
                EnterU.Children.Add(new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2,1,2,1),
                    Text = "No Current User are set"
                    
                });
                this.UserList = null;
                return;
            }
            for (int i = 0; i < tempUserList.Count; i++)
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
                    Text = tempUserList[i][0]///Need Help

                });
                UserP.Items.Add(new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2, 1, 2, 1),
                    Text = tempUserList[i][0]///Need Help

                });
                
                StackPanel EmailPanel=new StackPanel
                {
                    Name = "EmailPanel" + i,
                    Orientation = Orientation.Horizontal                   
                };
                EmailPanel.Children.Add(new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(2, 1, 2, 1),
                    Text = tempUserList[i][2]///Need Help

                });
                EmailPanel.Children.Add(delete);
                EmailPanel.Children.Add(update);
                UserE.Items.Add(EmailPanel);
                
            }
        }
}
