#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <stdlib.h>
#include <netinet.h/in.h>
#include <string.h>

#define PORT 4545

int main(int argc, char const *argv[]) {
    int sock = 0, valread;
    struct sock_addr_in serv_addr;
    char* msg = "Mensaje desde cliente Ccrosscross\n";
    int opt = 1;
    char buffer[1024] = {0};

    if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        perror("Error al crear el socket \n");
        return -1;
    }
   }

    serv_addr.sin_family = AF_INET;
    serv_addr.sin_port = htons( PORT );

    if (inet_pton(AF_INET, "127.0.0.1", &serv_addr.sin_addr) <= 0) {
        printf("Direccion IP no valida ]n");
        return -1;
    }

    if (connect(sock, (struct sockaddr *)&address, sizeof(serv_addr)) < 0) {
        perror("Fallo de conexion \n");
        exit(EXIT_FAILURE);
    }

    send(sock, msg, strlen(msg), 0);

    printf("Mensaje enviado\n");

    return 0;
}
