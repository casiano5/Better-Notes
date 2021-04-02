using System;
using System.Collections.Generic;
using System.Windows;
using BetterNotes;
using System.IO;
using System.Windows.Documents;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using NAudio.Wave;
using DataFormats = System.Windows.DataFormats;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using RichTextBox = System.Windows.Controls.RichTextBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

//TODO: Connect the buttons

namespace BetterNotesGUI {
    public partial class BetterNotesMainView : Window {
        Note openNote;
        private List<System.Drawing.Image> imageList;
        private List<string> imageLinks;
        private int imageIndex;
        private bool saved = false;
        private WMPLib.WindowsMediaPlayer ttsPlayer;
        private WaveInEvent waveIn;
        private WaveFileWriter RecordedAudioWriter;
        public virtual System.Windows.Forms.AnchorStyles Anchor { get; set; }
        public object Controls { get; private set; }
        public BetterNotesMainView(Note openNote) {
            this.openNote = openNote;
            InitializeComponent();
            if (!WindowExists()) _ = new MinimizedView();
            LoadXamlPackage(openNote.FilePath + "\\note\\note");
        }
        private void LoadXamlPackage(string _fileName) {
            TextRange range;
            FileStream fStream;
            if (File.Exists(_fileName)) {
                range = new TextRange(RichNote.Document.ContentStart, RichNote.Document.ContentEnd);
                fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                fStream.Close();
            }
        }
        private bool WindowExists() {
            foreach (Window element in System.Windows.Application.Current.Windows) if (element.GetType() == typeof(MinimizedView)) return true;
            return false;
        }
        private void ExitToOpen(object sender, RoutedEventArgs e) {
            Homepage homepageView = new Homepage();
            homepageView.Show();
            this.Close();
        }
                
        private void testWTN(object sender, RoutedEventArgs e) {
            NotesReminder.SendWindowsToastNotification("Test Notification", "Here is some content");
        }
        private void testPN(object sender, RoutedEventArgs e) {
            NotesReminder.SendPhoneEmailNotification("", "Test Reminder", "Test Reminder Body");
        }
        private void testEM(object sender, RoutedEventArgs e) {
            NotesReminder.SendPhoneEmailNotification("", "Test Reminder", "Test Reminder Body");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            UserManagement manageUserWindow = new UserManagement();
            manageUserWindow.Show();
        }

        //Error Check
        private void TextChange(object sender, RoutedEventArgs e) {
            this.saved = false;
        }

        private void OnCloseNote(object sender, CancelEventArgs e) {
            if (!this.saved) {
                var msgResult = MessageBox.Show("Do you want to save the current note changes?", "Save Current Note", MessageBoxButton.YesNoCancel);
                if (msgResult == MessageBoxResult.Yes) SaveCurrentNote(sender, new RoutedEventArgs());
                if (msgResult == MessageBoxResult.Cancel) {
                    e.Cancel = true;
                    return;
                }
            }
            if(openNote != null) if (Directory.Exists(openNote.FilePath)) Directory.Delete(openNote.FilePath, true);
        }

        //Integration
        private void ConvertToPDF(object sender, RoutedEventArgs e) {
            FlowDocument tempFlow = new FlowDocument();
            AddDocument(RichNote.Document, tempFlow);
            RichTextBox tempRTB = new RichTextBox(tempFlow);
            ConvertToPdf.Convert(tempRTB);
        }
        private static void AddDocument(FlowDocument from, FlowDocument to) {
            TextRange range = new TextRange(from.ContentStart, from.ContentEnd);
            MemoryStream stream = new MemoryStream();
            System.Windows.Markup.XamlWriter.Save(range, stream);
            range.Save(stream, DataFormats.XamlPackage);
            TextRange range2 = new TextRange(to.ContentEnd, to.ContentEnd);
            range2.Load(stream, DataFormats.XamlPackage);
        }
        private void NewNote(object sender, RoutedEventArgs e) {
            NewNoteDialog newNoteView = null;
            var msgResult = MessageBox.Show("Would you like to open this note in a new window?", "Open in New Window?", MessageBoxButton.YesNoCancel);
            if (msgResult == MessageBoxResult.No) {
                newNoteView = new NewNoteDialog(this);
                newNoteView.Show();
            }
            else if (msgResult == MessageBoxResult.Yes) {
                newNoteView = new NewNoteDialog();
                newNoteView.Show();
            }
        }
        private void SaveCurrentNote(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = GlobalVars.DocumentDir;
            saveFileDialog.Filter = "Better Notes Note (*.bnot)|*.bnot|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = openNote.Name;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == true) {
                openNote.SaveNote(RichNote, Path.GetFullPath(saveFileDialog.FileName));
                this.saved = true;
            }
        }
        private void OpenExistingNote(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = GlobalVars.DocumentDir;
            openFileDialog.Filter = "Better Notes Note (*.bnot)|*.bnot|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            BetterNotesMainView bnotView = null;
            var msgReturn = MessageBox.Show("Would you like to open this note in a new window?", "Open in New Window?", MessageBoxButton.YesNoCancel);
            if (msgReturn == MessageBoxResult.No) this.Close();
            if (msgReturn == MessageBoxResult.Cancel) return;
            if (openFileDialog.ShowDialog() == true) {
                bnotView = new BetterNotesMainView(new Note(openFileDialog.FileName));
                bnotView.Show();
            }
        }
        private void DeleteCurrentNote(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete this note", "Delete Note?", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes) {
                this.saved = true;
                openNote.DeleteNote();
                openNote = null;
                this.Close();
            }
        }

        private void SearchVideoClick(object sender, RoutedEventArgs e) {
            VideoInsert.GetVideosFromSearchTerm(VideoSearchBox.Text);
        }

        private void SearchImageClick(object sender, RoutedEventArgs e) {
            imageLinks = ImageInsert.GetImagesFromSearchTerm(ImageSearchBox.Text);
            imageList = new List<Image>();
            foreach (string link in imageLinks) {
                try {
                    System.Net.HttpWebRequest webRequest = System.Net.HttpWebRequest.Create(link) as System.Net.HttpWebRequest;
                    webRequest.AllowWriteStreamBuffering = true;
                    webRequest.Timeout = 30000;
                    System.Net.WebResponse webResponse = webRequest.GetResponse();
                    System.IO.Stream stream = webResponse.GetResponseStream();
                    imageList.Add(System.Drawing.Image.FromStream(stream));
                    webResponse.Close();
                }
                catch (Exception ex) {
                    MessageBox.Show("Failed to get images, please try again", "Image Get Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //TODO:add the next and previous buttons here
            imageIndex = 0;
            PlaceImages();
        }

        private void PlaceImages() {
            ImageInsertPrevious.IsEnabled = (imageIndex != 0);
            ImageInsertNext.IsEnabled = !(imageIndex + 3 > imageLinks.Count);
            InsertImagePanel.Children.Clear();
            for (int i = imageIndex; i < imageIndex + 3 && i < imageLinks.Count && i >= 0; i++) {
                Button imageButton = new Button {
                    Content = new System.Windows.Controls.Image {
                        Source = new BitmapImage(new Uri(imageLinks[i]))
                    },
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5)
                };
                int index = i;
                imageButton.Click += new RoutedEventHandler((s, e) => InsertImageToRTB(s, e, index));
                InsertImagePanel.Children.Add(imageButton);
            }
        }

        private void PlaceImagesPlus(object sender, RoutedEventArgs e) {
            imageIndex += 3;
            PlaceImages();
        }

        private void PlaceImagesMinus(object sender, RoutedEventArgs e) {
            imageIndex -= 3;
            PlaceImages();
        }

        private void InsertImageToRTB(object sender, RoutedEventArgs e, int index) {
            throw new NotImplementedException();
        }

        private void GenerateWavFile(object sender, RoutedEventArgs e) {
            TextToSpeech.PutSpeechInFile(TextTrans.Text, openNote.FilePath + "\\speech\\TTS.wav");
            ttsPlayer = new WMPLib.WindowsMediaPlayer();
            ttsPlayer.URL = openNote.FilePath + "\\speech\\TTS.wav";
            ttsPlayer.controls.stop();
            PlayText.Visibility = Visibility.Visible;
            StopPlay.Visibility = Visibility.Visible;
        }

        private void PlayTts(object sender, RoutedEventArgs e) {
            ttsPlayer.controls.play();
        }

        private void StopTts(object sender, RoutedEventArgs e) {
            ttsPlayer.controls.stop();
            ttsPlayer.close();
        }

        private void PauseTts(object sender, RoutedEventArgs e) {
            ttsPlayer.controls.pause();
        }

        private void StartRecordStt(object sender, RoutedEventArgs e) {
            if (File.Exists(openNote.FilePath + "\\speech\\STT.wav")) File.Delete(openNote.FilePath + "\\speech\\STT.wav");
            waveIn = new WaveInEvent();
            RecordedAudioWriter = new WaveFileWriter(openNote.FilePath + "\\speech\\STT.wav", waveIn.WaveFormat);
            waveIn.StartRecording();
            waveIn.DataAvailable += (s, a) => {
                RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };
            waveIn.RecordingStopped += (s, a) => {
                //RecordedAudioWriter?.Dispose();
                //RecordedAudioWriter = null;
                //waveIn.Dispose();
            };
        }

        private void StopRecordStt(object sender, RoutedEventArgs e) {
            waveIn.StopRecording();
            RecordedAudioWriter?.Dispose();
            RecordedAudioWriter = null;
            waveIn.Dispose();
            TransText.Text = SpeechToText.SpeechToTextFromFile(openNote.FilePath + "\\speech\\STT.wav");
        }
    }
}
