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
            Console.WriteLine("\nO for options, R to run, D to draw, Q to quit, X=restart");
            Console.Write(">>");
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
                case 'x':
                case 'X':
                    goto MenuX;

                default:
                    goto MainMenu;
            }
        MenuD:
            Console.WriteLine("Draw\n\tP for pheromone traces, \n\tA for the best ant of last iteration, \n\tB for the best ant ever, \n\tany other key to return");
            Console.Write("\t>>");
            switch (System.Console.ReadKey(true).KeyChar)
            {
                case 'P':
                case 'p':
                    Program.okno.ShowPhero();
                    goto MainMenu;

                case 'A':
                case 'a':
                    Program.okno.ShowBest(); //program się wysypie jeśli nie było w ogóle mrówek
                    goto MainMenu;

                case 'B':
                case 'b':
                    Program.okno.ShowBest(Program.GOAT.visitOrder); //program się wysypie jeśli nie było w ogóle mrówek
                    goto MainMenu;

                default:
                    goto MainMenu;
            }
        MenuO:
            Console.WriteLine("Options\n\tN to set number of ants, \n\tP to set pheromone (works as restart), \n\tany other key to return");
            Console.Write("\t>>");
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
            Console.WriteLine("Run\n\tenter a number of iterations");
            Console.Write("\t>>");
            if (Int32.TryParse(Console.ReadLine(), out int N))
                for (int param = 0; param < 5; param++)
                {
                    TSP_Mrowkowy.Edge.ro = Program.params_ro[param];
                    TSP_Mrowkowy.Ant.factor_phero = Program.params_a[param];
                    TSP_Mrowkowy.Ant.factor_visib = Program.params_b[param];
                    for (int i = 1; i <= N; i++)
                    {

                        if (i % Program.reset_every == Program.reset_every / 2) Program.reset_fero();
                        Console.WriteLine(Program.Trip());
                    }
                }
            Program.show_goat();

            goto MainMenu;
        MenuX:
            for (int i = 0; i < Program.ile_miast; i++)
                for (int j = 0; j < Program.ile_miast; j++)
                    if (i != j)
                        Program.edges[i, j].pheromone = Program.default_fero;
            Console.WriteLine("changes saved - default fero; restarted");
            Program.restart = true;
            goto MainMenu;
        }
    }
}
