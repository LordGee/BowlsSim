using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace crossregion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       Point p = new Point();
       public static int x = 50;
       public static int y = 100;

       public static int x1 = 29;
       public static int y1 = 121;
      // Defining the start X and Y cordinates for rectangle 

      GraphicsPath gpth, gp; //variables for referencing the Graphicspath class.

      Region rgn;
      Region rgn1;
    //declaring region class
      Rectangle rec1 = new Rectangle(x, y, 5, 50);
      Rectangle rec2 = new Rectangle(x1, y1, 50, 5);
      
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRegion(Brushes.Black, rgn);
            e.Graphics.FillRegion(Brushes.Black, rgn1);
       }//using the graphics for paint to fill defined regions with colour

        private void Form1_Load(object sender, EventArgs e)
        {
            drawCross();
            crossDrawn();
            timer1.Start();
        }
        public void drawCross()// function that adds the rectangle instance to the graphics path created
        {
            gpth = new GraphicsPath();
            gpth.AddRectangle(rec1);
            rgn = new Region(gpth);// adding the path to the region object
            Refresh();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
         
        }
        public void crossDrawn()
        {
            gp = new GraphicsPath();
            gp.AddRectangle(rec2);
            rgn1 = new Region(gp);
            Refresh();
        }
    }
}
