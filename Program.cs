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
        public static double ilośćFero = 10;
        public static double default_fero = 10;
        public static int reset_every = 50;
        public static int ile_miast;
        public static Cord[] cords;
        public static Edge[,] edges;
        public static Form1 okno;
        public static Ant GOAT;
        public static string filename = "berlin52";
        public static bool restart = false;
        static Cord[] input(ref int ile_miast)
        {
            

            string[] lines = System.IO.File.ReadAllLines(filename+".tsp");
            //load ile_miast
            string[] split_dimension = { "", "" };
            split_dimension = lines[3].Split(' ', '\t');
            ile_miast = Int32.Parse(split_dimension[1]);
            //Console.WriteLine($"ile mista: {ile_miast}");
            Cord[] kordy = new Cord[ile_miast];
            int start = 0;
            string[] split_cords = {"","","" };
            for (int i = 0; i < 10; i++)
            {
                split_cords = lines[i].Split(' ', '\t');
                if (Int32.TryParse(split_cords[0], out start))
                {
                    start = i;
                    break;
                }
            }
            //Console.WriteLine($"start: {start}");
            for (int i = 0; i < ile_miast; i++)
            {
               
                string[] XY = { "", "","" };
                XY = lines[i+start].Split(' ', '\t');
                float x, y;
                x = float.Parse(XY[1]);
                y = float.Parse(XY[2]);
                //Console.WriteLine($"{i}, {x}, {y}");
                kordy[i] = new Cord(x, y);
            }
            return kordy;
        }

        public static double Trip()
        {
            //towrzymy zadaną ilość mrówek i przypisujemy im miasto startowe (losowo)
            Ant[] ants = new Ant[ile_mrowek];
            for (int i = 0; i < ile_mrowek; i++)
            {
                //ants[i]= new Ant(ile_miast,ref cords,ref edges);
                ants[i] = new Ant();
            }
            //robimy tyle ruchów ile jest miast (ostatni ruch to powrót do miasta startowego
            for (int ruch = 0; ruch < ile_miast; ruch++)
            {
                //każdą mrówkę przesuwamy o 1 miasto
                for (int antId = 0; antId < ile_mrowek; antId++)
                {
                    ants[antId].goNextAuto();
                }
            }
            //znajdź najlepszą mrówkę (porównując travelled distance)
            double minimal = ants[0].travelledDistance;
            int IDofMinimal = 0;
            if (restart)
            {
                GOAT = null;
                restart = false;
            }
            if (GOAT == null || GOAT.travelledDistance > ants[0].travelledDistance)
                GOAT = ants[0];
            for (int antId = 1; antId < ile_mrowek; antId++)
            {
                if (ants[antId].travelledDistance < minimal)
                {
                    minimal = ants[antId].travelledDistance;
                    IDofMinimal = antId;
                    if (GOAT.travelledDistance > ants[antId].travelledDistance)
                        GOAT = ants[antId];
                }
            }
            Form1.wyswietlacz.bestWay = GOAT.visitOrder;
            

            //Console.Write($"wyniczek: {minimal}");
            ants[IDofMinimal].checkIfCorrect();

            for (int antId = 0; antId < ile_mrowek; antId++)
            {
                //Console.WriteLine($"ant:{antId} dist: {ants[antId].travelledDistance}");
                ////ants[antId].visitOrder.ForEach(Console.WriteLine);
                //Console.WriteLine("[{0}]", string.Join(", ", ants[antId].visitOrder));
            }

            //FINIGING Touches ------ŹLE??? nie w tym miejscu
            for (int i = 0; i < ile_miast; i++)
                for (int j = 0; j < ile_miast; j++)
                    if (i != j)
                        edges[i, j].GainPheromone(); // feromon jest kalkulowany po tym, gdy przejdą wszystkie mrówki
            return minimal;
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
        static List<int> load_opt()
        {
            string[] lines = System.IO.File.ReadAllLines(filename+".opt.tour");
            List<int> lista = new List<int>();
            lista.Clear();
            int nr = 0;
            int start = 0;
            bool error = false;
            //10 prób na wczytanie pierwszej wart z pliku - błędy na liniach z tekstem
            for (int i = 0; i < 10; i++)
            {
                if (Int32.TryParse(lines[i], out start))
                {
                    start = i;
                    error = false;
                    break;
                }
                error = true;
            }
            if (error) throw new System.ArgumentException("OptFileLoadFailed");
            //-1, bo wczytuje wcześniej jedną
            for (int i = start; i < ile_miast + start; i++)
            {
                nr = Int32.Parse(lines[i]) - 1;
                lista.Add(nr);
            }
            ;
            nr = Int32.Parse(lines[ile_miast + start]);



            if (nr == -1) //not 0, becouse i don't subrtract while reading last
            {
                lista.Add(lista.First());
                //calculate optimal length:
                
                double droga = 0;
                for (int i = 0; i < lista.Count() - 1; i++)
                {
                    droga += edges[lista[i], lista[i + 1]].length;
                    
                }
                
                
                //Console.WriteLine("[" + string.Join(", ", lista) + "]");
                Console.WriteLine($"OPTIMAL_DIST: {droga}");
                return lista;
            }
            else
            {
                throw new System.ArgumentException("OptFileLoadFailed");
            }
            


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
        static void run_greedy()
        {
            double greedy_travelled_distance = 0;
            var greedy_visited = new List<bool>();
            var greedy_visit_order = new List<int>();
            for (int i = 0; i < ile_miast; i++)
            {
                greedy_visited.Add(false);
            }
            int current = 0;
            greedy_visited[0] = true;
            greedy_visit_order.Add(0);
            for (int i = 0; i < ile_miast - 1; i++)
            {
                //find closest
                int closest_id = -1;
                for (int candidate = 0; candidate < ile_miast; candidate++)
                {
                    if (greedy_visited[candidate]) continue;
                    //only if NOT visited - cannot go to self, becouse self is already visited
                    if (closest_id == -1) closest_id = candidate;
                    else
                    {
                        if (edges[current, candidate].length < edges[current, closest_id].length)
                        {
                            closest_id = candidate;
                        }
                    }
                }
                //Console.WriteLine($"x: {current} y: {closest_id}");
                greedy_travelled_distance += edges[current, closest_id].length;
                current = closest_id;
                greedy_visited[current] = true;
                greedy_visit_order.Add(current);
            }
            //go back to home cityy
            greedy_travelled_distance += edges[current, greedy_visit_order.First()].length;
            greedy_visit_order.Add(greedy_visit_order.First());
            //Console.WriteLine(string.Join(" ", greedy_visit_order));
            Console.WriteLine($"GREEDY_DIST: {greedy_travelled_distance}");
        }
        public static void show_goat()
        {
            Console.WriteLine($"GOAT_DIST: {GOAT.travelledDistance}");
        }
        public static void reset_fero()
        {
            //Console.WriteLine("reset_fero");
            for (int i = 0; i < Program.ile_miast; i++)
                for (int j = 0; j < Program.ile_miast; j++)
                    if (i != j)
                        Program.edges[i, j].pheromone = Program.default_fero; 
        }
        static void Main(string[] args)
        {
            TSP_Mrowkowy.Program.cords= input(ref ile_miast);
            edges = new Edge[ile_miast, ile_miast];
            TSP_Mrowkowy.Edge.ro = 0.1;
            TSP_Mrowkowy.Ant.rnd = new Random();
            TSP_Mrowkowy.Ant.edges = edges;
            TSP_Mrowkowy.Ant.cords = cords;
            TSP_Mrowkowy.Ant.cities = ile_miast;
            for (int i = 0; i < ile_miast; i++)
                for (int j = 0; j < ile_miast; j++)
                    if (i != j)
                        edges[i, j] = new Edge(cords[i], cords[j]);
            okno = new Form1();
            if (filename == "berlin52" || filename== "kroC100" || filename== "pr76") load_opt();
            if (filename == "tsp1000") Console.WriteLine("Best from mmachowiak: 24724");
            if (filename == "tsp500") Console.WriteLine("Best from mmachowiak: 84585");
            if (filename == "tsp250") Console.WriteLine("Best from mmachowiak: 12642");
            run_greedy();
            //else
            //    edges[i, j] = new Edge(Cord.Zero(), Cord.Zero());
            //wypiszPunkty();
            //wypiszDist();
            Menu.Start();
        }
    }
}
