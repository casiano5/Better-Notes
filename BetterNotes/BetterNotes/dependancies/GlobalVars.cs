using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BetterNotes {
    public static class GlobalVars {
        //Environment Variables
        public readonly static string AppdataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public readonly static string UserDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public readonly static string DocumentDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public readonly static string ImageDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public readonly static string BnotTempDir = AppdataDir + "\\BetterNotes";

        //Button highlighting
        public readonly static SolidColorBrush ButtonHighLight = (SolidColorBrush) new BrushConverter().ConvertFromString("#50612787");
        public readonly static SolidColorBrush ButtonUnHighLight = (SolidColorBrush) new BrushConverter().ConvertFromString("#10AAB1BB");
    }
}
