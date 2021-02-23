using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterNotes {
    public class Reminder : Note {
        public DateTime SendReminderDateTime { get; set; }

        public Reminder(string workDir, string name, string createUser) : base(workDir + "\\" + name, name, createUser) {
            //just creates a note object, theres only one difference between a note and a reminder, one of them can alert and the other can not
        }

        //make a note object (existing file)
        public Reminder(string currentNoteDirectory) : base(currentNoteDirectory){
            //TODO: Read field "whenToRemind" in metadata and set sendReminderDateTime to this value
        }
        //TODO: Make destructor for Reminder class
    }
}
