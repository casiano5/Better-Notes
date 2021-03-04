using System.Speech.Synthesis;
namespace BetterNotes
{
    public class TextToSpeech{
        static void getSpeech(string input){
           SpeechSynthesizer reader = new SpeechSynthesizer();
           reader.SetOutputToDefaultAudioDevice();
           reader.Speak(input);
        }//getspeech

         static void putSpeechInFile(string input, string path){
            SpeechSynthesizer reader = new SpeechSynthesizer();
            reader.SetOutputToWaveFile(path);
            reader.Speak(input);
        }//file
    }//class 
}//namespace