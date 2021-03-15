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
using System.Windows.Forms;
using System.IO.Compression;

namespace BetterNotes { 
    class Files
{
        //Save button 
        public static void SaveFile(string fileName, string filePath)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //Showdialog() shows the form as a dialogbox
            //call ArchiveFile(fileName, filePath) to archive the folder
            if (saveFileDialog.ShowDialog() == DialogResult.OK)// if the savefile is determined that the dialogResult is OK 
            {
                if (!File.Exists(fileName))
                {

                    using (Stream stream = File.Open(saveFileDialog.FileName, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fileName))
                        {
                            //TODO: Link this to a UI to recieve information
                            
                        }
                    }
                }
                else if (File.Exists(fileName))
                {
                    
                }
            }

        }
        // Select text File button 
        private void Select_File_button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.ShowDialog();
                //textBox2.Text = file.FileName;
            }
           //textBox3.Text = (textBox2.Text).Replace(".txt", ".pdf");
        }

        // Convert text file to PDF button Use the itextsharp library 
        private void Convert_button3_Click(object sender, EventArgs e)
        {
            //StreamReader reader = new StreamReader(textBox2.Text);
            //Document doc = new Document();
            //PdfWriter.GetInstance(doc, new FileStream(textBox3.Text, FileMode.Create));
            //doc.Open();
            //doc.Add(new Paragraph(reader.ReadToEnd()));
            //doc.Close();
            //System.Diagnostics.Process.Start(textBox3.Text);
        }

        public static void OpenProgram(string archivepath, String dir)
        {
            //using (ZipArchive archive = new ZipArchive(File.OpenRead(archivepath), ZipArchiveMode.Read))
            {
               // foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    //UnarchiveFile(archivepath, dir);
                }
                //TODO: Create a Note object that returns on file open
                //TODO: Create a Reminder object similar to the noteObject 
            }

        }
        // When the note is being closed it will show a message that will ask user if they want to save before they close the notes.
        //TODO: Check for changes in the note
        private static void CloseFile(string fileName, string filePath)
        {

            string directory = System.IO.Directory.GetCurrentDirectory();

            DialogResult dialog = MessageBox.Show(" Do you want to save before closing", "BetterNotes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (dialog == DialogResult.Yes)
            {
                SaveFile(fileName, filePath);
                System.IO.Directory.GetParent(directory);

            }
            else if (dialog == DialogResult.No)
            {
                System.IO.Directory.GetParent(directory);

            }
        }
        private static void Documentrecovery()
        {


        }

    }
}