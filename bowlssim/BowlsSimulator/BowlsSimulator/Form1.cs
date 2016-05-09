using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BowlsSimulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(" Exit Game", new Font("ComfortaaBold.ttf", 36), Brushes.RoyalBlue, new Point(1000, 600));
            e.Graphics.DrawString(" Option ", new Font("Comfortaa.ttf", 36), Brushes.RoyalBlue, new Point(10, 600));

        }




        // this time next year we will be millionaires
    }
}
