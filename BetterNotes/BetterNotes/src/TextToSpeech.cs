using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.IO;
using System.Windows;

namespace TextToSpeech
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer reader; 
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            reader = new SpeechSynthesizer(); 
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            textBox1.ScrollBars = ScrollBars.Both;
        }

        //SPEAK TEXT
        private void button1_Click(object sender, EventArgs e)
        {
            reader.Dispose();
            if (textBox1.Text != "")    //if text area is not empty
            {

                reader = new SpeechSynthesizer();
                reader.SpeakAsync(textBox1.Text);
                label2.Text = "SPEAKING";
                button2.Enabled = true;
                button4.Enabled = true;
                reader.SpeakCompleted += new EventHandler<speakcompletedeventargs /> (reader_SpeakCompleted);
            }
            else
            {
                object err = MessageBox.Show("Please enter some notes", "Message", MessageBoxButtons.OK);
            }
        }

        //event handler
        void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            label2.Text = "IDLE";
        }

        //PAUSE
        private void button2_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                if (reader.State == SynthesizerState.Speaking)
                {
                    reader.Pause();
                    label2.Text = "PAUSED";
                    button3.Enabled = true;

                }
            }
        }

        //RESUME
        private void button3_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                if (reader.State == SynthesizerState.Paused)
                {
                    reader.Resume();
                    label2.Text = "SPEAKING";
                }
                button3.Enabled = false;
            }
        }

        //STOP
        private void button4_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                reader.Dispose();
                label2.Text = "IDLE";
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
            }
        }

        //load data from text file button
        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //copy data from text file and load it to textbox
            textBox1.Text = File.ReadAllText(openFileDialog1.FileName.ToString());

        }

    }
}