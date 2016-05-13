using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bias_prototype
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Green;
            this.ClientSize = new Size(600,200);// setting the size of the interface
        }
       
       // first draw a curve from end to end.
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Point p1 = new Point(60, 70);
            Point p2 = new Point(100, 60);
            Point p3 = new Point(360, 20);
            Point p4 = new Point(460, 20);
            Point p5 = new Point(450, 20);
            Point p6 = new Point(500, 25);
            Point p7 = new Point(500, 40);
            Point p8 = new Point(500, 35);
            Point p9 = new Point(500, 40);
            Point p10 = new Point(500, 35);
     
            // creating points that will draw a curve
            // Point[] bias = { p1, p2, p3,p4,p5,p6,p7,p8,p9,p10};// placing all points into an array
            foreach (Point b in bwls)
	        {
		     g.DrawCurve(Pens.Black, );//drawing the curve onto the canvas
	        }

            
        }
        //public Point[] bias;
        public int power = 200;
        int startX ,startY;
        class bowls
        {
        public Point bias;
        }
        bowls bwls;
        List<bowls> newbias = new List<bowls>();
        private void timer1_Tick(object sender, EventArgs e)
        {
            bwls = new bowls();
           
            
            if (power < (200/2))
            {
                bwls.bias = new Point(startX + 5, startY + 3);
                newbias.Add(bwls);
            }
        }
        
        
    }
}
