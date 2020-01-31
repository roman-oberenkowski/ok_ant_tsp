using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Mrowkowy
{
    public class Ant
    {
        //constanst
        static double factor_phero = 1; //anfa
        static double factor_visib = 3.0; //beta
        
        //pola
        public static Random rnd;
        public int currentCity;
        public bool[] visited;
        public double travelledDistance = 0;
        public List<int> visitOrder = new List<int>();
        public static int cities;
        public static Cord[] cords;
        public static Edge[,] edges;
        public Ant()
        {
            this.currentCity = rnd.Next(cities); //losowanie miasta startowego
            //Console.WriteLine("Wylosowant start: "+currentCity);
            visitOrder.Add(currentCity);
            visited = new bool[cities];
            //visitOrder = new int[cities+1]; //+1 dla ostatniego miasta, które będzie i na 0 i na n pozycji (cykl)
            for (int i = 0; i < cities; i++) //inicjalizacja tablicy - same false poza miastem startowym
            {
                visited[i] = (currentCity == i);     
            }
            //visited.ToList().ForEach(i => Console.WriteLine(i.ToString()));
        }
        void goTo(int cityNumber)//funkcja przesuwająca mrówkę do podanego miasta 
        {
            if (visited[cityNumber])
            {
                //spr, czy właśnie wróciła mrówka do domku
                if (cityNumber == visitOrder[0])
                {

                    //Console.WriteLine("NO ERROR - going home"); 
                    visitOrder.Add(cityNumber);
                    travelledDistance += edges[cityNumber, currentCity].length;
                    currentCity = cityNumber;
                    return;
                }
                else { Console.WriteLine("ERROR: already been in here"); }
            }
            visitOrder.Add(cityNumber);
            visited[cityNumber] = true;
            //Console.WriteLine($"ERRROR?: {cityNumber} {currentCity}");
            travelledDistance += edges[cityNumber, currentCity].length;
            //dodaj feromon!!!----------------------
            edges[cityNumber, currentCity].pheromoneToGain += 100;
            //po przejściu ustaw obecne miasto
            currentCity = cityNumber;
        }
        public void goNextAuto() //funkcja przesuwająca mrówkę automatycznie do kolejnego miasta (uwzględnia prawdopodobieństwa)
        {
            List<int> targets=getPossibleTargetList();
            List<double> probability = getProbabilityList(targets);

            //double sum = probability.Sum(); //mayby UINT64?
            int item = 0;
            //Console.WriteLine($"tag: {targets.Count()}, prob: {probability.Count()}");
            if (probability.Count()<=0)
            {
                //Console.WriteLine("NIE MOGE DALEJ ISC - wracam do domku");
                goTo(visitOrder[0]);
                return;
            }
            double randomVal = rnd.NextDouble()*probability.Last(); //check this
            for (int i = 0; i < targets.Count(); i++)
                if (randomVal < probability[i])
                {
                    //odwiedzone_wierzchołki[i] = true;
                    //kolejnosc[etap] = i;
                    item = i;
                    break;
                }
            //Console.WriteLine($"TESTING RNG: {item} {targets[item]}");
            goTo(targets[item]);
        }
        List<int> getPossibleTargetList()
        {
            List<int> wynik = new List<int>();
            for (int city = 0; city < cities; city++)
            {
                if (!visited[city])
                {
                    wynik.Add(city);
                }
            }
            return wynik;
        }
        double getProbabilityItem(int targetCity) //oblicz prawdopodobieństwo przejścia do targetciti (z currentCity)
        {
            double pheromone = edges[currentCity, targetCity].pheromone;
            double visibility = edges[currentCity, targetCity].visibility; //1.0/edges[currentCity, targetCity].length;
            return  Math.Pow(pheromone, factor_phero) * Math.Pow(visibility, factor_visib);
        }
        List<double> getProbabilityList(List <int> targets) //obicz probab. dla wszystkich podanych miast (dostępnych, czyli nieodwiedzonych)
        {
            int length = targets.Count();
            List<double> probabilities = new List<double>();
            for (int item = 0; item < length; item++)
            {
                if (item != 0)
                {
                    probabilities.Add(getProbabilityItem(targets[item]) + probabilities[item - 1]);
                }
                else
                {
                    probabilities.Add(getProbabilityItem(targets[item]));
                }
            }
            return probabilities;
        }
        public void checkIfCorrect()
        {
            bool error = false;
            double droga = 0;
            if (visitOrder.First() != visitOrder.Last())
            {
                error = true;
            }
            for (int i = 0; i < visitOrder.Count()-1; i++)
            {
                droga+= edges[visitOrder[i], visitOrder[i+1]].length;
                if (!visited[i]) error = true;
            } 
            if(travelledDistance<0.99*droga || travelledDistance > 1.01 * droga)
            {
                error = true;
            }
            if (error) Console.WriteLine("D***-zły wynik");
            //else { Console.WriteLine(" OK"); }
        }
    }
}
