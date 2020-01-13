using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TSP_Mrowkowy
{
    public class Grafika
    {
        public static int rozmiar = 1005;
        public Bitmap mapaPunkty = new Bitmap(rozmiar,rozmiar);
        float maxXY = 0;
        static float scale;
        public List<int> bestWay;

        public Grafika(Cord[] cords)
        {
            //znajdź max dla skali
            foreach (Cord pkt in cords)
            {
                if (pkt.x > maxXY)
                    maxXY = pkt.x;
                if (pkt.y > maxXY)
                    maxXY = pkt.y;
            }
            scale = 1000 / maxXY;
            DrawPoints(cords);
        }

        void DrawPoints(Cord[] cords)
        {
            foreach (Cord pkt in cords)
            {
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        mapaPunkty.SetPixel(Convert.ToInt32(pkt.x * scale) + i, Convert.ToInt32(pkt.y * scale) + j, Color.Red);
            }
        }

        public Bitmap DrawRoads()
        {
            Pen pheroPen = new Pen(Color.Black, 1);
            double maxPhero = 0;

            for (int i = 0; i < Program.ile_miast; i++)
                for (int j = i + 1; j < Program.ile_miast; j++)
                    if (maxPhero < Program.edges[i, j].pheromone)
                        maxPhero = Program.edges[i, j].pheromone;

            Bitmap mapaDrogi = new Bitmap(mapaPunkty);

            for (int i = 0; i < Program.ile_miast; i++)
                for (int j = i + 1; j < Program.ile_miast; j++)
                    using (var graphics = Graphics.FromImage(mapaDrogi))
                    {
                        pheroPen.Color = Color.FromArgb(Convert.ToInt32(Program.edges[i, j].pheromone / maxPhero * 255), Color.Black);
                        float x1 = Program.edges[i, j].A.x * scale;
                        float y1 = Program.edges[i, j].A.y * scale;
                        float x2 = Program.edges[i, j].B.x * scale;
                        float y2 = Program.edges[i, j].B.y * scale;
                        graphics.DrawLine(pheroPen, x1, y1, x2, y2);
                    }

            return mapaDrogi;
        }

        public Bitmap DrawBest(List<int> way = null)
        {
            if (way == null)
                way = bestWay;
            Pen pheroPen = new Pen(Color.Black, 1);

            Bitmap mapaDrogi = new Bitmap(mapaPunkty);

            using (var graphics = Graphics.FromImage(mapaDrogi))
            {
                for (int i = 1; i <= Program.ile_miast; i++)
                {
                    float x1 = Program.cords[way[i]].x * scale;
                    float y1 = Program.cords[way[i]].y * scale;
                    float x2 = Program.cords[way[i-1]].x * scale;
                    float y2 = Program.cords[way[i-1]].y * scale;

                    graphics.DrawLine(pheroPen, x1, y1, x2, y2);
                }
            }


            return mapaDrogi;
        }
    }
}
