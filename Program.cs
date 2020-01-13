using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Mrowkowy
{ 
    public class Program
    { 
        public static int ile_mrowek = 10;
        public static double ilośćFero = 1;
        public static int ile_miast;
        static Cord[] input(ref int ile_miast)
        {
            string[] lines = System.IO.File.ReadAllLines("input52.txt");
            ile_miast = Int32.Parse(lines[0]);
            Cord[] kordy = new Cord[ile_miast];
            for (int i = 0; i < ile_miast; i++)
            {
                string[] XY = {"", ""};
                XY = lines[i + 1].Split(' ', '\t');
                float x, y;
                x = float.Parse(XY[0]);
                y = float.Parse(XY[1]);
                kordy[i] = new Cord(x,y);
            }
            return kordy;
        }
        
        public static Cord[] cords = input(ref ile_miast);
        public static Edge[,] edges = new Edge[ile_miast, ile_miast];
        public static void TripRom()
        {
            //ile_miast,ile_mrowek,edges[x,y],
            //towrzymy zadaną ilość mrówek i przypisujemy in miasto startowe (losowo)
            Ant[] ants=new Ant[ile_mrowek];
            for (int i = 0; i < ile_mrowek; i++)
            {
                //ants[i]= new Ant(ile_miast,ref cords,ref edges);
                ants[i] = new Ant();
            }
            //robimy tyle ruchów ile jest miast 
            for(int ruch = 0; ruch < ile_miast; ruch++)
            {
                //każdą mrówkę przesuwamy o 1 miasto
                for(int antId = 0; antId < ile_mrowek; antId++)
                {
                    ants[antId].goNextAuto();
                }
            }
            //znajdź najlepszą mrówkę (porównując travelled distance)
            double minimal = ants[0].travelledDistance;
            int IDofMinimal = 0;
            for(int antId = 1; antId < ile_mrowek; antId++)
            {
                if (ants[antId].travelledDistance < minimal)
                {
                    minimal = ants[antId].travelledDistance;
                    IDofMinimal = antId;
                }
            }
            Form1.wyswietlacz.bestWay = ants[IDofMinimal].visitOrder;

            Console.Write($"wyniczek: {minimal}");
            ants[IDofMinimal].checkIfCorrect();

            for (int antId = 0; antId < ile_mrowek; antId++)
            {
                //Console.WriteLine($"ant:{antId} dist: {ants[antId].travelledDistance}");
                ////ants[antId].visitOrder.ForEach(Console.WriteLine);
                //Console.WriteLine("[{0}]", string.Join(", ", ants[antId].visitOrder));
            }

            //FINIGING Touches ------ŹLE nie w tym miejscu
            for (int i = 0; i < ile_miast; i++)
                for (int j = 0; j < ile_miast; j++)
                    if (i != j)
                        edges[i, j].GainPheromone(); // feromon jest kalkulowany po tym, gdy przejdą wszystkie mrówki
        }
        static void wypiszPunkty()
        {
            for (int i = 0; i < ile_miast; i++)
            {
                Console.WriteLine($"x:{cords[i].x} y:{cords[i].y}");
            }
        }
        static void wypiszFero()
        {
            for (int i = 0; i < ile_miast; i++)
            {
                for (int j = 0; j < ile_miast; j++)
                    if (i == j)
                        Console.Write($"\t");
                    else
                        Console.Write($"{Math.Round(edges[i, j].pheromone, 1)}\t");
                Console.WriteLine("");
            }
            Console.WriteLine("------------------------");
        }
        
        static void wypiszDist()
        {
            for (int i = 0; i < ile_miast; i++)
            {
                for (int j = 0; j < ile_miast; j++)
                    if (i == j)
                        Console.Write($"\t");
                    else
                        Console.Write($"{Math.Round(edges[i, j].length, 1)}\t");
                Console.WriteLine("");
            }
            Console.WriteLine("------------------------");
        }
        static void test_rng()
        {
            int[] tab = { 0, 0, 0, 0 };
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {

                List<int> probability = new List<int>() { 1, 10, 3, 10 };
                List<int> targets = new List<int>() { 0, 1, 2, 3 };
                int sum = probability.Sum(); //mayby UINT64?
                int counter = 0;
                int item = 0;

                int randomVal = rnd.Next(0, sum); //check this
                Console.WriteLine($"RND:{randomVal}");
                while (counter <= randomVal)
                {
                    counter += probability[item++];
                }
                item--;
                tab[item]++;
            }
            Console.WriteLine($"{tab[0]}, {tab[1]}, {tab[2]}, {tab[3]}");
        }

        public static Form1 okno = new Form1();

        static void Main(string[] args)
        {
            TSP_Mrowkowy.Ant.rnd = new Random();
            TSP_Mrowkowy.Ant.edges = edges;
            TSP_Mrowkowy.Ant.cords = cords;
            TSP_Mrowkowy.Ant.cities = ile_miast;
            for (int i = 0; i < ile_miast; i++)
                for (int j = 0; j < ile_miast; j++)
                    if (i != j)
                        edges[i, j] = new Edge(cords[i], cords[j]);
                    //else
                    //    edges[i, j] = new Edge(Cord.Zero(), Cord.Zero());
            //wypiszPunkty();
            //wypiszDist();
            Menu.Start();
        }
    }
}
