using System.Speech.Synthesis;
namespace BetterNotes
{
    public class TextToSpeech{
        static void GetSpeech(string input){
           SpeechSynthesizer reader = new SpeechSynthesizer();
           reader.SetOutputToDefaultAudioDevice();
           reader.Speak(input);
           reader.Dispose();
        }//getspeech

         static void PutSpeechInFile(string input, string path){
            SpeechSynthesizer reader = new SpeechSynthesizer();
            reader.SetOutputToWaveFile(path);
            reader.Speak(input);
            reader.Dispose();
        }//file
    }//class 
}//namespace