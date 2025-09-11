
public class MainProgram
{

    public static void PrintBoard(char[,] board)
    {

        if (board == null) throw new ArgumentNullException(
            nameof(board));

        int n = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                n += 1;

                if (j == 0)
                {
                    Console.Write("|");
                }
                if (board[i, j] != '*')
                {
                    Console.Write(" " + board[i, j] + " |");
                }
                else
                {
                    Console.Write(" " + n + " |");
                }
            }

            if (i == 2)
            {
                continue;
            }

            Console.Write("\n ───────────");
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public static bool ValidMove(char[,] board, int move, char symbol)
    {
        int n = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (n == move && board[i, j] == '*')
                {
                    board[i, j] = symbol;
                    return true;
                }
                n++;
            }
        }

        return false;
    }

    public static bool CheckWinner(char[,] board, char player, char computer)
    {

        char first = board[0, 0];
        char second = board[0, 1];
        char third = board[0, 2];
        char fourth = board[1, 0];
        char fifth = board[1, 1];
        char sixth = board[1, 2];
        char seventh = board[2, 0];
        char eighth = board[2, 1];
        char ninth = board[2, 2];

        char? winner = null;

        // Horizontals
        if (first == second && second == third)
        {
            winner = first;
        }
        else if (fourth == fifth && fifth == sixth)
        {
            winner = fourth;
        }
        else if (seventh == eighth && eighth == ninth)
        {
            winner = seventh;
        }

        // Verticals
        if (first == fourth && fourth == seventh)
        {
            winner = first;
        }
        else if (second == fifth && fifth == eighth)
        {
            winner = second;
        }
        else if (third == sixth && sixth == ninth)
        {
            winner = third;

        }

        // cross
        if (first == fifth && fifth == ninth)
        {
            winner = first;
        }
        else if (third == fifth && fifth == seventh)
        {
            winner = third;
        }

        if (winner == '*')
            winner = null;

        if (winner != null)
        {
            if (winner == computer)
                Console.WriteLine("Gano el rival!");

            if (winner == player)
                Console.WriteLine("Ganaste!");

            PrintBoard(board);
        }

        return winner != null;

    }

    public static void GameCat()
    {

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        String logo = @"
  /$$$$$$              /$$     /$$   /$$              
 /$$__  $$            | $$    |__/  | $$              
| $$  \__/  /$$$$$$  /$$$$$$   /$$ /$$$$$$    /$$$$$$       /\_/\ 
| $$ /$$$$ |____  $$|_  $$_/  | $$|_  $$_/   /$$__  $$     ( o .o )
| $$|_  $$  /$$$$$$$  | $$    | $$  | $$    | $$  \ $$      > /\  < 
| $$  \ $$ /$$__  $$  | $$ /$$| $$  | $$ /$$| $$  | $$
|  $$$$$$/|  $$$$$$$  |  $$$$/| $$  |  $$$$/|  $$$$$$/
 \______/  \_______/   \___/  |__/   \___/   \______/                                 
";
        Console.WriteLine(logo);
        const String TURN_PLAYER = "player";
        const String TURN_COMPUTER = "computer";

        char[,] board = new char[3, 3];
        String turn = TURN_PLAYER;
        const char player = 'X';
        const char computer = '◯';

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = '*';
            }
        }


        while (true)
        {
            PrintBoard(board);
            if (turn == TURN_PLAYER)
            {
                Console.WriteLine("Turno jugador");
                Console.Write("Ingresa tu jugada: ");
                int jugada = Convert.ToInt32(Console.ReadLine());

                while (!ValidMove(board, jugada, player))
                {
                    Console.WriteLine("Casilla ocupada");
                    jugada = Convert.ToInt32(Console.ReadLine());
                }
            }
            else if (turn == TURN_COMPUTER)
            {
                Console.WriteLine("Turno del rival");
                int computer_jugada = new Random().Next(1, 11);

                while (!ValidMove(board, computer_jugada, computer))
                {
                    computer_jugada = new Random().Next(1, 11);
                }
            }

            if (CheckWinner(board, player, computer))
                break;

            turn = turn == TURN_PLAYER ? TURN_COMPUTER : TURN_PLAYER;
        }



    }

    public static Nullable<(int, int)> FindInMatrix(int elem, List<List<int>> items)
    {

        int i = 0;
        int lenght = items.Count;
        while (i < lenght)
        {
            for (int j = 0; j < items[i].Count; j++)
            {
                if (items[i][j] == elem)
                {
                    return (i, j);
                }
            }

            i++;
        }

        return null;
    }

    public static int Fibo(int n)
    {
        if (n <= 1) return n;

        return Fibo(n - 1) + Fibo(n - 2);
    }

    static void Menu()
    {

        string message = @"
Menu de opciones
    1) Mostrar serie Fibonacci
    2) Jugar con Matriz 3x3 
    3) Gatito
    4) salir
        ";

        int opc = 0;
        while (opc != 4)
        {
            Console.WriteLine(message);
            Console.Write("> ");
            opc = Convert.ToInt32(Console.ReadLine());
            switch (opc)
            {
                case 1:
                    Console.Write("Ingresa un número > ");
                    int fibNum = Convert.ToInt32(Console.ReadLine());
                    for (int i = 0; i < fibNum; i++)
                    {
                        Console.Write(Fibo(i) + ", ");
                    }
                    Console.WriteLine(Fibo(fibNum));
                    continue;
                case 2:
                    List<List<int>> matrix = [];
                    Console.WriteLine("LLenar matriz!");
                    for (int i = 0; i < 3; i++)
                    {
                        matrix.Add([]);
                        for (int j = 0; j < 3; j++) // 4 columns
                        {
                            Console.Write("Ingresa un número: ");
                            int number = Convert.ToInt32(Console.ReadLine()!);

                            matrix[i].Add(number); // Fill with some values
                        }
                    }

                    Console.WriteLine("Que valor deseas buscar? ");
                    int elem = Convert.ToInt32(Console.ReadLine()!);

                    (int, int)? resp = FindInMatrix(elem, matrix);

                    if (resp.HasValue)
                    {
                        Console.WriteLine("La coordenada es => " + resp.Value.ToString());
                    }
                    else
                    {
                        Console.WriteLine("No se encontro el elemento");
                    }

                    continue;
                case 3:
                    GameCat();
                    break;
                case 4:
                    Console.WriteLine("Saliendo...");
                    break;
                default:
                    continue;
            }
            ;
        }

    }

    static void Main()
    {

        Menu();

    }
}
