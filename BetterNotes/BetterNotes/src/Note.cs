using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;


//TODO CREATE STATIC FINAL ENV VAR FOR WORKING DIRECTORY OF BETTERNOTES AND STORE IT SOMEWHERE


namespace BetterNotes {
    public class Note {
        //private vars and get/sets
        public string FilePath { get; set; } //set only in case you want to move the and change property while open (renaming notes, temp directories, etc)
        public string Name { get; set; } 
        public string CreateUser { get; set; } 
        public DateTime CreatedDateTime { get; } //RONLY value, should not change after first creation
        public DateTime LastModifiedDateTime { get; set; }
        public bool isReminder { get; set; }
        public DateTime timeToRemind { get; set; }

        private string note; //PLACEHOLDER VALUE 

        //make a note object (new file)
        public Note(string workDir, string name, string createUser) { 
            this.FilePath = workDir + "\\" + name;
            this.Name = name;
            this.CreateUser = createUser;
            this.CreatedDateTime = DateTime.Now;
            this.LastModifiedDateTime = DateTime.Now;
        }

        //make a reminder object
        public Note(string workDir, string name, string createUser, DateTime timeToRemind) {
            this.FilePath = workDir + "\\" + name;
            this.Name = name;
            this.CreateUser = createUser;
            this.CreatedDateTime = DateTime.Now;
            this.LastModifiedDateTime = DateTime.Now;
            this.isReminder = true;
            this.timeToRemind = timeToRemind;
        }

        //make a note/reminder object (existing file)
        public Note(string currentNoteDirectory) {
            //TODO make constructor for note based on existing note file
            //constructor will expect a directory 
            //modify date time modified, read other info from possible metadata file
            //TODO design metadata file
        }

        //TODO make a close note function that calls a object save/delete function based on user input
        //TODO make a save function that calls a static save function
        //TODO make a delete function that calls a static function to delete a note file
        //TODO make a destructor for note object after delete or save
        //TODO make a function that saves note metadata to BetterNotes metadata files foe easy access (recent notes)

    }

}
