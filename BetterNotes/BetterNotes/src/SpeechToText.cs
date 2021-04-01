using System;
using System.Speech.Recognition;

namespace BetterNotes {
    public static class SpeechToText {
        private static string returnValue = "";

        public static string SpeechToTextFromFile(string path) {
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            recognizer.LoadGrammar(new DictationGrammar());
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            recognizer.SetInputToWaveFile(path); 
            recognizer.Recognize();
            recognizer.Dispose();
            return returnValue;
        }
        private static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
            returnValue += e.Result.Text + ".";
        }
    }
}