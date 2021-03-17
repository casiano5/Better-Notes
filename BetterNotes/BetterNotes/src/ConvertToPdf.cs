using System;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BetterNotes {
    public class ConvertToPdf {
        public static void Convert(RichTextBox noteContent) {
            PrintDialog pd = new PrintDialog();
            pd.PrintQueue = new PrintQueue(new PrintServer(), "Microsoft Print to PDF");
            pd.PrintDocument((((IDocumentPaginatorSource)noteContent.Document).DocumentPaginator), "Save to PDF");
        }
    }
}
