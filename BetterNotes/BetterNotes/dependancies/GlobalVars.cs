using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BetterNotes
{
    public static class GlobalVars
    {
        //Button highlighting
        public static SolidColorBrush ButtonHighLight = (SolidColorBrush) new BrushConverter().ConvertFromString("#50612787");
        public static SolidColorBrush ButtonUnHighLight = (SolidColorBrush) new BrushConverter().ConvertFromString("#10AAB1BB");
    }
}
