using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace BetterNotes {
    public static class NotificationHandler {
        private static List<TimerHandler> timerList = new List<TimerHandler>();
        public static void RefreshList() {
            using (var reader = new StreamReader(GlobalVars.BnotReminderCsv)) {
                List<TimerHandler> tempTimerList = new List<TimerHandler>();
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] temp = line.Split(',');
                    DateTime.TryParse(temp[0], out DateTime tryDateTime);
                    Boolean.TryParse(temp[4], out bool trySendToast);
                    if (tryDateTime > DateTime.Now) tempTimerList.Add(new TimerHandler(temp[1], tryDateTime, temp[2], temp[3], trySendToast));
                }
                timerList = tempTimerList;
                reader.Close();
            }
            SetTimers();
        }
        public static void SetTimers() {
            foreach (TimerHandler element in timerList) {
                element.Start();
            }
        }       
    }
    class TimerHandler {
        private string name;
        private string phoneContact;
        private string emailContact;
        private bool sendToast;
        private string reminderBody;
        private DateTime timeToRemind;
        private Timer timer;

        public TimerHandler(string name, DateTime timeToRemind, string phoneContact, string emailContact, bool sendToast) {
            this.name = name;
            this.timeToRemind = timeToRemind;
            this.phoneContact = phoneContact;
            this.emailContact = emailContact;
            this.sendToast = sendToast;
            this.reminderBody = "Reminder for your note: " + name;
        }
        public void Start() {
            timer = new Timer((int)(timeToRemind - DateTime.Now).TotalMilliseconds);
            timer.Interval = (int)(timeToRemind - DateTime.Now).TotalMilliseconds;
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(SendNotification);
        }
        public void SendNotification(object sender, ElapsedEventArgs e) {
            if (!(emailContact.Equals("null") || emailContact.Equals("") || emailContact == null)) NotesReminder.SendPhoneEmailNotification(emailContact, this.name, this.reminderBody);
            if (!(phoneContact.Equals("null") || phoneContact.Equals("") || phoneContact == null)) NotesReminder.SendPhoneEmailNotification(phoneContact, this.name, this.reminderBody);
            if (sendToast) NotesReminder.SendWindowsToastNotification(this.name, this.reminderBody);
            DeleteReminderMetadata();
            NotificationHandler.RefreshList();
        }
        public void DeleteReminderMetadata() {
            string remindCsv = "";
            using (var reader = new StreamReader(GlobalVars.BnotReminderCsv)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    if (!line.Split(',')[1].Equals(this.name)) remindCsv += line + Environment.NewLine;
                }
                reader.Close();
            }
            File.WriteAllText(GlobalVars.BnotReminderCsv, remindCsv);
        }
    }
}
