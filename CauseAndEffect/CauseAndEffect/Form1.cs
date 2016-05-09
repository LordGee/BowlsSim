using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CauseAndEffect
{
    public partial class Form1 : Form
    {
        // This is a prototype to test for collision between two elipse 
        // This will also demonstrate to collision effect to move both objects
        // in an appropriate direction and speed.
        public Form1()
        {
            InitializeComponent();
        }
        // public variables
        public Rectangle bowl1, bowl2;
        public Point bowlA = new Point(0, 100);
        public Point bowlB = new Point(300,100);
        public int bowlAV = 600;
        public int bowlW = 75;

        private void Form1_Load(object sender, EventArgs e)
        {
            tmrAnimate.Interval = 30;
            tmrAnimate.Start();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // when the formloads draw two elipse
            e.Graphics.FillEllipse(Brushes.Green, bowl1);
            e.Graphics.FillEllipse(Brushes.Blue, bowl2);
        }

        private void tmrAnimate_Tick(object sender, EventArgs e)
        {
            
            if (bowlA.X < bowlAV)
            {
                bowlA.X = bowlA.X + 2;
                bowlAV--;
                
            }
            else
            {
                tmrAnimate.Stop();
            }
            bowl1 = new Rectangle(bowlA.X, bowlA.Y, bowlW, bowlW);
            bowl2 = new Rectangle(bowlB.X, bowlB.Y, bowlW, bowlW);
            if (bowl1.IntersectsWith(bowl2))
            {
                tmrAnimate.Stop();
            }
            else
            {
                Refresh();
            }
            
        }
    }
}
