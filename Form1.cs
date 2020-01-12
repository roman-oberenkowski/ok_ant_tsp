using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSP_Mrowkowy
{
    public partial class Form1 : Form
    {
        public static Grafika wyswietlacz;

        public Form1()
        {
            InitializeComponent();
            wyswietlacz = new Grafika(Program.cords);
            pictureBox1.Height = Grafika.rozmiar;
            pictureBox1.Width = Grafika.rozmiar;
            pictureBox1.Image = wyswietlacz.mapaPunkty;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        public void ShowPhero()
        {
            pictureBox1.Image = wyswietlacz.DrawRoads(Program.edges);
            ShowDialog();
        }
        public void ShowBest()
        {
            pictureBox1.Image = wyswietlacz.DrawBest(Program.cords);
            ShowDialog();
        }
    }
}
