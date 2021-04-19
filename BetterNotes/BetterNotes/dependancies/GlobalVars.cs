using System;
using System.Windows.Media;

namespace BetterNotes {
    public static class GlobalVars {
        //Environment Variables
        public readonly static string AppdataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public readonly static string UserDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public readonly static string DocumentDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public readonly static string ImageDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public readonly static string BnotWorkDir = AppdataDir + "\\BetterNotes";
        public readonly static string BnotReminderCsv = BnotWorkDir + "\\ReminderListMetadata.properties";
        public readonly static string BnotRecentNoteCsv = BnotWorkDir + "\\RecentNoteMetaData.properties";
        public readonly static string BnotUsersCsv = BnotWorkDir + "\\Users.properties";

        //Button highlighting
        public readonly static SolidColorBrush ButtonHighLight = (SolidColorBrush)new BrushConverter().ConvertFromString("#50612787");
        public readonly static SolidColorBrush ButtonUnHighLight = (SolidColorBrush) new BrushConverter().ConvertFromString("#10AAB1BB");
        public readonly static SolidColorBrush ManageUnHighLight = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF696969"); //colored button unhigliging
        public readonly static SolidColorBrush MainBack = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF45474A");//Main Background color
        public readonly static SolidColorBrush MainText = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF7F9CA");//Main Text color
        public readonly static SolidColorBrush MainPBack = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF393939");//Main Panel Background color(remind panel, recent note panel, Resource Panel)
    }
}
