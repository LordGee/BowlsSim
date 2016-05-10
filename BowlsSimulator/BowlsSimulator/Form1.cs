using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
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
        // Public Variables
        public int fs = 36; // Font Size
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Font ff = new Font("Comfortaa.ttf", fs, FontStyle.Bold);
            e.Graphics.DrawString(" Exit Game", ff, Brushes.DarkRed, new Point(1000, 600));
            e.Graphics.DrawString(" a Option ", ff, Brushes.DarkBlue, new Point(10, 600));
        }




        // this time next year we will be millionaires
    }
}
