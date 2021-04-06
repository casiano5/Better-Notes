using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
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
using System.Timers;
using Timer = System.Timers.Timer;

//TODO: Connect the buttons

namespace BetterNotesGUI {
    public partial class BetterNotesMainView : Window {
        Note openNote;
        private bool saved = false;
        public virtual System.Windows.Forms.AnchorStyles Anchor { get; set; }
        public object Controls { get; private set; }

        public BetterNotesMainView(Note openNote) {
            this.openNote = openNote;
            InitializeComponent();
            StopRecord.IsEnabled = false;
            Back.Background = GlobalVars.MainBack;
            ReminderStack.Background = GlobalVars.MainPBack;
            InsertMedia.Background = GlobalVars.MainPBack;
            InsertMedia.BorderBrush = GlobalVars.MainPBack;
            if (!WindowExists()) _ = new MinimizedView();
            LoadXamlPackage(openNote.FilePath + "\\note\\note");
            InitializeReminderElements();
            if (openNote.IsReminder) FillReminderInfo();
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
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            UserManagement manageUserWindow = new UserManagement();
            manageUserWindow.Show();
        }
        private void UnHighlightButton(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = GlobalVars.ManageUnHighLight;
        }
        private void UnHighlightButtonStop(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = Brushes.Red;
        }
        private void HighlightButton(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = GlobalVars.ButtonUnHighLight;
        }
        private void HighlightButtonD(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = GlobalVars.ButtonHighLight;
        }
        private void UnHighlightButtonD(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = GlobalVars.ButtonUnHighLight;
        }
        private void RemindPanelShowHide(object sender, RoutedEventArgs e) {
            if (ReminderGrid.ColumnDefinitions[0].ActualWidth == 0) {
                double actualSize = bnotGrid.ColumnDefinitions[0].ActualWidth - ReminderGrid.ColumnDefinitions[1].ActualWidth;
                Timer timer = new Timer(1);
                timer.Interval = 1;
                timer.Enabled = true;
                timer.Elapsed += (s, a) => ShowRemindPanel(s, a, actualSize);
            }
            else {
                Timer timer = new Timer(1);
                timer.Interval = 1;
                timer.Enabled = true;
                timer.Elapsed += new ElapsedEventHandler(HideRemindPanel);
            }
        }
        private void HideRemindPanel(object sender, ElapsedEventArgs e) {
            if ((int) ReminderGrid.ColumnDefinitions[0].ActualWidth > 9) {
                Dispatcher.Invoke(() => {
                    ReminderGrid.ColumnDefinitions[0].Width = new GridLength((int) ReminderGrid.ColumnDefinitions[0].ActualWidth - 10);
                });
            }
            else {
                (sender as Timer).Enabled = false;
                Dispatcher.Invoke(() => {
                    ReminderGrid.ColumnDefinitions[0].Width = new GridLength(0);
                });
            }
        }
        private void ShowRemindPanel(object sender, ElapsedEventArgs e, double fullSize) {
            if ((int)ReminderGrid.ColumnDefinitions[0].ActualWidth < fullSize - 9) {
                Dispatcher.Invoke(() => {
                    ReminderGrid.ColumnDefinitions[0].Width = new GridLength((int)ReminderGrid.ColumnDefinitions[0].ActualWidth + 10);
                });
            }
            else {
                (sender as Timer).Enabled = false;
                Dispatcher.Invoke(() => {
                    ReminderGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                });
            }
        }
        private void ResourcePanelShowHide(object sender, RoutedEventArgs e) {
            if (ResourceGrid.ColumnDefinitions[2].ActualWidth == 0) {
                double actualSize = bnotGrid.ColumnDefinitions[2].ActualWidth - ResourceGrid.ColumnDefinitions[1].ActualWidth;
                Timer timer = new Timer(1);
                timer.Interval = 1;
                timer.Enabled = true;
                timer.Elapsed += (s, a) => ShowResourcePanel(s, a, actualSize);
            }
            else {
                Timer timer = new Timer(1);
                timer.Interval = 10;
                timer.Enabled = true;
                timer.Elapsed += new ElapsedEventHandler(HideResourcePanel);
            }
        }
        private void HideResourcePanel(object sender, ElapsedEventArgs e) {
            if ((int)ResourceGrid.ColumnDefinitions[2].ActualWidth > 9) {
                Dispatcher.Invoke(() => {
                    ResourceGrid.ColumnDefinitions[2].Width = new GridLength((int)ResourceGrid.ColumnDefinitions[2].ActualWidth - 10);
                    ResourceGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                });
            }
            else {
                (sender as Timer).Enabled = false;
                Dispatcher.Invoke(() => {
                    ResourceGrid.ColumnDefinitions[2].Width = new GridLength(0);
                    ResourceGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                });
            }
        }
        private void ShowResourcePanel(object sender, ElapsedEventArgs e, double fullSize) {
            if ((int)ResourceGrid.ColumnDefinitions[2].ActualWidth < fullSize - 9) {
                Dispatcher.Invoke(() => {
                    ResourceGrid.ColumnDefinitions[2].Width = new GridLength(ResourceGrid.ColumnDefinitions[2].ActualWidth + 10);
                });
            }
            else {
                (sender as Timer).Enabled = false;
                Dispatcher.Invoke(() => {
                    ResourceGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                    ResourceGrid.ColumnDefinitions[0].Width = new GridLength(0);
                });
            }
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

        //Convert To PDF
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

        //Note operations
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
                if (SetReminder.IsChecked == true) SaveReminderInformation();
                else {
                    Note tempOpenNote = new Note(openNote.Name, openNote.CreateUser);
                    openNote = tempOpenNote;
                }
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

        //Reminder Operations
        private TextBox EmailToSend;
        private GroupBox EmailRemindBox;
        private TextBox PhoneToSend;
        private GroupBox PhoneRemindBox;
        private ComboBox CarrierToSend;
        private GroupBox CarrierBox;

        private void FillReminderInfo() {
            SetReminder.IsChecked = openNote.IsReminder;
            UserComboBox.SelectedItem = openNote.CreateUser.Name;
            TimeToRemind.Text = openNote.TimeToRemind.ToString("yyyy-MM-dd HH:mm");
            if (openNote.RemindToast) ToastNotification.IsChecked = true;
            if (openNote.RemindEmail != null && !openNote.RemindEmail.Equals("") && !openNote.RemindEmail.Equals("null")) {
                EmailNotification.IsChecked = true;
                EmailToSend.Text = openNote.RemindEmail;
            }
            if (openNote.RemindPhone != null && !openNote.RemindPhone.Equals("") && !openNote.RemindPhone.Equals("null")) {
                PhoneNotification.IsChecked = true;
                if (openNote.RemindPhone.Contains("VZW")) CarrierToSend.SelectedItem = "Verizon";
                if (openNote.RemindPhone.Contains("ATT")) CarrierToSend.SelectedItem = "AT&T";
                if (openNote.RemindPhone.Contains("TMO")) CarrierToSend.SelectedItem = "T-Mobile";
                PhoneToSend.Text = openNote.RemindPhone.Substring(3);
            }
        }
        private void InitializeReminderElements() {
            EmailToSend = new TextBox {
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = Double.NaN,
                Width = Double.NaN,
            };
            EmailRemindBox = new GroupBox {
                Name = "EmailRemindBox",
                Header = "Email",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Height = Double.NaN,
                Width = Double.NaN,
                Content = EmailToSend,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0,10,0,10),
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF7F9CA")
            };
            PhoneToSend = new TextBox {
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = Double.NaN,
                Width = Double.NaN,
            };
            PhoneRemindBox = new GroupBox {
                Name = "PhoneRemindBox",
                Header = "Phone",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10),
                Height = Double.NaN,
                Width = Double.NaN,
                Content = PhoneToSend,
                BorderThickness = new Thickness(0),
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF7F9CA")
            };
            CarrierToSend = new ComboBox {
                Name = "CarrierToSend",
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            CarrierBox = new GroupBox {
                Name = "CarrierBox",
                Header = "Carrier",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10),
                Height = Double.NaN,
                Width = Double.NaN,
                BorderThickness = new Thickness(0),
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFF7F9CA")
            };
            FillCarriers();
            FillUsers();
        }
        private void FillCarriers() {
            CarrierToSend.Items.Clear();
            CarrierToSend.Items.Add("AT&T");
            CarrierToSend.Items.Add("T-Mobile");
            CarrierToSend.Items.Add("Verizon");
            CarrierBox.Content = CarrierToSend;
        }
        private void FillUsers() {
            UserHandler.AddAllUsersInMetadata();
            UserComboBox.Items.Clear();
            foreach (User user in UserHandler.UserList) UserComboBox.Items.Add(user.Name);
        }
        private void SendEmail(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Add(EmailRemindBox);
        }
        private void SendPhone(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Add(CarrierBox);
            ParentPanel.Children.Add(PhoneRemindBox);
        }
        private void DontSendEmail(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Remove(EmailRemindBox);
        }
        private void DontSendPhone(object sender, RoutedEventArgs e) {
            ParentPanel.Children.Remove(CarrierBox);
            ParentPanel.Children.Remove(PhoneRemindBox);
        }
        private void FillUserInPhoneEmail(object sender, RoutedEventArgs e) {
            foreach (User user in UserHandler.UserList) {
                if (UserComboBox.SelectedValue.Equals(user.Name)) {
                    EmailToSend.Text = user.Email;
                    PhoneToSend.Text = user.PhoneNumber.Substring(3);
                    if (user.PhoneNumber.Contains("VZW")) CarrierToSend.SelectedItem = "Verizon";
                    if (user.PhoneNumber.Contains("ATT")) CarrierToSend.SelectedItem = "AT&T";
                    if (user.PhoneNumber.Contains("TMO")) CarrierToSend.SelectedItem = "T-Mobile";
                }
            }
        }
        private void SaveReminderInformation() {
            if (!ErrorCheckReminderCreate()) return;
            string phoneToRemind = "";
            string emailToRemind = "";
            DateTime tryTimeToRemind = DateTime.Now;
            DateTime.TryParse(TimeToRemind.Text + ":00", out tryTimeToRemind);
            if (DateTime.Now >= tryTimeToRemind) {
                System.Windows.MessageBox.Show("Please select a time in the future for time to remind", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (PhoneNotification.IsChecked == true) {
                if (CarrierToSend.SelectedValue.Equals("AT&T")) phoneToRemind = "ATT";
                if (CarrierToSend.SelectedValue.Equals("T-Mobile")) phoneToRemind = "TMO";
                if (CarrierToSend.SelectedValue.Equals("Verizon")) phoneToRemind = "VZW";
                phoneToRemind += PhoneToSend.Text;
                if (!NotesReminder.IsValidPhoneNumber(phoneToRemind)) {
                    System.Windows.MessageBox.Show("Phone number is not valid, please enter only numbers", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (PhoneNotification.IsChecked == true) {
                emailToRemind = EmailToSend.Text;
                if (!NotesReminder.IsValidEmail(emailToRemind)) {
                    System.Windows.MessageBox.Show("Email is not valid, please use the following format \n example@domain.com", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            Note tempOpenNote = new Note(openNote.Name, openNote.CreateUser, tryTimeToRemind, (bool)ToastNotification.IsChecked, emailToRemind, phoneToRemind);
            openNote = tempOpenNote;
        }
        private bool ErrorCheckReminderCreate() {
            if (TimeToRemind.Text == null) {
                System.Windows.MessageBox.Show("Please choose a time to remind", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (ToastNotification.IsChecked == false && PhoneNotification.IsChecked == false && EmailNotification.IsChecked == false) {
                System.Windows.MessageBox.Show("Please select at least one notification method", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (PhoneNotification.IsChecked == true && (CarrierToSend.SelectedItem == null || CarrierToSend.SelectedItem.Equals("") || PhoneToSend.Text.Equals(""))) {
                System.Windows.MessageBox.Show("Please input phone contact information", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (EmailNotification.IsChecked == true && EmailToSend.Text.Equals("")) {
                System.Windows.MessageBox.Show("Please input email contact information", "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        //ImageInsert
        private List<System.Drawing.Image> imageList;
        private List<string> imageLinks;
        private int imageIndex;

        private void ContextMenuImage(object sender, RoutedEventArgs e) {
            if (RichNote.Selection != null && !RichNote.Selection.IsEmpty) {
                ImageSearchBox.Text = RichNote.Selection.Text;
                Dispatcher.BeginInvoke((Action)(() => InsertMedia.SelectedIndex = 1));
                SearchImageClick(sender, new RoutedEventArgs());
            }
            else {MessageBox.Show("Please select some text for inline features", "Oops", MessageBoxButton.OK, MessageBoxImage.Exclamation); }
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
            for (int i = 0; i < Int32.MaxValue; i++) {
                if (!File.Exists(openNote.FilePath + "\\img\\" + i + ".png")) {
                    imageList[index].Save(openNote.FilePath + "\\img\\" + i + ".png");
                    Paragraph imageParagraph = new Paragraph();
                    imageParagraph.Inlines.Add(new System.Windows.Controls.Image {
                        Source = new BitmapImage(new Uri(imageLinks[i])),
                        Width = 200
                    });
                    RichNote.Document.Blocks.Add(imageParagraph);
                    return;
                }
            }

        }

        //Video Insert (not implemented)
        private List<string> videoLinks;
        private int videoIndex;

        private void ContextMenuVideo(object sender, RoutedEventArgs e) {
            if (RichNote.Selection != null && !RichNote.Selection.IsEmpty) {
                VideoSearchBox.Text = RichNote.Selection.Text;
                Dispatcher.BeginInvoke((Action)(() => InsertMedia.SelectedIndex = 0));
                SearchVideoClick(sender, new RoutedEventArgs());
            }
            else { MessageBox.Show("Please select some text for inline features", "Oops", MessageBoxButton.OK, MessageBoxImage.Exclamation); }
        }
        private void SearchVideoClick(object sender, RoutedEventArgs e) {
            PlaceVideos();
            throw new NotImplementedException();
        }
        private void PlaceVideos() {
            throw new NotImplementedException();
        }
        private void PlaceVideosPlus(object sender, RoutedEventArgs e) {
            videoIndex += 3;
            PlaceImages();
        }
        private void PlaceVideosMinus(object sender, RoutedEventArgs e) {
            videoIndex -= 3;
            PlaceImages();
        }
        private void VideoImageToRTB(object sender, RoutedEventArgs e, int index) {
            throw new NotImplementedException();
        }

        //TTS
        private WMPLib.WindowsMediaPlayer ttsPlayer;

        private void ContextMenuTts(object sender, RoutedEventArgs e) {
            if (RichNote.Selection != null && !RichNote.Selection.IsEmpty) {
                TextTrans.Text = RichNote.Selection.Text;
                Dispatcher.BeginInvoke((Action)(() => InsertMedia.SelectedIndex = 2));
                GenerateWavFile(sender, new RoutedEventArgs());
            }
            else { MessageBox.Show("Please select some text for inline features", "Oops", MessageBoxButton.OK, MessageBoxImage.Exclamation); }
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

        //STT
        private WaveInEvent waveIn;
        private WaveFileWriter RecordedAudioWriter;
        private void StartRecordStt(object sender, RoutedEventArgs e) {
            if (File.Exists(openNote.FilePath + "\\speech\\STT.wav")) File.Delete(openNote.FilePath + "\\speech\\STT.wav");
            RecordSpeech.IsEnabled = false;
            StopRecord.IsEnabled = true;
            waveIn = new WaveInEvent();
            RecordedAudioWriter = new WaveFileWriter(openNote.FilePath + "\\speech\\STT.wav", waveIn.WaveFormat);
            waveIn.StartRecording();
            waveIn.DataAvailable += (s, a) => {
                RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };
            waveIn.RecordingStopped += (s, a) => {};
        }
        private void StopRecordStt(object sender, RoutedEventArgs e) {
            RecordSpeech.IsEnabled = true;
            StopRecord.IsEnabled = false;
            waveIn.StopRecording();
            RecordedAudioWriter?.Dispose();
            RecordedAudioWriter = null;
            waveIn.Dispose();
            TransText.Text = SpeechToText.SpeechToTextFromFile(openNote.FilePath + "\\speech\\STT.wav");
        }
        
    }
}
