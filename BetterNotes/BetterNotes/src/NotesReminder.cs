using System;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Data.Xml.Dom;

namespace BetterNotes {
    public class NotesReminder {
        void sendWindowsToastNotification(string toastTitleContent) {

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
    }
}