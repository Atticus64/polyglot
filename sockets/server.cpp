#include <unistd.h>
#include <sys/socket.h>
#include <stdlib.h>
#include <netinet/in.h>
#include <iostream>

void execute_prog(int id) {
#if OS_Windows
	switch(id) {
		case 1:
			std::system("start notepad");
			break;
		case 2:
			std::system("start word");
			break;
		case 3:
			std::system("start brave-browser");
			break;
		case 4:
			std::system("start powerpoint");
			break;
		case 5:
			std::system("start excel");
			break;
		default: 
			std::cout << "Canceled");
			return;
	}
    // windows-specific code goes here
#else
	/*
	 * */
	std::cout << "id:  "<< id << "\n";
	switch(id) {
		case 1: 
			std::system("gnome-text-editor &");
			break;
		case 2: 
			std::system("libreoffice --writer &");
			break;
		case 3: 
			std::system("brave-browser &");
			break;
		case 4: 
			std::system("libreoffice --impress &");
			break;
		case 5: 
			std::system("libreoffice --calc &");
			break;
		default:
			return;
	}
    // linux-specific code
#endif

}



#define PORT 4545

int main(int argc, char const *argv[]) {
    int server_fd, new_socket;
	std::string valread;
    struct sockaddr_in address;
    int opt = 1;
    socklen_t addr_len = sizeof(address);

    if ((server_fd = socket(AF_INET, SOCK_STREAM, 0)) == 0) {
        perror("Error al crear el socket \n");
        exit(EXIT_FAILURE);
    }

    if (setsockopt(server_fd, SOL_SOCKET, SO_REUSEADDR | SO_REUSEPORT, &opt, sizeof(opt))) {
        perror("Error al crear el socket \n");
        exit(EXIT_FAILURE);
    }

    address.sin_family = AF_INET;
    address.sin_addr.s_addr = INADDR_ANY;
    address.sin_port = htons( PORT );

    if (bind(server_fd, (struct sockaddr *)&address, addr_len) < 0) {
        perror("Error al asociar el socket a IP y puerto\n");
        exit(EXIT_FAILURE);
    }

    if (listen(server_fd, 3) < 0) {
        perror("Error al poner el server en escucha \n");
        exit(EXIT_FAILURE);
    }

	while(true) {
    	char buffer[1024] = {0};
		puts("Servidor escuchando");
		if ((new_socket = accept(server_fd, (struct sockaddr *)&address, (socklen_t*)&addr_len)) < 0) {
			perror("Error al aceptar la conexion\n");
			exit(EXIT_FAILURE);
		}
		valread = read(new_socket, buffer, 1024);

		std::cout << buffer << "\n";
		int val = std::atoi(buffer);
		if (val < 1 && val > 5) {
			std::string msg = "Invalid option\n";
    		send(new_socket, msg.data(), msg.size(), 0);
			continue;
		}
		execute_prog(val);
	}

    return 0;
}
