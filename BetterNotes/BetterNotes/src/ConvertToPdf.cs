using System;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BetterNotes {
    class ConvertToPdf {
        public static void convert(RichTextBox noteContent) {
            PrintDialog pd = new PrintDialog();
            pd.PrintQueue = new PrintQueue(new PrintServer(), "Microsoft Print to PDF");
            pd.PrintDocument((((IDocumentPaginatorSource)noteContent.Document).DocumentPaginator), "Save to PDF");
        }
    }
}
