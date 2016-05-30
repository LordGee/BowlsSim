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
        public int ditchW=30;// this will be the value of the width for the ditch
        public Region exitButton;
        public Brush exitColour = Brushes.DarkRed;
        public GraphicsPath pth;
        public Region optionsButtons;
        public Brush optionColour = Brushes.DarkBlue;
        public Random ran = new Random();
        public SizeF bSize = new SizeF(50, 50);
        public float pow = 0;
        public const float startX = 100;
        public const float startY = 300;
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
        public int powerSpeed = 10;
        public Brush p2Colour = Brushes.DarkRed;
        public Brush p1Colour = Brushes.DarkBlue;
        

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
        }
        theBowls newbowl = new theBowls();
        List<theBowls> Bowls = new List<theBowls>(); // generates the list of the dynamic class

        private void frmMainGame_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
             Font ff = new Font("resources/Comfortaa-Regular.ttf", fs, FontStyle.Bold); // defines the font style for the graphic text used // No longer needed as it's now in its own function
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
                    e.Graphics.FillEllipse(Brushes.Blue, _b.bowl);
                }
                else if (_b.play == 2)
                {
                    e.Graphics.FillEllipse(Brushes.Red, _b.bowl);
                }
            }
            
        }

        private void frmMainGame_Load(object sender, EventArgs e)
        {
            screenSize(); // executes function to find out the form width and height at launch
            // customFont(); // executes function that adds a custom font family to be used regardless of whether the user has it installed or not
            drawButtons(); // draws regions for generic layout
            
            drawMat(); // defines the mat region that is drawn to the canvas
            powerTime.Interval = powerInterval;
            powerTime.Start();
            
            
            
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
        
    
        private void frmMainGame_MouseMove(object sender, MouseEventArgs e)
        {   
            /*
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
            */
        }

        private void frmMainGame_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && exitButton.IsVisible(e.Location))
            {
                Application.Exit();
            }
            else if (e.Button == MouseButtons.Left && optionsButtons.IsVisible(e.Location))
            {
               
            }
        }
        public void pickColor()
            {
        
        
            }    

        private void powerTime_Tick(object sender, EventArgs e)
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
                    powerCount += powerSpeed;
                }
                else
                {
                    powerCount -= powerSpeed;
                }
                Rectangle pbFront = new Rectangle(0, bannerHeight + gameHeight, powerCount, 30);
                pth = new GraphicsPath();
                pth.AddRectangle(pbFront);
                pbF = new Region(pth);
                Refresh();
        }

        public void screenSize()
        {
            screenWidth = this.ClientSize.Width; // populates variable screenWidth with the actual current form width
            screenHeight = this.ClientSize.Height; // populates variable screenHeight with the actual current form height
            // the next two items need to be loaded before the paint event //
            gameHeight = (screenWidth / 7) + 200; // calculation to work out the game area // increased from the design to 200 from 100 to make the game area bigger then the banners
            bannerHeight = (screenHeight - gameHeight) / 2; // the height of the header and footer banners
        }

        public void customFont()
        {
            PrivateFontCollection pfc = new PrivateFontCollection(); // Creates a new instance of a Private Font Collection
            pfc.AddFontFile("resources/Comfortaa-bold.ttf"); // Adds a font held in the application resources into the collection pfc
            ff = new Font(pfc.Families[0], fs, FontStyle.Bold); // defines the variable ff with the font family, size and style
        }

        public bool circleCollide(float _X1, float _Y1, float _X2, float _Y2, int _r1, int _r2)
        {
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
    }
}
