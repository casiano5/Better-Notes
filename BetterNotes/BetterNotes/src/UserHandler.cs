using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterNotes {
    public static class UserHandler {
        private static List<User> userList;
        public static List<User> UserList {
            get {
                AddAllUsersInMetadata();
                return userList;
            }
            set {userList = value;} 
        }
        public static void AddAllUsersInMetadata() {
            List<User> userListTemp = new List<User>();
            using (var reader = new StreamReader(GlobalVars.BnotUsersCsv)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] temp = line.Split(',');
                    userListTemp.Add(new User(temp[0], temp[1], temp[2]));
                }
                reader.Close();
            }
            UserList = userListTemp;
        }
    }
}
