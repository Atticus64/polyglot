#include <iostream>
#include <optional>
#include <vector>
#include <cstdlib>
using namespace std;

typedef vector<vector<char>> Board;
typedef vector<vector<int>> Matrix;


optional<tuple<int, int>> find_elem_matrix(int elem, const Matrix &matrix) {

    int i = 0;
    while (i < 3) {
        for (int j = 0; j < 3; j++) {
            if (matrix[i][j] == elem) {
                return make_tuple(i, j);
            }
        }
        i++;
    }

    return {};
}

int fibo(int n) {
    if (n <= 1)
        return n;

    return fibo(n - 1) + fibo(n - 2);
}

void print_cat() {
    cout <<
    "/ $$$$$$              /$$     /$$   /$$\n"
    "/ $$__  $$            | $$    |__/  | $$\n"
    "| $$  \__/   /$$$$$$  /$$$$$$   /$$ /$$$$$$    /$$$$$$       /\\_/\\ \n"
    "| $$ /$$$$ |____  $$|_  $$_/  | $$|_  $$_/   /$$__  $$     ( o .o )\n"
    "| $$|_  $$  /$$$$$$$  | $$    | $$  | $$    | $$   \ $$      > /\\  <\n"
    "| $$  \ $$  /$$__  $$  | $$ /$$| $$  | $$ /$$| $$  | $$ \n"
    "|  $$$$$$/|  $$$$$$$  |  $$$$/| $$  |  $$$$/|  $$$$$$/\n"
    "\______/  \_______/   \___/  |__/   \___/   \______/ \n\n";
}

void print_board(const Board &board) {

    int n = 0;
    for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
            n++;
            if (j == 0)
                cout << "|";
            if (board[i][j] != '*') {
                if (const char ch = board[i][j]; ch == 'O') {
                    cout << " " << "○" << " |";
                    continue;
                }

                cout << " " << board[i][j] << " |";
            } else {
                cout << " " << n << " |";

            }
        }
        if (i == 2)
            continue;
        cout << "\n ───────────";
        cout << "\n";
    }
    cout << "\n";
}

bool valid_move(Board &board, int move, char ch) {

    int n = 1;
    for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
            if (move == n && board[i][j] == '*') {
                board[i][j] = ch;
                return true;
            }

            n++;
        }
    }
    return false;
}

optional<char> check_winner(const Board &board, char player, char computer) {
    optional<char> winner = std::nullopt;




    char first = board[0][0];
    char second = board[0][1];
    char third = board[0][2];
    char fourth = board[1][0];
    char fifth = board[1][1];
    char sixth = board[1][2];
    char seven = board[2][0];
    char eight = board[2][1];
    char ninth = board[2][2];

    // horizontals
    if (first == second && second == third)
        winner = first;
    else if (fourth == fifth && fifth == sixth)
        winner = fourth;
    else if (seven == eight && eight == ninth)
        winner = seven;

    // verticals
    if (first == fourth && fourth == seven)
        winner = first;
    else if (second == fifth && fifth == eight)
        winner = second;
    else if (third == sixth && sixth == ninth)
        winner = third;

    // cross
    if (first == fifth && fifth == ninth)
        winner = first;
    else if (third == fifth && fifth == seven)
        winner = third;

    if (winner.has_value() && winner.value() == '*')
        winner = std::nullopt;

    if (!winner.has_value()) {
        bool available = false;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] == '*') {
                    available = true;
                }
            }
        }
        if (!available) {
            winner = 'D';
        }
    }

    if (winner.has_value()) {
        if (winner.value() == player) {
            cout << "Ganaste!!\n";
        } else if (winner.value() == computer) {
            cout << "Gano el rival!\n";
        } else {
            cout << "Fue empate" << "\n";
        }
        print_board(board);
    }

    return winner;
}

void game_cat() {
    const char* TURN_PLAYER = "player";
    const char* TURN_COMPUTER = "computer";

    print_cat();
    Board board = {
        { '*', '*', '*'},
        { '*', '*', '*'},
        { '*', '*', '*'},
    };

    string turn = TURN_PLAYER;
    char jug = 'X';
    char com = 'O';

    while (true) {
        print_board(board);
        if (turn == TURN_PLAYER) {
            cout << "Turno del Jugador" << "\n";
            int move = -1;
            cout << "Ingresa tu jugada: ";
            cin >> move;

            while (not valid_move(board, move, jug)) {
                cout << "Casilla ocupada \n";
                cout << "Ingresa tu jugada: ";
                cin >> move;
            }
        } else if (turn == TURN_COMPUTER) {
            cout << "Turno del Rival \n";
            int move = -1;
            move = rand()%(9-1 + 1) + 1;

            while (not valid_move(board, move, com)) {
                move = rand()%(9-1 + 1) + 1;
            }
        }

        optional<char> winner;
        winner = check_winner(board, jug, com);
        if (winner.has_value()) {
            break;
        }

        turn = TURN_PLAYER == turn ? TURN_COMPUTER : TURN_PLAYER;
    }

}

void menu() {

    int opc = 0;
    while (opc != 4) {
        cout << "Ingresa una opcion " << "\n";
        cout << "1. Fibonacci\n";
        cout << "2. Jugar con matrices\n";
        cout << "3. Gatito\n";
        cout << "4. Salir\n";
        cout << "> ";
        cin >> opc;
        cin.clear();

        switch (opc) {
            case 1: {
                cout << "Ingresa un número: ";
                cin >> opc;
                cin.clear();
                for (int i = 0; i < opc; i++) {
                    cout << fibo(i) << ", ";
                }
                cout << fibo(opc) << "\n";
                continue;
            }
            case 2: {
                Matrix m = {};
                cout << "Llenar matriz" << "\n";
                for (int i = 0; i < 3; i++) {
                    vector<int> line;
                    m.push_back(line);
                    for (int j = 0; j < 3; j++) {
                        int num = 0;
                        cout << "Ingresa un número: ";
                        cin >> num;
                        m[i].push_back(num);
                    }
                }

                int elem = 0;
                cout << "Elemento que deseas buscar: ";
                cin >> elem;
                optional<tuple<int, int>> coords = find_elem_matrix(elem, m);
                if (coords.has_value()) {
                    tuple<int, int> coord = coords.value();
                    cout << "Las coordenadas son (" << std::get<0>(coord);
                    cout << ", " << std::get<1>(coord) << ")" << "\n";
                } else {
                    cout << "No se encontro el elemento " << elem << "\n";
                }
                continue;
            }
            case 3: {
                game_cat();
                continue;
            }
            case 4: {
                cout << "Saliendo\n";
                exit(0);
            }
            default: {
                cout << "opcion invalida\n";
            }
        }
    }
}

int main() {
    menu();

    return 0;
}