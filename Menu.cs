using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Mrowkowy
{
    class Menu
    {
        public static void Start()
        {
        MainMenu:
            Console.WriteLine("O for options, R to run, D to draw, Q to quit");
            switch (System.Console.ReadKey(true).KeyChar)
            {
                case 'O':
                case 'o':
                    goto MenuO;

                case 'R':
                case 'r':
                    goto MenuR;

                case 'D':
                case 'd':
                    goto MenuD;

                case 'Q':
                case 'q':
                    return;

                default:
                    goto MainMenu;
            }
        MenuD:
            Console.WriteLine("P for pheromone traces, A for the best ant");
            switch (System.Console.ReadKey(true).KeyChar)
            {
                case 'P':
                case 'p':
                    Program.okno.ShowPhero();
                    goto MainMenu;

                case 'A':
                case 'a':
                    Program.okno.ShowBest();
                    goto MainMenu;
            }
        MenuO:
            Console.WriteLine("N to set number of ants, P to set pheromone (works as restart), any other key to return");
            switch (System.Console.ReadKey(true).KeyChar)
            {
                case 'N':
                case 'n':
                    Console.WriteLine("enter an integer");
                    if (Int32.TryParse(Console.ReadLine(), out Program.ile_mrowek))
                        Console.WriteLine("changes saved");
                    goto MainMenu;
                case 'P':
                case 'p':
                    Console.WriteLine("enter a non-zero double");
                    if (Double.TryParse(Console.ReadLine(), out Program.ilośćFero) && Program.ilośćFero != 0)
                    {
                        for (int i = 0; i < Program.ile_miast; i++)
                            for (int j = 0; j < Program.ile_miast; j++)
                                if (i != j)
                                    Program.edges[i, j].pheromone = Program.ilośćFero;
                        Console.WriteLine("changes saved");
                    }
                    goto MainMenu;
                default:
                    goto MainMenu;
            }
        MenuR:
            Console.WriteLine("enter a number of iterations");
            if (Int32.TryParse(Console.ReadLine(), out int N))
                for (int i = 1; i <= N; i++)
                {
                    for (int k = 1; k <= N; k *= 10)
                    {
                        if (i == k)
                        {
                            Console.Write($"{i} ");
                            break;
                        }
                        if (i % k == 0 && k*10 >= N)
                        {

                            Console.Write($"{i} ");
                            break;
                        }
                    }
                    Program.TripRom();
                }
            else
            {
                int default_iterations = 5;
                Console.WriteLine($"wybieram domyślne {default_iterations} iteracji");
                for (int i = 0; i < default_iterations; i++)
                    Program.TripRom();
            }
            //wypiszFero();

            goto MainMenu;
        }
    }
    
}
