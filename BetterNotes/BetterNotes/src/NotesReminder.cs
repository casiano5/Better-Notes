using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net.Mail;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Data.Xml.Dom;

namespace BetterNotes {
    public class NotesReminder {
        //Windows Toast Notification
        public static void sendWindowsToastNotification(string toastTitleContent) {

            ToastContent toastContent = new ToastContent() {
                Launch = "Action=viewConversation&conversationId=5",
                Visual = new ToastVisual() {
                    BindingGeneric = new ToastBindingGeneric() {
                        Children = {
                            new AdaptiveText() {
                                Text = toastTitleContent
                            }
                        }
                    }
                }
            };
            /*Option2
            ToastContent toastContent = new ToastContentBuilder().AddToastActivationInfo("action=viewConversation&conversationId=5", ToastActivationType.Foreground).AddText("Hello world!").GetToastContent();
            */
            /*Create XML Doc*/
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(toastContent.GetContent());

            /*Populate toast based on XML Doc*/
            var toast = new ToastNotification(xmlDoc);
            //Option2 var toast = new ToastNotification(toastContent.GetXml());

            /*Send Notification*/
            DesktopNotificationManagerCompat.CreateToastNotifier().Show(toast);

        }

        //Email notification
        public static void sendPhoneEmailNotification(string contactInformation, string reminderTitle, string reminderBody) {
            if (isValidEmail(contactInformation)) {
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

            if (isValidPhoneNumber(contactInformation)) {
                string email = convertPhoneToEmail(contactInformation);
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
        public static bool isValidEmail(string email) {
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
        public static bool isValidPhoneNumber(string number) {
            int hold = 0;
            return int.TryParse(number.Substring(4), out hold);

        }
        //append email for carrier based on original string with character (eg: VZW8314786726 becomes 8314786726@vtext.com)
        public static string convertPhoneToEmail(string phoneNumber) {
            string rtrn = "";
            if (phoneNumber.Contains("VZW")) rtrn = phoneNumber.Substring(4) + "@vtext.com";
            if (phoneNumber.Contains("ATT")) rtrn = phoneNumber.Substring(4) + "@txt.att.net";
            if (phoneNumber.Contains("TMO")) rtrn = phoneNumber.Substring(4) + "@tmomail.net";
            if (isValidEmail(rtrn)) return rtrn;
            return "betternotesfailedphonetoemail@casiano.dev";
        }
    }
}