using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP_Mrowkowy
{
    public class Edge
    {
        public static double ro;
        public double pheromone;
        public double length;
        public double pheromoneToGain;
        public double visibility; //ROM
        public Cord A;
        public Cord B;
        public Edge(Cord a, Cord b)
        {
            A = a;
            B = b;
            //obliczanie dlugosci
            float xx = a.x - b.x;
            float yy = a.y - b.y;
            length = Math.Sqrt(xx * xx + yy * yy);
            //inicjacja domyslnych poziomow feromonu
            pheromone = Program.default_fero; //param in main
            visibility = 1.0 / length;         
        }
        public void GainPheromone()
        {
            //this.pheromone /= length;
            this.pheromone *= 1 - ro;
            this.pheromone += pheromoneToGain;
            this.pheromoneToGain = 0;
        }
    }
}
