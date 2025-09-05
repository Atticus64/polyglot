
def find_elem_matrix(elem, matrix) -> (int, int):
    i = 0
    length = len(matrix)
    while i < length:
        for j in range(len(matrix[i])):
            if matrix[i][j] == elem:
                return i, j

        i += 1

    return None


def fibo(n: int) -> int:
    if n <= 1: return n

    return fibo(n - 1) + fibo(n - 2)

def menu():
    message = """
Menu de opciones
    1) Mostrar serie Fibonacci
    2) Jugar con Matriz 3x3 
    3) salir
    """

    opc = 0
    while opc != 3:
        print(message)
        opc = int(input("> "))
        match opc:
            case 1:
                fib = int(input("Ingresa un número > "))
                for i in range(fib):
                    print(f"{fibo(i)}, ", end="")

                print(fibo(fib))
                continue
            case 2:
                m = []
                print("Llenar matriz!")
                for i in range(3):
                    m.append([])
                    for j in range(3):
                        num = int(input("Ingresa un número: "))
                        m[i].append(num)

                elem = int(input("Que valor deseas buscar? "))
                res = find_elem_matrix(elem, m)
                if res is not None:
                    print(f"La coordenada es: {res}")
                else:
                    print("No se encontro el elemento")

                continue
            case 3:
                print("Saliendo...")
                exit(0)
            case _:
                print("Opcion invalida!", end="")
                continue


if __name__ == '__main__':
    menu()
