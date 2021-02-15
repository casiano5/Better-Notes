/*
 SECURITY 

 THIS FILE IS NOT TO BE STORED IN PRODUCTION VERSIONS 
 This file is stricly for testing purposed, smtp credentials
 should not be stored here.

 SECURITY
*/

using System;
using System.Net;
using System.Net.Mail;

namespace BetterNotes {
    public class EmailSMTPService {
        public static int sendMail(MailMessage message) {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com") {
                Port = 587,
                Credentials = new NetworkCredential("betternotesproject@gmail.com", ""), //password to be filled in for gmail smtp account. ONLY FOR TESTING PURPOSES.
                EnableSsl = true
            };
            smtpClient.Send(message);
            return 0;
        }
    }
}
