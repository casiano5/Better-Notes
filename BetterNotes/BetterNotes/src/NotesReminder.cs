using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace BetterNotes {
    public static class NotesReminder {
        public static System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();

        //Windows Toast Notification
        public static void SendWindowsToastNotification(string title, string content) {
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = content;
            notifyIcon.ShowBalloonTip(10000);
        }

        //Email notification
        public static void SendPhoneEmailNotification(string contactInformation, string reminderTitle, string reminderBody) {
            if (IsValidEmail(contactInformation)) {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("betternotes@casiano.dev"),
                    Subject = "Your Reminder for: " + reminderTitle,
                    Body = "<h1>Reminder: " + reminderTitle + "</h1>"
                            + "<p>" + reminderBody + "</p>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(contactInformation);
                EmailSMTPService.sendMail(mailMessage);
            }

            if (IsValidPhoneNumber(contactInformation)) {
                string email = ConvertPhoneToEmail(contactInformation);
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("betternotes@casiano.dev"),
                    Subject = "Your Reminder for: " + reminderTitle,
                    Body = "<h1>Reminder: " + reminderTitle + "</h1>"
                           + "<p>" + reminderBody + "</p>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);
                EmailSMTPService.sendMail(mailMessage);
            }
        }

        //Check for email func
        public static bool IsValidEmail(string email) {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match) {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e) {
                return false;
            }
            catch (ArgumentException e) {
                return false;
            }
            try {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException) {
                return false;
            }
        }

        //Check for phone func
        public static bool IsValidPhoneNumber(string number) {
            return int.TryParse(number.Substring(3), out _);

        }
        //append email for carrier based on original string with character (eg: VZW8314786726 becomes 8314786726@vtext.com)
        public static string ConvertPhoneToEmail(string phoneNumber) {
            string rtrn = "";
            if (phoneNumber.Contains("VZW")) rtrn = phoneNumber.Substring(3) + "@vtext.com";
            if (phoneNumber.Contains("ATT")) rtrn = phoneNumber.Substring(3) + "@txt.att.net";
            if (phoneNumber.Contains("TMO")) rtrn = phoneNumber.Substring(3) + "@tmomail.net";
            if (IsValidEmail(rtrn)) return rtrn;
            return "betternotesfailedphonetoemail@casiano.dev";
        }
    }
}