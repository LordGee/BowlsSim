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
        const float startY = 300;
        GraphicsPath pth;
        Region oMat, iMat;

        class theBowls
        {
            public int play;
            public RectangleF bowl;
            public float power;
            public float power30;
            public double newY;
            public double startY;
            public int coll;
            public double bias;
        }
        theBowls newbowl = new theBowls();
        List<theBowls> Bowls = new List<theBowls>();

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRegion(Brushes.White, oMat);
            e.Graphics.FillRegion(Brushes.Black, iMat);
            if (pbB != null)
            {
                e.Graphics.FillRegion(Brushes.PaleGoldenrod, pbB);
                e.Graphics.FillRegion(Brushes.IndianRed, pbF);
            }
            
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
        //public bool collide = false;
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
                                    //_b.newY = 0;
                                    //_b2.newY = 0;
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
                        
                        if (_b.power < 400)
                        {
                             if (_b.startY <= 0.00)
                            {
                                _b.bias = 0.014;
                            }
                            else
                            {
                                _b.bias = -0.014;
                            }
                        }
                        else if (_b.power < _b.power30)
                        {
                             // Currently working on a calc the will make these if statements consistant
                            if (_b.startY <= 0.00)
                            {
                                _b.bias = 0.005;
                            }
                            else
                            {
                                _b.bias = -0.005;
                            }
                        }
                        //if (_b.power < _b.power30) // to be continued
                        //{
                        //    float dtt = 200;
                        //    float tic = _b.power * 2;
                        //    float res = (dtt / tic) / 2;
                        //    if (_b.startY <= 0.00)
                        //    {
                        //        _b.bias = res;
                        //    }
                        //    else
                        //    {
                        //        _b.bias = -res;
                        //    }
                        //}
                        _b.newY += _b.bias;
                        _b.bowl.X = _b.bowl.X + 2;
                        _b.power = _b.power - 2;
                        _b.bowl.Y += (float)_b.newY;
                    }
                    if (_b.bowl.X > 1000) // testing a ditch effect
                    {
                        _b.bowl.Size = new SizeF(20, 20);
                        _b.power = 0;
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
                PointF bowlXY = new PointF(startX, startY);
                drawNewBowl(bowlXY);
                powerTmr.Start();
            }

        }
        public void drawNewBowl(PointF _xy)
        {
            newbowl = new theBowls();
            newbowl.bowl = new RectangleF(_xy, bSize);
            newbowl.play = thisPlayer;
            newbowl.power = ran.Next(500, 1000);
            newbowl.power30 = newbowl.power / 3;
            newbowl.newY = 0.5;// ran.NextDouble() * 2 - 1;
            newbowl.startY = newbowl.newY;
            newbowl.bias =  0.0;
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
            if (passY > 4)
            {
                passY = 4;
            }
            else if (passY < -4)
            {
                passY = -4;
            }
            pow = _p / 2.5f;

            // double _H = Math.Sqrt(Math.Pow((_A), 2)) + Math.Sqrt(Math.Pow((_O), 2));
            // double _LX = Math.Abs(_X1 - _X2) / 2;
            // double _LY = Math.Abs(_Y1 - _Y2) / 2;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            drawMat();
            gameLoop();
        }
        public void gameLoop()
        {
            thisPlayer = 2;
            tmr.Start();
        }
        Region pbB, pbF;
        int count = 0;
        bool test = false;
        private void powerTmr_Tick(object sender, EventArgs e)
        {
            Rectangle pbBack = new Rectangle(0,0,this.ClientSize.Width,50);
            pth = new GraphicsPath();
            pth.AddRectangle(pbBack);
            pbB = new Region(pth);
            
            if (count > this.ClientSize.Width)
            {
                test = true;
                count -= 3;
            }
            else if (count < 10)
            {
                test = false;
                count += 3;
            }
            if (!test)
            {
                count += 3;
            }
            else
            {
                count -= 3;
            }
            
            Rectangle pbFront = new Rectangle(0, 0, count, 50);
            pth = new GraphicsPath();
            pth.AddRectangle(pbFront);
            pbF = new Region(pth);
            Refresh();
        }

        public void drawMat()
        {
            Rectangle outerMat = new Rectangle((int)startX - 50, (int)startY - (100 / 2), 200, 100);
            pth = new GraphicsPath();
            pth.AddRectangle(outerMat);
            oMat = new Region(pth);

            Rectangle innerMat = new Rectangle((int)startX - 40, (int)startY - (100 / 2) + 10, 180, 80);
            pth = new GraphicsPath();
            pth.AddRectangle(innerMat);
            iMat = new Region(pth);
        }
    }
}
