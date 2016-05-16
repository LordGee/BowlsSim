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
        public RectangleF bowl1, bowl2;
        public Region bowl3, bowl4;
        public GraphicsPath pth;
        public PointF bowlA = new Point(0, 101);
        public PointF bowlB = new Point(300,150);
        public int bowlAV = 800;
        public int bowlW = 75;
        public int bowlBV = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            tmrAnimate.Interval = 30;
            tmrAnimate.Start();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //bowl3 = new Region(pth);
            
            // when the formloads draw two elipse
            e.Graphics.FillEllipse(Brushes.Green, bowl1);
            e.Graphics.FillEllipse(Brushes.Blue, bowl2);
        }
        public bool coll = false;
        private void tmrAnimate_Tick(object sender, EventArgs e)
        {
            
                if (bowlA.X < bowlAV)
                {
                    if (!coll)
                    {
                        bowlA.X = bowlA.X + 2;
                        bowlAV--;
                        bowlA.Y -= (float)newY1;
                        if (bowlBV > 0 && !coll)
                        {
                            bowlB.X = bowlB.X + 2;
                            bowlBV--;
                            bowlB.Y += (float)newY1;
                        }
                    }
                    else
                    {
                    float b1X = bowlA.X + (bowlW / 2);
                    float b1Y = bowlA.Y + (bowlW / 2);
                    float b2X = bowlB.X + (bowlW / 2);
                    float b2Y = bowlB.Y + (bowlW / 2);
                    newDirect(b1X, b1Y, b2X, b2Y);
                        bowlAV--;
                        bowlA.X = bowlA.X + 2;
                        bowlA.Y -= (float)newY1;
                        bowlB.X = bowlB.X + 2;
                        bowlBV--;
                        bowlB.Y += (float)newY1;
                        coll = false;
                    once = true;
                    }

                }
                else
                {
                    tmrAnimate.Stop();
                }
            
            
            bowl2 = new RectangleF(bowlB.X, bowlB.Y, bowlW, bowlW);
            bowl1 = new RectangleF(bowlA.X, bowlA.Y, bowlW, bowlW);
            //if (bowl1.IntersectsWith(bowl2))
            //{
            //    tmrAnimate.Stop();
            //}
            if (circleCollide(bowl1.X, bowl1.Y, bowl2.X, bowl2.Y, bowlW /2, bowlW / 2) && !once)
            {
                coll = true;
            }
            else
            {
                Refresh();
            }
            
        }
        public bool once = false;
        public int newY;
        public bool circleCollide(float _X1, float _Y1, float _X2, float _Y2, int _r1, int _r2)
        {
            return (Math.Sqrt(Math.Pow(Math.Abs(_X1 - _X2), 2) + Math.Pow(Math.Abs(_Y1 - _Y2), 2)) <= (_r1 + _r2));
        }
        public double newY1;
        public void newDirect(float _X1, float _Y1, float _X2, float _Y2)
        {
            double _O = Math.Abs(_X1 - _X2);
            double _A = Math.Abs(_Y1 - _Y2);
            // double _H = Math.Sqrt(Math.Pow((_A), 2)) + Math.Sqrt(Math.Pow((_O), 2));
            // double _LX = Math.Abs(_X1 - _X2) / 2;
            // double _LY = Math.Abs(_Y1 - _Y2) / 2;
            double _toa = Math.Abs(_O / _A);
            double _deg = Math.Atan(_toa) * 180 / Math.PI;
            // _deg = Math.Abs(_deg);
            newY1 = Math.Abs(Math.Tan(_deg) * 2);
            bowlAV = bowlAV / 2;
            bowlBV = bowlAV;
            // newY1 = Math.Abs(newY1 - _Y1);
        }
        
    }
}
