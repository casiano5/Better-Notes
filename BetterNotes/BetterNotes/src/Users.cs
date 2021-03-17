using System;
using System.IO;

namespace BetterNotes {
    public class Users {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Users(string name, string phoneNumber, string email) {
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
        }

        public void AddUserToMetadata() {
            string userMetadata =
                this.Name + "," +
                this.PhoneNumber + "," +
                this.Email + "," +
            Environment.NewLine;
            File.AppendAllText(GlobalVars.BnotReminderCsv, userMetadata);
        }

        public void SaveUserToMetadata() {
            string userMetadata =
                this.Name + "," + 
                this.PhoneNumber + "," + 
                this.Email;
            string userCsv = "";
            using (var reader = new StreamReader(GlobalVars.BnotUsersCsv)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    if (line.Split(',')[0].Equals(this.Name)) userCsv += userMetadata + Environment.NewLine;
                    else { userCsv += line + Environment.NewLine; }
                }
            }
            File.WriteAllText(GlobalVars.BnotReminderCsv, userCsv);
        }

        public void DeleteUserFromMetadata() {
            string userCsv = "";
            using (var reader = new StreamReader(GlobalVars.BnotUsersCsv)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] temp = line.Split(',');
                    if (!line.Split(',')[0].Equals(this.Name)) userCsv += line + Environment.NewLine;
                }
            }
            File.WriteAllText(GlobalVars.BnotReminderCsv, userCsv);
        }
    }
}
