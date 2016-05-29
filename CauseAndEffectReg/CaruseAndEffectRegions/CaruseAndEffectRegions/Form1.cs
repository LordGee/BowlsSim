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
        public Random ran = new Random();
        public SizeF bSize = new SizeF(50, 50);
        public float pow = 0;
        const float startX = 100;
        const float startY = 500;
        class theBowls
        {
            public int play;
            public RectangleF bowl;
            public float power;
            public double newY;
            public double startY;
            public int coll;
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
            foreach (theBowls _c in Bowls)
            {
                if (_c.power <= 0)
                {
                    _c.coll = 0;
                }
                
            }
            foreach (theBowls _b in Bowls)
            {
                if (_b.power > 0)
                {
                    foreach (theBowls _b2 in Bowls)
                    {
                        if (_b != _b2 /*&& _b.coll == 0*/)
                        {
                            if (circleCollide(_b.bowl.X, _b.bowl.Y, _b2.bowl.X, _b2.bowl.Y, (int)bSize.Height / 2, (int)bSize.Width / 2)) // math provided by Glenn
                            {
                                if (_b.coll == 0 || _b2.coll == 0)
                                {
                                    newDirect(_b.bowl.X, _b.bowl.Y, _b2.bowl.X, _b2.bowl.Y, _b.power);
                                    _b.power = pow;
                                    _b2.power = pow;
                                    _b.newY = 0;
                                    _b2.newY = 0;
                                    _b.newY = passY;
                                    _b2.newY = passY;
                                    _b.coll = 1;
                                    _b2.coll = 2;
                                }
                            }
                        }
                    }
                    if (_b.coll == 2)
                    {
                        _b.bowl.X = _b.bowl.X + 2;
                        _b.power = _b.power - 2;
                        _b.bowl.Y += (float)_b.newY;
                    }
                    else if (_b.coll == 1)
                    {
                        _b.bowl.X = _b.bowl.X + 2;
                        _b.power = _b.power - 2;
                        _b.bowl.Y -= (float)_b.newY;
                    }
                    else
                    {
                        if (_b.startY <= 0)
                        {
                            _b.newY = _b.newY + 0.005;
                        }
                        else if (_b.startY > 0)
                        {
                            _b.newY = _b.newY - 0.005;
                        }
                        _b.bowl.X = _b.bowl.X + 2;
                        _b.power = _b.power - 2;
                        _b.bowl.Y += (float)_b.newY;
                    }
                    Refresh();
                }
            }
            int c = 0;
            int l = Bowls.Count();
            foreach (theBowls _t in Bowls)
            {
                if (_t.power <= 0)
                {
                    c++;
                }
            }
            if (c == l)
            {
                if (thisPlayer == 1)
                {
                    thisPlayer = 2;
                }
                else
                {
                    thisPlayer = 1;
                }
                PointF bowlXY = new PointF(startX, 500);
                drawNewBowl(bowlXY);
            }

        }
        public void drawNewBowl(PointF _xy)
        {
            newbowl = new theBowls();
            newbowl.bowl = new RectangleF(_xy, bSize);
            newbowl.play = thisPlayer;
            newbowl.power = ran.Next(500, 1000);
            newbowl.newY = ran.NextDouble() * 2 - 1;
            newbowl.startY = newbowl.newY;
            Bowls.Add(newbowl);
        }

        public bool circleCollide(float _X1, float _Y1, float _X2, float _Y2, int _r1, int _r2)
        {
            return (Math.Sqrt(Math.Pow(Math.Abs(_X1 - _X2), 2) + Math.Pow(Math.Abs(_Y1 - _Y2), 2)) <= (_r1 + _r2));
        }
        public double passY;
        
        public void newDirect(float _X1, float _Y1, float _X2, float _Y2, float _p)
        {
            double _A = _X1 - _X2;
            double _O = _Y1 - _Y2;
            double _toa = _O / _A;
            double _deg = Math.Atan(_toa)/* * 180 / Math.PI*/;
            passY = Math.Tan(_deg) * 2;
            pow = _p / 2;

            // double _H = Math.Sqrt(Math.Pow((_A), 2)) + Math.Sqrt(Math.Pow((_O), 2));
            // double _LX = Math.Abs(_X1 - _X2) / 2;
            // double _LY = Math.Abs(_Y1 - _Y2) / 2;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            gameLoop();
        }
        public void gameLoop()
        {
            thisPlayer = 2;
            tmr.Start();
        }
    }
}
