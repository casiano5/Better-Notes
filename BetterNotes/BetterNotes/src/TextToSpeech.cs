using System.Speech.Synthesis;
namespace BetterNotes
{
    public class TextToSpeech{
        static void getSpeech(string input, SpeechSynthesizer reader){
           reader = new SpeechSynthesizer();
           reader.SetOutputToDefaultAudioDevice();
           reader.Speak(input.ToString());
        }//getspeech

         static void putSpeechInFile(string input, SpeechSynthesizer reader){
            reader = new SpeechSynthesizer();
            reader.SetOutputToWaveFile(@"C:\textToSpeech.wav");
            reader.Speak(input.ToString());
        }//file
    }//class 
}//namespace