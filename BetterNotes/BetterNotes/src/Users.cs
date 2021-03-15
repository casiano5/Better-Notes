using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterNotes.src {
    public class Users {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Users(string name, string phoneNumber, string email) {
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
        }

        public Users() {

        }
    }
}
