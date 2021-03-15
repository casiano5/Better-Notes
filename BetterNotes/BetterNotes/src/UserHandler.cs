using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterNotes {
    public static class UserHandler {
        public static List<Users> AddAllUsersInMetadata() {
            List<Users> userList = new List<Users>();
            using (var reader = new StreamReader(GlobalVars.BnotUsersCsv)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] temp = line.Split(',');
                    userList.Add(new Users(temp[0], temp[1], temp[2]));
                }
            }
            return userList;
        }
    }
}
