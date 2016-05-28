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

namespace CaruseAndEffectRegions
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Brush p1c = Brushes.Blue;
        public Brush p2c = Brushes.Red;
        public Random ran = new Random();
        public SizeF bSize = new SizeF(50, 50);
        public float pow = 0;
        const float startX = 100;
        const float startY = 600;
        class theBowls
        {
            public int play;
            public RectangleF bowl;
            public float power;
        }
        theBowls newbowl = new theBowls();
        List<theBowls> Bowls = new List<theBowls>();

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (theBowls _b in Bowls)
            {
                if (_b.play == 1)
                {
                    e.Graphics.FillEllipse(Brushes.Blue, _b.bowl);
                }
                else if (_b.play == 2)
                {
                    e.Graphics.FillEllipse(Brushes.Red, _b.bowl);
                }
            }
        }
        public bool collide = false;
        public int thisPlayer = 1;
        private void tmr_Tick(object sender, EventArgs e)
        {
            PointF bowlXY = new PointF(startX, startY);
            drawNewBowl(bowlXY);
            foreach (theBowls _b in Bowls)
            {
                while (_b.power > 0)
                {
                    if (!collide)
                    {
                        _b.bowl.X = _b.bowl.X + 2;
                        _b.power--;
                        _b.bowl.Y -= (float)newY;
                        Refresh();
                    }
                    foreach (theBowls _b2 in Bowls)
                    {
                        if (_b != _b2)
                        {
                        if (circleCollide(_b.bowl.X, _b.bowl.Y, _b2.bowl.X, _b2.bowl.Y, (int)bSize.Height / 2, (int)bSize.Width / 2))
                        {
                            collide = true;
                            newDirect(_b.bowl.X, _b.bowl.Y, _b2.bowl.X, _b2.bowl.Y, _b.power);
                            _b.power = pow;
                            _b2.power = pow;
                            while (_b2.power > 0)
                            {
                                _b2.bowl.X = _b2.bowl.X + 2;
                                _b2.bowl.Y -= (float)newY;
                                _b2.power--;
                                _b.bowl.X = _b.bowl.X + 2;
                                _b.bowl.Y -= (float)newY;
                                _b.power--;
                                Refresh();
                            }
                        }
                        }
                    }
                }
            }
            if (thisPlayer == 1)
            {
                thisPlayer = 2;
            } 
            else
            {
                thisPlayer = 1;
            }
        }
        public void drawNewBowl(PointF _xy)
        {
            newbowl = new theBowls();
            newbowl.bowl = new RectangleF(_xy, bSize);
            newbowl.play = thisPlayer;
            newbowl.power = ran.Next(500, 1000);
            Bowls.Add(newbowl);
        }

        public bool circleCollide(float _X1, float _Y1, float _X2, float _Y2, int _r1, int _r2)
        {
            return (Math.Sqrt(Math.Pow(Math.Abs(_X1 - _X2), 2) + Math.Pow(Math.Abs(_Y1 - _Y2), 2)) <= (_r1 + _r2));
        }
        public double newY;
        
        public void newDirect(float _X1, float _Y1, float _X2, float _Y2, float _p)
        {
            double _A = _X1 - _X2;
            double _O = _Y1 - _Y2;
            double _toa = _O / _A;
            double _deg = Math.Atan(_toa)/* * 180 / Math.PI*/;
            newY = Math.Tan(_deg) * 2;
            pow = _p / 2;

            // double _H = Math.Sqrt(Math.Pow((_A), 2)) + Math.Sqrt(Math.Pow((_O), 2));
            // double _LX = Math.Abs(_X1 - _X2) / 2;
            // double _LY = Math.Abs(_Y1 - _Y2) / 2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmr.Start();
        }
    }
}
