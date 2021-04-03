using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BetterNotes;

namespace BetterNotesGUI {
    public partial class NotificationManagement : Window {
        private List<List<string>> notificationList = new List<List<string>>();
        public NotificationManagement() {
           
            InitializeComponent();
            Back.Background = GlobalVars.MainBack;
            FillNotifications();
        }
        private void FillNotifications() {
            
            List<List<string>> tempNotificationList = new List<List<string>>();
            using (var reader = new StreamReader(GlobalVars.BnotReminderCsv)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    tempNotificationList.Add(line.Split(',').ToList<string>());
                }
                reader.Close();
            }
            if (tempNotificationList.Count <= 0) {
                NotificationListPanel.Children.Add(new GroupBox{
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    BorderThickness = new Thickness(0),
                    Content = new Label {
                        Content = "No notifications are set",
                        Foreground=GlobalVars.MainText,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    } 
                });
                this.notificationList = null;
                return;
            }
            for (int i = 0; i < tempNotificationList.Count; i++) {
                Button delete = new Button {
                    Name = "button" + i,
                    Content = "Delete",
                    Width = Double.NaN
                };
                delete.Click += new RoutedEventHandler((s, e) => DeleteNotificationMetadata(s, e));
                StackPanel parentStack = new StackPanel {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                StackPanel labelStack = new StackPanel {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                StackPanel contentStack = new StackPanel {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                labelStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = "When to Remind:" });
                labelStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = "Send Windows Toast Notification:" });
                labelStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = "Send Email Notification:" });
                labelStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = "Send Phone Notification:" });
                labelStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = "Delete:" });
                contentStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = tempNotificationList[i][0] });
                contentStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = tempNotificationList[i][4][0].ToString().ToLower().Equals("t") ? "Yes" : "No" } );
                contentStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = tempNotificationList[i][3].Equals("") ? "No" : tempNotificationList[i][3] });
                contentStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = tempNotificationList[i][2].Equals("") ? "No" : tempNotificationList[i][2] });
                contentStack.Children.Add(new Label { Foreground = GlobalVars.MainText, Content = delete });
                parentStack.Children.Add(labelStack);
                parentStack.Children.Add(contentStack);
                GroupBox groupBox = new GroupBox {
                    Header = tempNotificationList[i][1],
                    //BorderBrush = GlobalVars.ButtonUnHighLight,
                    Foreground= GlobalVars.MainText,
                    BorderThickness = new Thickness(0.2),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = Double.NaN,
                    Width = Double.NaN,
                    Content = parentStack
                };
                NotificationListPanel.Children.Add(groupBox);
                this.notificationList = tempNotificationList;
            }
        }
        private void DeleteNotificationMetadata(object sender, RoutedEventArgs e) {
            int rowIndex = 0;
            Int32.TryParse((sender as Button).Name[6].ToString(), out rowIndex);
            if (MessageBox.Show("Are you sure you want to delete this reminder?\n\nWARNING: If the note containing this notification is opened, it will re-add the notification", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                string remindCsv = "";
                using (var reader = new StreamReader(GlobalVars.BnotReminderCsv)) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (!line.Split(',')[1].Equals(notificationList[rowIndex][1])) remindCsv += line + Environment.NewLine;
                    }
                    reader.Close();
                }
                File.WriteAllText(GlobalVars.BnotReminderCsv, remindCsv);
                NotificationListPanel.Children.Clear();
                FillNotifications();
            }
        }
    }
}
