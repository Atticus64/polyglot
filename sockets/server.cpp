#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <stdlib.h>
#include <netinet.h/in.h>
#include <string.h>

#define PORT 4545

int main(int argc, char const *argv[]) {
    int server_fd, new_socket, valread;
    struct sock_addr_in address;
    int opt = 1;
    int addr_len = sizeof(address);
    char buffer[1024] = {0};

    if ((server_fd = socket(AF_INET, SOCK_STREAM, 0)) == 0) {
        perror("Error al crear el socket \n");
        exit(EXIT_FAILURE);
    }

    if (setsockopt(server_fd, SOL_SOCKET, SO_REUSEADDR | SO_REUSEPORT, &opt, sizeof(opt))) {
        perror("Error al crear el socket \n");
        exit(EXIT_FAILURE);
    }

    address.sin_family = AF_INET;
    adress.sin_addr.s_addr = INADDR_ANY;
    address.sin_port = htons( PORT );

    if (bind(server_fd, (struct sock_addr *)&address, (sock_len_t*)&addrlen)) < 0) {
        perror("Error al asociar el socket a IP y puerto\n");
        exit(EXIT_FAILURE);
    }

    if (listen(server_fd, 3) < 0) {
        perror("Error al poner el server en escucha \n");
        exit(EXIT_FAILURE);
    }

    if ((new_socket = accept(server_fd, (struct sockaddr *)&address, (socklen_t*)&addr_len)) < 0) {
        perror("Error al aceptar la conexion\n");
        exit(EXIT_FAILURE);
    }

    valread = read(new_socket, buffer, 1024);
    printf("recibido: %s\n", buffer);

    return 0;
}
