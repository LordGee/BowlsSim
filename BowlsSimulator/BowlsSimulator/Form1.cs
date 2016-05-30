using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // Needed for Graphics Paths
using System.Drawing.Text; //Needed to add a Private Font Collection class
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; // use for creating seperate theads of functions.



namespace BowlsSimulator
{
    public partial class frmMainGame : Form
    {
        public frmMainGame()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        // Public Variables Go Here
        public int fs = 36; // Define the size of the font used
        public int screenWidth, screenHeight; // Varable to hold the size of the form maximised on load
        public int bannerHeight, gameHeight; // Variables to gold the height of the game area and the heaight for the header and footers.
        public Font ff; // Font variable that holds the main font that will be used throughout the application
        public int ditchW;// this will be the value of the width for the ditch
        public Region exitButton;
        public Brush exitColour = Brushes.DarkRed;
        public GraphicsPath pth;
        public Region optionsButtons;
        public Brush optionColour = Brushes.DarkBlue;
        public Random ran = new Random();
        public SizeF bSize = new SizeF(30, 30);
        public float pow = 0;
        public float startX = 100;
        public float startY = 300;
        public GraphicsPath mpth;
        public Region oMat, iMat;
        public int thisPlayer = 1;
        public double passY;
        public Region pbB, pbF;
        public int powerCount = 0;
        public bool powerTest = false;
        public bool testExitColour;
        public bool testOptionColour;
        public Brush continueColour = Brushes.DarkGreen;
        public bool launch = true;
        public int powerInterval = 30;
        public int gameSpeed = 5;
        public Brush p2Colour = Brushes.DarkRed;
        public Brush p1Colour = Brushes.DarkBlue;
        public bool game = true; // starts the game
        public bool crossHair = false; // if this is true wil redraw graphic
        public Region xHair1, xHair2; // define the cross hair regions
        public float selectedY, selectedP; // temp storage for the Y co-ord and P Power Count
        public float centerY;
        public RectangleF jack;
        public float jPower;
        public double jNewY;
        public int jColl;
        public int xHairY;
        public bool powerClick = true;
        public bool xHairClick, xHairTest;
        public bool bowlConfirm;

        class theBowls // The main dynamic class
        {
            public int play;
            public RectangleF bowl;
            public float power;
            public float power30;
            public double newY;
            public double startY;
            public int coll;
            public double bias;
            public int end;
        }
        theBowls newbowl = new theBowls();
        List<theBowls> Bowls = new List<theBowls>(); // generates the list of the dynamic class

        private void frmMainGame_Load(object sender, EventArgs e)
        {
            screenSize(); // executes function to find out the form width and height at launch
            // customFont(); // executes function that adds a custom font family to be used regardless of whether the user has it installed or not
            drawButtons(); // draws regions for generic layout
            drawJack(); // draws the jack to the screen
            drawMat(); // defines the mat region that is drawn to the canvas
            gameLoop(); // starts the game controller
        }

        public void screenSize()
        {
            screenWidth = this.ClientSize.Width; // populates variable screenWidth with the actual current form width
            screenHeight = this.ClientSize.Height; // populates variable screenHeight with the actual current form height
            // the next three items need to be loaded before the paint event //
            centerY = screenHeight / 2;
            gameHeight = (screenWidth / 7) + 200; // calculation to work out the game area // increased from the design to 200 from 100 to make the game area bigger then the banners
            bannerHeight = (screenHeight - gameHeight) / 2; // the height of the header and footer banners
            ditchW = screenWidth * 3 / 100;
            startX = ditchW * 4;
            startY = centerY;
        }

        public void customFont()
        {
            PrivateFontCollection pfc = new PrivateFontCollection(); // Creates a new instance of a Private Font Collection
            pfc.AddFontFile("resources/Comfortaa-bold.ttf"); // Adds a font held in the application resources into the collection pfc
            ff = new Font(pfc.Families[0], fs, FontStyle.Bold); // defines the variable ff with the font family, size and style
        }

        public void drawButtons()
        {
            Rectangle exitR = new Rectangle((screenWidth - 300), (screenHeight - (bannerHeight / 2) - (fs / 2)), 250, 50);
            pth = new GraphicsPath();
            pth.AddRectangle(exitR);
            exitButton = new Region(pth);
            pth.ClearMarkers();
            Rectangle optionR = new Rectangle(50, (screenHeight - (bannerHeight / 2) - (fs / 2)), 250, 60);
            pth.AddRectangle(optionR);
            optionsButtons = new Region(pth);
        }

        public void drawMat()
        {
            Rectangle outerMat = new Rectangle(ditchW * 2, gameHeight + bannerHeight - (gameHeight / 2) - 50, 200, 100);
            pth = new GraphicsPath();
            pth.AddRectangle(outerMat);
            oMat = new Region(pth);

            Rectangle innerMat = new Rectangle(ditchW * 2 + 10, gameHeight + bannerHeight - (gameHeight / 2) - 50 + 10, 180, 80);
            pth = new GraphicsPath();
            pth.AddRectangle(innerMat);
            iMat = new Region(pth);

        }
        public void drawJack()
        {
            jack = new RectangleF(screenWidth - (ditchW * 6), centerY, 15, 15);
        }
        
        private void frmMainGame_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Font ff = new Font("resources/Comfortaa-Regular.ttf", fs, FontStyle.Bold); // defines the font style for the graphic text used // No longer needed as it's now in its own function
            g.FillRectangle(Brushes.DarkGreen, 0, 0, screenWidth, screenHeight);
            g.FillRectangle(Brushes.LightGreen, 0, 0, screenWidth, bannerHeight); // draw the header banner
            g.FillRectangle(Brushes.LightGreen, 0, bannerHeight + gameHeight, screenWidth, bannerHeight); // draw the footer banner
            g.FillRectangle(Brushes.Brown, 0, bannerHeight, ditchW, gameHeight); // draw the left ditch
            g.FillRectangle(Brushes.Brown, screenWidth - ditchW, bannerHeight, ditchW, gameHeight); // draw the right ditch
            g.FillRectangle(Brushes.PeachPuff, 0, bannerHeight + gameHeight, screenWidth, 30); // draw bar
            g.DrawString("Options", ff, optionColour, new Point(50, (screenHeight - (bannerHeight / 2)) - (fs / 2))); // draw the options button
            g.DrawString("Exit Game", ff, exitColour, new Point((screenWidth - 300), (screenHeight - bannerHeight / 2) - (fs / 2))); // draw the exit button   
            //g.DrawString("Two Player Game", ff, twoplayerColour, new Point((screenWidth - 900), (screenHeight / 2) - (fs / 2))); //  draw option menu button
            g.FillRegion(Brushes.Transparent, exitButton);
            g.FillRegion(Brushes.Transparent, optionsButtons);
            if (oMat != null)
            {
                g.FillRegion(Brushes.White, oMat);
                g.FillRegion(Brushes.Black, iMat);
                
            }
            if (pbF != null)
            {
                g.FillRegion(Brushes.LightSkyBlue, pbF);
            }
            
            foreach (theBowls _b in Bowls)
            {
                if (_b.play == 1)
                {
                    g.FillEllipse(p1Colour, _b.bowl);
                }
                else if (_b.play == 2)
                {
                    g.FillEllipse(p2Colour, _b.bowl);
                }
            }
            g.FillEllipse(Brushes.White, jack);
            if (crossHair && !xHairClick && xHair1 != null)
            {
                g.FillRegion(Brushes.Yellow, xHair1);
                g.FillRegion(Brushes.Yellow, xHair2);
            }
        }

        private void frmMainGame_MouseMove(object sender, MouseEventArgs e)
        {
            if (exitButton.IsVisible(e.Location) && !testExitColour)
            { // if the mouse cursor hovers over the exit button change the colour to Gold
                exitColour = Brushes.Gold;
                Refresh();
                testExitColour = true;

            }
            else if (testExitColour && !exitButton.IsVisible(e.Location))
            { // if it's anywhere else change it back to Red
                exitColour = Brushes.DarkRed;
                Refresh();
                testExitColour = false;
            }

            if (optionsButtons.IsVisible(e.Location) && !testOptionColour)
            {
                optionColour = Brushes.Gold;
                Refresh();
                testOptionColour = true;
            }
            else if (testOptionColour && !optionsButtons.IsVisible(e.Location))
            {
                optionColour = Brushes.DarkBlue;
                Refresh();
                testOptionColour = false;
            }
        }

        private void frmMainGame_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && exitButton.IsVisible(e.Location))// setting up an exit button
            {
                Application.Exit();
            }
            else if (e.Button == MouseButtons.Left && optionsButtons.IsVisible(e.Location)) // setting up an option button
            {
                DialogResult dr = MessageBox.Show("Do you want to Change the colour of player bowls?\n\nClick YES for Player 1\n\nClick NO for Player 2", "Change player colour", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    pickColor(p1Colour);
                }
                else if (dr == DialogResult.No)
                {
                    pickColor(p2Colour);
                }
            }
            else if (e.Button == MouseButtons.Left && oMat.IsVisible(e.Location))
            {
                if (!xHairClick)
                {
                    selectedY = xHairY;
                    xHairClick = true;
                    powerClick = false;
                    xHairTime.Stop();
                    startPowerBar();
                }
                else if (!powerClick && xHairClick)
                {
                    selectedP = powerCount;
                    powerClick = true;
                    powerTime.Stop();
                    startBowl();
                }
            }
        }

        public void pickColor(Brush _b)// setting up a funtion to pick up the colour
        {
            if (playerColour.ShowDialog() == DialogResult.OK)
            {
                _b = new SolidBrush(playerColour.Color);
            }
        }

        public void gameLoop()
        {
            if (game)
            {
                startCrossHair();
            }
        }

        public void startCrossHair()
        {
            xHairY = bannerHeight + (gameHeight / 2);
            xHairClick = false;
            xHairTime.Start();
        }

        private void xHairTime_Tick(object sender, EventArgs e)
        {
            if (!xHairClick)
            {
                crossHair = true;
                if (xHairY > bannerHeight + gameHeight - 45)
                {
                    xHairTest = true;
                }
                else if (xHairY < bannerHeight + 15)
                {
                    xHairTest = false;
                }
                if (!xHairTest)
                {
                    xHairY += gameSpeed;
                }
                else
                {
                    xHairY -= gameSpeed;
                }
                Rectangle hair1 = new Rectangle(screenWidth / 2, xHairY, 2, 30);
                Rectangle hair2 = new Rectangle((screenWidth / 2) - 14, xHairY + 14, 30, 2);
                pth = new GraphicsPath();
                pth.AddRectangle(hair1);
                xHair1 = new Region(pth);// adding the path to the region object
                pth = new GraphicsPath();
                pth.AddRectangle(hair2);
                xHair2 = new Region(pth);
                Refresh();
            }
        }

        public void startPowerBar()
        {
            powerClick = false;
            powerTime.Start();
        }

        private void powerTime_Tick(object sender, EventArgs e)
        {
            if (!powerClick)
            {
                if (powerCount > ClientSize.Width)
                {
                    powerTest = true;
                }
                else if (powerCount < 10)
                {
                    powerTest = false;
                }
                if (!powerTest)
                {
                    powerCount += gameSpeed;
                }
                else
                {
                    powerCount -= gameSpeed;
                }
                Rectangle pbFront = new Rectangle(0, bannerHeight + gameHeight, powerCount, 30);
                pth = new GraphicsPath();
                pth.AddRectangle(pbFront);
                pbF = new Region(pth);
                Refresh();
            }
        }

        public void startBowl()
        {
            calcBowl();
            PointF bowlXY = new PointF(startX, startY);
            drawNewBowl(bowlXY);
            bowlConfirm = true;
            bowlTime.Start();
        }

        public void calcBowl()
        {
            double _A = startX - (screenWidth / 2);
            double _O = startY - selectedY;
            double _toa = _O / _A;
            double _deg = Math.Atan(_toa);
            passY = Math.Tan(_deg) * 2;
        }

        public void drawNewBowl(PointF _xy)
        {
            newbowl = new theBowls();
            newbowl.bowl = new RectangleF(_xy, bSize);
            newbowl.play = thisPlayer;
            newbowl.power = selectedP;
            newbowl.power30 = newbowl.power / 1.5f;
            newbowl.newY = passY;
            newbowl.startY = newbowl.newY;
            newbowl.bias = 0.0;
            Bowls.Add(newbowl);
        }

        private void bowlTime_Tick(object sender, EventArgs e)
        {
            if (bowlConfirm)
            {
                foreach (theBowls _c in Bowls)
                {
                    if (_c.power <= 0)
                    {
                        _c.coll = 0;
                    }
                }
                if (jPower <= 0)
                {
                    jColl = 0;
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
                                        _b.newY = passY;
                                        _b2.newY = passY;
                                        _b.coll = 1;
                                        _b2.coll = 2;
                                    }
                                }
                            }
                        }
                        if (circleCollide(_b.bowl.X, _b.bowl.Y, jack.X, jack.Y, (int)bSize.Height / 2, (int)bSize.Width / 2)) // math provided by Glenn
                        {
                            newDirect(_b.bowl.X, _b.bowl.Y, jack.X, jack.Y, _b.power);
                            _b.power = pow;
                            jPower = pow;
                            _b.newY = passY;
                            jNewY = passY;
                            _b.coll = 1;
                            jColl = 2;
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
                        if (jColl == 2)
                        {
                            jack.X = jack.X + 2;
                            jPower = jPower - 2;
                            jack.Y += (float)jNewY;
                        }
                        else if (jColl == 1)
                        {
                            jack.X = jack.X + 2;
                            jPower = jPower - 2;
                            jack.Y -= (float)jNewY;
                        }
                        else
                        {
                            if (_b.power < 200)
                            {
                                if (_b.startY <= 0.00)
                                {
                                    _b.bias = 0.01;
                                }
                                else
                                {
                                    _b.bias = -0.01;
                                }
                            }
                            else if (_b.power < _b.power30)
                            {
                                if (_b.startY <= 0.00)
                                {
                                    _b.bias = 0.002;
                                }
                                else
                                {
                                    _b.bias = -0.005;
                                }
                            }
                            _b.newY += _b.bias;
                            _b.bowl.X = _b.bowl.X + 2;
                            _b.power = _b.power - 2;
                            _b.bowl.Y += (float)_b.newY;
                        }
                        if (_b.bowl.X > screenWidth - ditchW) // testing a ditch effect
                        {
                            _b.bowl.Size = new SizeF(20, 20);
                            _b.power = 0;
                            _b.coll = 2;
                        }
                        if (jack.X > screenWidth - ditchW)
                        {
                            jack.Size = new SizeF(8, 8);
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
                }
            }
        }

        public bool circleCollide(float _X1, float _Y1, float _X2, float _Y2, int _r1, int _r2)
        { // the following code was provided by Glenn
            return (Math.Sqrt(Math.Pow(Math.Abs(_X1 - _X2), 2) + Math.Pow(Math.Abs(_Y1 - _Y2), 2)) <= (_r1 + _r2));
        }

        public void newDirect(float _X1, float _Y1, float _X2, float _Y2, float _p)
        {
            double _A = _X1 - _X2;
            double _O = _Y1 - _Y2;
            double _toa = _O / _A;
            double _deg = Math.Atan(_toa);
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
        }

        private void gameLoopy_Tick(object sender, EventArgs e)
        {
            // redundent, this didn't quite work the way I hoped
        }
    }
}
