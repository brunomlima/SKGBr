# SKGBr

SKGBr é um projeto .NET 4.0 para consolidar diversas funções usadas para interagir com o hardware de um usuário, incluindo:
	
	- Screenshots (Display + Foto da WebCam)
	- Audio (Microfones e Alto-falantes)
	- Keylogger (Grava teclas em um arquivo)
	- Ative a gravação de voz quando o usuário disser uma frase-chave.

Para cada função, o SKGBr gravará o arquivo resultante no diretório temporário com o sufixo apropriado.

## Usage

```
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
```
