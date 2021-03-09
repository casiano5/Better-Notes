using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BetterNotes {
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("68FD0036-8472-48BA-97AA-23955A4AFE96"), ComVisible(true)]
    public class BetterNotesNotifActivator : NotificationActivator {
        public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId) {
            //Implement to handle activation of toast
        }
    }
}
