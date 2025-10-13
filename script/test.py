import socket
import base64
import threading
import tkinter as tk
from tkinter import filedialog, messagebox

HOST = "4.tcp.us-cal-1.ngrok.io"
PORT = 10593


def enviar_imagen(filename):
    try:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((HOST, PORT))
            with open(filename, 'rb') as f:
            	imagen_base64 = base64.b64encode(f.read()).decode('utf-8')
            s.sendall(imagen_base64.encode('utf-8'))
        
        messagebox.showinfo("Éxito", "Imagen enviada correctamente")
    except Exception as e:
        messagebox.showerror("Error", f"No se pudo enviar la imagen:\n{e}")


def seleccionar_y_enviar():
    filename = filedialog.askopenfilename(
        title="Seleccionar imagen",
        filetypes=[("Imágenes", "*.jpg *.jpeg *.png *.bmp *.gif")]
    )
    if filename:
        threading.Thread(target=enviar_imagen, args=(filename,), daemon=True).start()



# Crear ventana principal
root = tk.Tk()
root.title("Cliente de envío de imagen")
root.geometry("400x200")
root.config(bg="#1e1e1e")

# Etiqueta de título
titulo = tk.Label(
    root, 
    text="Envío de imagen al servidor", 
    font=("Segoe UI", 14, "bold"),
    fg="white", 
    bg="#1e1e1e"
)
titulo.pack(pady=20)

# Botón de selección
boton_select = tk.Button(
    root, 
    text="Seleccionar imagen y enviar", 
    command=seleccionar_y_enviar, 
    font=("Segoe UI", 11),
    bg="#0078D7", 
    fg="white", 
    relief="raised",
    padx=10, 
    pady=5
)
boton_select.pack(pady=20)


root.mainloop()
