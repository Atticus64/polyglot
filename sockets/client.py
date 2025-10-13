import socket
import base64
from tkinter.filedialog import askopenfilename

#HOST = "10.68.172.135"
HOST = "4.tcp.us-cal-1.ngrok.io"
#HOST = "127.0.0.1"
#PORT = 15000
PORT = 10593



with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))
    filename = askopenfilename()
    with open(filename, 'rb') as f:
        imagen_base64 = base64.b64encode(f.read()).decode('utf-8')
    	#imagen_bytes = f.read()

# Enviar tamaño primero (para que el cliente sepa cuántos bytes esperar)
    #size = len(imagen_bytes)
    #s.send(size.to_bytes(4, byteorder='big'))
    s.sendall(imagen_base64.encode('utf-8'))
    #s.send(imagen_bytes)
    #s.sendall(bytes(filename, 'UTF-8'))
    #s.sendall(b' <EOF>')

print("Mensaje enviado")


