from random import randint


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


def print_cat():
    print("""
  /$$$$$$              /$$     /$$   /$$              
 /$$__  $$            | $$    |__/  | $$              
| $$  \__/  /$$$$$$  /$$$$$$   /$$ /$$$$$$    /$$$$$$       /\_/\ 
| $$ /$$$$ |____  $$|_  $$_/  | $$|_  $$_/   /$$__  $$     ( o .o )
| $$|_  $$  /$$$$$$$  | $$    | $$  | $$    | $$  \ $$      > /\  < 
| $$  \ $$ /$$__  $$  | $$ /$$| $$  | $$ /$$| $$  | $$
|  $$$$$$/|  $$$$$$$  |  $$$$/| $$  |  $$$$/|  $$$$$$/
 \______/  \_______/   \___/  |__/   \___/   \______/                                         
""")


def print_board(board):
    n = 0
    for i in range(3):
        for j in range(3):
            n += 1
            if j == 0:
                print("|", end="")
            if board[i][j] != '*':
                print(f" {board[i][j]} |", end="")
            else:
                print(f" {n} |", end="")

        if i == 2:
            continue
        print("\n ───────────", end="")

        print()

    print()


def valid_move(board, move, char):
    n = 1
    for i in range(3):
        for j in range(3):
            if n == move and board[i][j] == '*':
                board[i][j] = char
                return True
            n += 1

    return False


def check_win(board, player, computer):
    winner = None
    first = board[0][0]
    second = board[0][1]
    third = board[0][2]
    fourth = board[1][0]
    fifth = board[1][1]
    sixth = board[1][2]
    seventh = board[2][0]
    eighth = board[2][1]
    ninth = board[2][2]

    # horizontals
    if first == second and second == third:
        winner = first
    elif fourth == fifth and fifth == sixth:
        winner = fourth
    elif seventh == eighth and eighth == ninth:
        winner = seventh

    # verticals
    if first == fourth and fourth == seventh:
        winner = first
    elif second == fifth and fifth == eighth:
        winner = second
    elif third == sixth and sixth == ninth:
        winner = third

    # cross
    if first == fifth and fifth == ninth:
        winner = first
    elif third == fifth and fifth == seventh:
        winner = third

    if winner is not None and winner == '*':
        winner = None

    if winner is None:
        available = False
        for i in range(3):
            for j in range(3):
                if board[i][j] == '*':
                    available = True
                    break

        if not available :
            winner = 'D'

    if winner != '*' and winner is not None:
        if winner == computer:
            print("Gano el rival!")
        elif winner == player:
            print("Ganaste!!")
        else:
            print("Fue empate!")
        print_board(board)

    return winner != '*' and winner is not None


def game_cat():
    TURN_PLAYER = 'player'
    TURN_COMPUTER = 'computer'
    print_cat()
    board = [['*' for _ in range(3)] for _ in range(3)]
    turn = TURN_PLAYER
    jug = 'X'
    com = '◯'

    while True:
        print_board(board)
        if turn == TURN_PLAYER:
            print("Turno jugador")
            move = int(input(f"Ingresa tu jugada {jug}: "))

            while not valid_move(board, move, jug):
                print("casilla ya ocupada")
                move = int(input(f"Ingresa tu jugada {jug}: "))

        if turn == TURN_COMPUTER:
            print("Turno rival")
            move = randint(0, 9)
            while not valid_move(board, move, com):
                move = randint(0, 9)

        if check_win(board, jug, com):
            break

        turn = TURN_COMPUTER if turn == TURN_PLAYER else TURN_PLAYER


def menu():
    message = """
Menu de opciones
    1) Mostrar serie Fibonacci
    2) Jugar con Matriz 3x3 
    3) Jugar al gatito 
    4) salir
    """

    opc = 0
    while opc != 4:
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
                game_cat()
                continue
            case 4:
                print("Saliendo...")
                exit(0)

            case _:
                print("Opcion invalida!", end="")
                continue


if __name__ == '__main__':
    menu()
