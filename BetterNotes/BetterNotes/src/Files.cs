using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace BetterNotes { 
    class Files
{
        //Save button 
        private void Save_button1_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = File.Open(saveFileDialog.FileName, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(richTextBox1.Text);
                    }
                }
            }
        }

        // Select text File button 
        private void Select_File_button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.ShowDialog();
                textBox2.Text = file.FileName;
            }
            textBox3.Text = (textBox2.Text).Replace(".txt", ".pdf");
        }

        // Convert text file to PDF button Use the itextsharp library 
        private void Convert_button3_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader(textBox2.Text);
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(textBox3.Text, FileMode.Create));
            doc.Open();
            doc.Add(new Paragraph(reader.ReadToEnd()));
            doc.Close();
            System.Diagnostics.Process.Start(textBox3.Text);
        }

        // opens a text file into a rich text box
        private void Open_File_button6_Click(object sender, EventArgs e)
        {
            Stream stream;
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((stream = openFile.OpenFile()) != null)
                {
                    string fileName = openFile.FileName;
                    String file = File.ReadAllText(fileName);
                    richTextBox1.Text = file;
                }
            }

        }

    }
}