#include <unistd.h>
#include <iostream>
#include <stdlib.h>
#include <sys/socket.h>
#include <arpa/inet.h>

#define PORT 4545

int main(int argc, char const *argv[]) {
    int sock = 0, valread;
    struct sockaddr_in serv_addr;
    // const char* msg = "Mensaje desde cliente Ccrosscross\n";
	int opcion = 0;
	std::cout << "Elige una opcion" << "\n";
	std::cout << "1. Editor notas" << "\n";
	std::cout << "2. Editor de Texto" << "\n";
	std::cout << "3. Navegador" << "\n";
	std::cout << "4. Editor Presentaciones" << "\n";
	std::cout << "5. Hoja de calculo" << "\n";
	std::cin >> opcion;
	std::string val = std::to_string(opcion);
    int opt = 1;
    char buffer[1024] = {0};

    if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        perror("Error al crear el socket \n");
        return -1;
    }

    serv_addr.sin_family = AF_INET;
    serv_addr.sin_port = htons( PORT );

    if (inet_pton(AF_INET, "127.0.0.1", &serv_addr.sin_addr) <= 0) {
        printf("Direccion IP no valida \n");
        return -1;
    }

    if (connect(sock, (struct sockaddr *)&serv_addr, sizeof(serv_addr)) < 0) {
        perror("Fallo de conexion \n");
        exit(EXIT_FAILURE);
    }

    send(sock, val.data(), val.size(), 0);

	std::cout << "Mensaje enviado\n";

    return 0;
}
