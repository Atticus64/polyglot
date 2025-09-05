using System.Diagnostics;
using System.Numerics;

public class MainProgram
{


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
    3) salir
        ";

        int opc = 0;
        while (opc != 3)
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
                    } else
                    {
                        Console.WriteLine("No se encontro el elemento");
                    }

                    continue;
                case 3:
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
