using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BetterNotes {
    public class Note {
        //private vars and get/sets
        public string FilePath { get; set; } //set only in case you want to move the and change property while open (renaming notes, temp directories, etc)
        public string Name { get; set; } 
        public string CreateUser { get; set; } 
        public DateTime CreatedDateTime { get; } //RONLY value, should not change after first creation
        public DateTime LastModifiedDateTime { get; set; }
        public bool IsReminder { get; set; }
        public DateTime TimeToRemind { get; set; }
        public bool RemindToast { get; set; }
        public string RemindPhone { get; set; }
        public string RemindEmail { get; set; }

        //make a note object (new file)
        public Note(string name, string createUser) {
            this.FilePath = GlobalVars.BnotWorkDir + "\\" + name;
            this.Name = name;
            this.CreateUser = createUser;
            this.CreatedDateTime = DateTime.Now;
            this.LastModifiedDateTime = DateTime.Now;
        }

        //make a reminder object
        public Note(string name, string createUser, DateTime timeToRemind, bool remindToast, string remindEmail, string remindPhone) {
            this.FilePath = GlobalVars.BnotWorkDir + "\\" + name;
            this.Name = name;
            this.CreateUser = createUser;
            this.CreatedDateTime = DateTime.Now;
            this.LastModifiedDateTime = DateTime.Now;
            this.IsReminder = true;
            this.TimeToRemind = timeToRemind;
            this.RemindToast = remindToast;
            this.RemindPhone = remindPhone;
            this.RemindEmail = remindEmail;
        }

        //make a note/reminder object (existing file)
        public Note(string currentBnotDir) {
            Regex testRegex = new Regex(@"([^\\]*[.]*\.bnot)$", RegexOptions.Multiline);
            string nameAndExt = testRegex.Match(currentBnotDir).Value;
            this.Name = nameAndExt.Substring(0, nameAndExt.Length - 5);
            this.FilePath = GlobalVars.BnotWorkDir + "\\" + this.Name;
            Archive.UnarchiveFile(currentBnotDir, this.FilePath);
            List<string> csvIn = new List<string>();
            using (var reader = new StreamReader(this.FilePath + "\\NoteMetaData.properties")) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    foreach (string element in line.Split(',')) csvIn.Add(element);
                }
            }
            this.CreateUser = csvIn[1];
            DateTime tryCreateDate = DateTime.Now;
            DateTime.TryParse(csvIn[2], out tryCreateDate);
            this.CreatedDateTime = tryCreateDate;
            this.LastModifiedDateTime = DateTime.Now;
            bool tryIsReminder = false;
            Boolean.TryParse(csvIn[4], out tryIsReminder);
            this.IsReminder = tryIsReminder;
            DateTime tryRemindDate = DateTime.Now;
            DateTime.TryParse(csvIn[5], out tryRemindDate);
            this.TimeToRemind = tryRemindDate;
            bool tryRemindToast = false;
            Boolean.TryParse(csvIn[6], out tryRemindToast);
            this.IsReminder = tryRemindToast;
            this.RemindPhone = csvIn[7];
            this.RemindEmail = csvIn[8];
        }

        public void SaveNote(RichTextBox noteContent, string savePath) {
            SaveCurrentNoteMetadata();
            if (this.IsReminder) SaveCurrentNoteReminderMetadata();
            SaveRichTexBox(noteContent);
            Archive.ArchiveFile(this.FilePath, savePath);
        }

        public void SaveCurrentNoteMetadata() {
            string noteMetadata =
                this.Name + "," +
                this.CreateUser + "," +
                this.CreatedDateTime.ToString("yyyy-MM-dd") + "," +
                DateTime.Now.ToString("yyyy-MM-dd") + "," +
                this.IsReminder.ToString() + "," +
                this.TimeToRemind.ToString("yyyy-MM-dd") + "," +
                this.RemindToast.ToString() + "," +
                this.RemindPhone + "," +
                this.RemindEmail;
            File.WriteAllText(this.FilePath + "\\NoteMetadata.properties", noteMetadata);
        }

        public void SaveCurrentNoteReminderMetadata() {
            string remindMetadata =
                this.TimeToRemind.ToString("yyyy-MM-dd") + "," +
                this.Name;
            string remindCsv = "";
            using (var reader = new StreamReader(GlobalVars.BnotReminderCsv)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    if (line.Contains("," + this.Name)) remindCsv += remindMetadata + Environment.NewLine;
                    else { remindCsv += line + Environment.NewLine; }
                }
            }
            File.WriteAllText(GlobalVars.BnotReminderCsv, remindCsv);
        }

        public void SaveRichTexBox(RichTextBox noteContent) {
            TextRange range;
            FileStream fStream;
            range = new TextRange(noteContent.Document.ContentStart, noteContent.Document.ContentEnd);
            fStream = new FileStream(this.FilePath + "\\note\\note", FileMode.Create);
            range.Save(fStream, DataFormats.XamlPackage);
            fStream.Close();
        }
    }
}
