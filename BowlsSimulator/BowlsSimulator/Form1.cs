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
        // Numerical Values
        public const int bowlSpeed = 3; // defines the speed the bowl travels
        public const int gameSpeed = 6; // defines the speed of the cross hair and powerbar
        public int fs = 36; // Define the size of the font used
        public int screenWidth, screenHeight; // Varable to hold the size of the form maximised on load
        public int bannerHeight, gameHeight; // Variables to hold the height of the game area and the height for the header and footers.
        public int ditchW;// this will be the value of the width for the ditch
        public int bs = 30; // references the size of the Bowl
        public int thisPlayer = 1; // defines the current player
        public int powerCount = 0; // indicates the value of where the power bar X co-ord is
        public int js = 15; // references the size of the Jack
        public int jColl; // sets a collision value when the jack is struck by another object
        public int xHairY; // indicates the value of where the cross hair Y co-ord is
        public int p1score, p2score, p1EndScore, p2EndScore, currEnd, currBowl, gameNo; // current game statistics
        public int cpuHairY, cpuPower; // indicates the value of the CPU Crosshair and Power Bar
        public float pow = 0; // Temp storage of the power value
        public float startX = 100; // Indicates the starting X value of the bowl
        public float startY = 300; // indicates the starting Y value of the bowl
        public float selectedY, selectedP; // temp storage for the Y co-ord and P Power Count
        public float centerY; // holds the value for the centre of the screen
        public float jPower; // after collision the jack now has power
        public double jNewY; // indicates the Jacks new Y co-ord after collision
        public double passY; // Temp storage of the new Y value after collision has been detected
        public Random ran = new Random(); // generates a random number, used for testing and computer play
        // Graphics
        public Font ff; // Font variable that holds the main font that will be used throughout the application
        public Brush exitColour = Brushes.DarkRed; // the colour of the exit button
        public Brush optionColour = Brushes.DarkBlue; // the colour of the Options Button
        public Brush matColour = Brushes.Black; // the colour of the inner mat, this will change colour depending on what player is playing
        public Brush p1Colour = Brushes.DarkBlue; // defines player one colour, this can be changed in options
        public Brush p2Colour = Brushes.DarkRed; // defines player two colour, this can be changed in options
        public Region exitButton; // defines the location of the exit button so a mouse click can be detected
        public Region optionsButtons; // defines the location of the options button so a mouse click can be detected
        public Region oMat, iMat; // defines the location of the inner and outer mat
        public Region pbB, pbF; // befines the two layers of the power bar
        public Region xHair1, xHair2; // define the cross hair regions
        public RectangleF jack; // defines the shape and location of the Jack
        public GraphicsPath pth; // defines the path for the appropriate regions
        public GraphicsPath mpth; // same as above
        public SizeF bSize = new SizeF(30, 30);
        // Booleans
        public bool testExitColour, testOptionColour; // Indicates if the colour has been changed already
        public bool game = true; // starts the game
        public bool showOnce; // ensures a dialog is only shown once per end
        public bool crossHair = false; // if this is true wil redraw graphic
        public bool powerTest = false; // determines if a click is waiting to be detected
        public bool powerClick = true; // determines if a click is waiting to be detected
        public bool xHairClick, xHairTest; // determins if a click is waiting to be detected
        public bool bowlConfirm; // sets the bowl ready to be drawn to screen
        public bool cpu, allCpu; // Defines if at least one of the players is a computer playerComputer control

        class theBowls // The main dynamic class
        {
            public int play; // stores the player number
            public RectangleF bowl; // stores the position of the bowl
            public float power; // holds the power value which decreases as it travels
            public float power30; // calculates the new power value after collision
            public double newY; // Calculate new Y co-ord after a collision
            public double startY; // Holds the previous Y co-ord for calculating collisions and the bias movement
            public int coll; // indicates if a collision which is the front bowls and which is the back, reset to 0 after each tick
            public double bias; // Holds the value of the bias decrease / increase
            public int end; // stores the end the bowl was played
            public double distance; // calculates the distance between the jack and the bowls
            public int shot; // holds a calculation of the bowl thats closest (6,4,2,0) 6 == closest
        }
        theBowls newbowl = new theBowls(); // initiates an instance to add a new object to the class
        List<theBowls> Bowls = new List<theBowls>(); // generates the list of the dynamic class

        private void frmMainGame_Load(object sender, EventArgs e)
        {
            screenSize(); // executes function to find out the form width and height at launch
            // customFont(); // defunct: worked well to start but now crashes the application // executes function that adds a custom font family to be used regardless of whether the user has it installed or not
            drawButtons(); // draws regions for generic layout
            drawJack(); // draws the jack to the screen
            drawMat(); // defines the mat region that is drawn to the canvas
            // display instruction and start or exit game
            DialogResult startBox = MessageBox.Show("Would you like to start a new game?\n\n\nInstructions\n\n1. When the game starts a crosshair will appear this determines the direction of play.\n2. Click the black and white mat to stop the crosshair.\n3. The power bar will start moving right and left across the screen, this will determine how far your bowl will travel\n4. Click the black and white mat to stop the Power Bar\n5. Your bowl will be played, and the next player takes their turn. Once all bowls are played (2 each) the closest bowl scores 1 point, if that same player has the second closest bowl then that player will score 2 points.\n\n The game ends with the first player to achieve 7 points.\n\n\n GOOD LUCK", "New Game?", MessageBoxButtons.YesNo);
            if (startBox == DialogResult.Yes)
            {
                newGame(); // starts and resets the game controller
            }
            else
            {
                Application.Exit(); // exit game
            }
        }

        public void screenSize()
        {
            screenWidth = this.ClientSize.Width; // populates variable screenWidth with the actual current form width
            screenHeight = this.ClientSize.Height; // populates variable screenHeight with the actual current form height
            // the next three items need to be loaded before the paint event //
            centerY = screenHeight / 2;
            gameHeight = (screenWidth / 7) + 200; // calculation to work out the game area // increased from the design to 200 from 100 to make the game area bigger then the banners
            bannerHeight = (screenHeight - gameHeight) / 2; // the height of the header and footer banners
            ditchW = screenWidth * 2 / 100; // calculation for the ditch width
            startX = ditchW * 4; // calculates where to start the bowls from
            startY = centerY; // as above
        }

        public void customFont()
        {
            PrivateFontCollection pfc = new PrivateFontCollection(); // Creates a new instance of a Private Font Collection
            pfc.AddFontFile("resources/Comfortaa-bold.ttf"); // Adds a font held in the application resources into the collection pfc
            ff = new Font(pfc.Families[0], fs, FontStyle.Bold); // defines the variable ff with the font family, size and style
        }

        public void drawButtons()
        {
            // the following draws the region for the buttons to be detected from a mouse click
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
            // the following draws the region for the mat to be detected from a mouse click
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
            // draws the jack to a random start position
            int jackX = ran.Next(screenWidth / 2, screenWidth - (ditchW * 6));
            jack = new RectangleF(jackX, centerY, js, js);
        }
        
        private void frmMainGame_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Font ff = new Font("resources/Comfortaa-Regular.ttf", fs, FontStyle.Bold); // defines the font style for the graphic text used // No longer needed as it's now in its own function
                Font ffs = new Font("resources/Comfortaa-Regular.ttf", 20, FontStyle.Bold); // probably not the best name for a variable ffs = Font Family Smaller, not the more commonly used phrase
                g.FillRectangle(Brushes.DarkGreen, 0, 0, screenWidth, screenHeight); // Due to the grassy background hindering performance this is a solid colour the covers the whole screen... and works
                g.FillRectangle(Brushes.Brown, screenWidth - ditchW, bannerHeight, ditchW, gameHeight); // draw the right ditch
                g.FillRectangle(Brushes.Brown, 0, bannerHeight, ditchW, gameHeight); // draw the left ditch
                foreach (theBowls _b in Bowls) // draws any bowls for the current end to the canvas
                {
                    if (_b.end == currEnd)
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
                }
                g.FillEllipse(Brushes.White, jack); // Draws the jack, the jack is drawn after the bowls . If theres a glitch the jack should still be visiable
                g.FillRectangle(Brushes.LightGreen, 0, 0, screenWidth, bannerHeight); // draw the header banner
                g.FillRectangle(Brushes.LightGreen, 0, bannerHeight + gameHeight, screenWidth, bannerHeight); // draw the footer banner
                g.FillRectangle(Brushes.PeachPuff, 0, bannerHeight + gameHeight, screenWidth, 30); // draw bar
                g.DrawString("Options", ff, optionColour, new Point(50, (screenHeight - (bannerHeight / 2)) - (fs / 2))); // draw the options button
                g.DrawString("Exit Game", ff, exitColour, new Point((screenWidth - 300), (screenHeight - bannerHeight / 2) - (fs / 2))); // draw the exit button
                g.DrawString("Player One: " + p1score, ffs, p1Colour, new Point(ditchW, ditchW)); // draw player ones score to the top left
                g.DrawString("Player Two: " + p2score, ffs, p2Colour, new Point(ditchW, ditchW * 3)); // just below player one
                g.DrawString("Now Playing", ffs, Brushes.Black, new Point(screenWidth - 300, ditchW)); // the following presents the current player although now the mat changes colour this can be obsolete
                if (thisPlayer == 1)
                {
                    g.DrawString("Player: ONE", new Font("resources/Comfortaa-Regular.ttf", 20, FontStyle.Bold), p1Colour, new Point(screenWidth - 300, ditchW * 3));
                }
                else if (thisPlayer == 2)
                {
                    g.DrawString("Player: TWO", new Font("resources/Comfortaa-Regular.ttf", 20, FontStyle.Bold), p2Colour, new Point(screenWidth - 300, ditchW * 3));
                }
                //g.DrawString("Two Player Game", ff, twoplayerColour, new Point((screenWidth - 900), (screenHeight / 2) - (fs / 2))); //  draw option menu button
                g.FillRegion(Brushes.Transparent, exitButton); // hidden region, found this easier to detect a mouse click
                g.FillRegion(Brushes.Transparent, optionsButtons); // same as the last one
                if (oMat != null)
                {
                    g.FillRegion(Brushes.White, oMat);
                    g.FillRegion(matColour, iMat);

                }
                if (pbF != null)
                {
                    g.FillRegion(Brushes.DarkViolet, pbF);
                }

                if (crossHair && !xHairClick && xHair1 != null)
                {
                    g.FillRegion(Brushes.Yellow, xHair1);
                    g.FillRegion(Brushes.Yellow, xHair2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kaa-Boom... Something Broke: " + ex.Message, "Explosion Detected");
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
            { // if the mouse cursor hovers over the Options button change the colour to Gold
                optionColour = Brushes.Gold;
                Refresh();
                testOptionColour = true;
            }
            else if (testOptionColour && !optionsButtons.IsVisible(e.Location))
            { // if it's anywhere else change it back to Blue
                optionColour = Brushes.DarkBlue;
                Refresh();
                testOptionColour = false;
            }
        }

        private void frmMainGame_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && exitButton.IsVisible(e.Location))// setting up an exit button
            {
                Application.Exit(); // closes the game
            }
            else if (e.Button == MouseButtons.Left && optionsButtons.IsVisible(e.Location)) // setting up an option button
            { // opens colour picker to change the player colour
                DialogResult dr = MessageBox.Show("Do you want to Change the colour of player bowls?\n\nClick YES for Player 1\n\nClick NO for Player 2", "Change player colour", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    p1Colour = pickColor(p1Colour);
                }
                else if (dr == DialogResult.No)
                {
                    p2Colour = pickColor(p2Colour);
                }
            }
            else if (e.Button == MouseButtons.Left && oMat.IsVisible(e.Location))
            {
                if (cpu && thisPlayer == 2 || allCpu == true)
                {
                    // do nothing // this prevents cheating against the CPU
                }
                else
                {
                    if (!xHairClick) 
                    { // stores the information from the crosshair, starts the powerbar
                        selectedY = xHairY; // stores the crosshair position ready to be drawn
                        xHairClick = true;
                        powerClick = false;
                        xHairTime.Stop(); // stops the crosshair animation
                        startPowerBar(); // starts the powerbar animation
                    }
                    else if (!powerClick && xHairClick)
                    { // stores the information from the power bar and starts the bowl animation
                        selectedP = powerCount; // store the power value ready to be drawn
                        powerClick = true;
                        powerTime.Stop(); // stopps the power bar animation
                        startBowl(); // draws the bowl
                    }
                }
            }
        }

        public Brush pickColor(Brush _b)// setting up a funtion to pick up the colour
        { // overloaded function to change the colour of the passed in player
            playerColour.SolidColorOnly = false;
            if (playerColour.ShowDialog() == DialogResult.OK)
            {
                _b = new SolidBrush(playerColour.Color);
            }
            return _b;
        }

        /// <summary>
        /// This is where the actual game mechanics begins!!!!
        /// </summary>

        public void newGame()
        {
            // reset variables ready for new game
            Bowls.Clear(); // empty the dynamic structure
            cpu = false; // sets no cpu player
            allCpu = false; // sets no cpu players
            p1score = 0; // sets player score to zero
            p2score = 0; // sets player two score to zero
            currEnd = 1; // sets the first end
            currBowl = 1; // sets the first bowl to be played
            gameNo += 1; // obsolete not that the dynamic structure is cleared
            // Asks if the user would like a one or two player game... there is an easter egg here
            DialogResult noPlay = MessageBox.Show("How many players would you like?\n\nClick YES for One Player\nClick NO for Two Players", "Number of Players", MessageBoxButtons.YesNoCancel);
            if (noPlay == DialogResult.Yes)
            {
                cpu = true;
                allCpu = false;
            }
            else if (noPlay == DialogResult.No)
            {
                cpu = false;
                allCpu = false;
            }
            else
            {
                allCpu = true;
            }
            gameLoop(); // start the main game loop
        }

        public void gameLoop()
        {
            if (game) // pointless variable may remove this or do something else with it
            {
                showOnce = true; // shows a dialog once per end
                startCrossHair(); // starts the cross hair animation
            }
        }
        
        public void startCrossHair()
        {
            if (cpu == true && thisPlayer == 2 || allCpu == true)
            { // random value for the computers cross hair position
                cpuHairY = ran.Next(bannerHeight + 15, bannerHeight + gameHeight - 45);
            }
            xHairY = bannerHeight + (gameHeight / 2); // indicates new starting position so it always starts in the middle
            xHairClick = false; // sets a value to indicate that a mouse click is to be expected
            xHairTime.Start(); // starts the animation
        }

        private void xHairTime_Tick(object sender, EventArgs e)
        {
            if (!xHairClick)
            { // the following increases and decreases the animation of the crosshair within the bounds specified
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
                if (cpu == true && thisPlayer == 2 || allCpu == true)
                {
                    if (cpuHairY <= xHairY + gameSpeed && cpuHairY >= xHairY - gameSpeed)
                    { // the following animates the crosshair until the computers random number is achieved
                        selectedY = cpuHairY; // stores the position ready to be drawn
                        xHairClick = true;
                        powerClick = false;
                        xHairTime.Stop(); // stops the crosshair animation
                        startPowerBar(); // starts the power bar animation
                    }
                }
            }
        }
        
        public void startPowerBar()
        {
            if (cpu == true && thisPlayer == 2 || allCpu == true)
            { // random value for the computers power value
                cpuPower = ran.Next((int)jack.X - (int)startX - 100, (int)jack.X - (int)startX + 200);
            }
            powerClick = false;
            powerTime.Start(); // starts the power bar animation
        }

        private void powerTime_Tick(object sender, EventArgs e)
        { // the following increases and decreases the animation of the power bar within the bounds specified
            if (!powerClick)
            {
                if (powerCount > screenWidth)
                {
                    powerTest = true;
                }
                else if (powerCount < gameHeight)
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
                if (cpu == true && thisPlayer == 2 || allCpu == true)
                {
                    if (cpuPower <= powerCount + gameSpeed && cpuPower >= powerCount - gameSpeed)
                    { // the following animates the power bar until the computers random number is achieved
                        selectedP = cpuPower; // stores the computer value ready to be drawn
                        powerClick = true;
                        powerTime.Stop(); // stops powerbar animation
                        startBowl(); // draws the bowl
                    }
                }
            }
        }

        public void startBowl()
        {
            calcBowl(); // calculates the starting bias
            PointF bowlXY = new PointF(startX, startY); // defines the starting position of the bowl
            drawNewBowl(bowlXY); // draws the bowl
            bowlConfirm = true;
            bowlTime.Interval = 10; // Sets the initial bowl time animation, this changes throughout its life span
            bowlTime.Start(); // start the bowl animation
        }

        public void calcBowl()
        {
            double _A = startX - (screenWidth / 2); // calculates the adjacent
            double _O = startY - selectedY; // calculates the opposite
            double _toa = _O / _A; 
            double _deg = Math.Atan(_toa); // calculates the radians
            passY = Math.Tan(_deg) * bowlSpeed; // calculates the new Y co-ords for the next position
        }

        public void drawNewBowl(PointF _xy)
        {
            newbowl = new theBowls(); // initilises a new object in theBowls class
            newbowl.bowl = new RectangleF(_xy, bSize); // adds the starting bowls position and size
            newbowl.play = thisPlayer; // adds the player number the bowl belongs to
            newbowl.power = selectedP; // adds the initial power allocated to this bowl
            newbowl.power30 = newbowl.power / 1.5f; // calculates at what position the bias will come int affect
            newbowl.newY = passY; // adds the next Y co-ord
            newbowl.startY = newbowl.newY; // adds the starting Y co-ord
            newbowl.bias = 0.0; // adds the start bias, this will change later
            newbowl.end = currEnd; // adds the current end
            Bowls.Add(newbowl); // adds the new bowl to the main list
        }

        private void bowlTime_Tick(object sender, EventArgs e)
        {
            if (bowlConfirm)
            {
                foreach (theBowls _c in Bowls)
                { // resets the collision states if the bowl is not in motion any longer
                    if (_c.power <= 0 && _c.end == currEnd &&  _c.coll != 3)
                    {
                        _c.coll = 0;
                    }
                }
                if (jPower <= 0 && jColl != 3)
                { // resets the jack collision if the jack is no longer in motion
                    jColl = 0;
                }
                foreach (theBowls _b in Bowls)
                {
                    if (_b.power > 0 && _b.end == currEnd)
                    { // if the bowl is in motion
                        foreach (theBowls _b2 in Bowls)
                        {
                            if (_b != _b2 && _b2.end == currEnd) // the comparison is not the same object
                            { 
                                if (circleCollide(_b.bowl.X, _b.bowl.Y, _b2.bowl.X, _b2.bowl.Y, (int)bSize.Height / 2, (int)bSize.Width / 2)) // math provided by Glenn
                                { // if two bowls have colided
                                    if (_b.coll == 0 || _b2.coll == 0 && _b2.coll != 3)
                                    { // if both have not previously colided
                                        newDirect(_b.bowl.X, _b.bowl.Y, _b2.bowl.X, _b2.bowl.Y, _b.power); // calculate new direction
                                        _b.power = pow; // add the power after calculation
                                        _b2.power = pow; // add the power after calculation
                                        _b.newY = passY; // add the new next Y co-ord
                                        _b2.newY = passY; // add the new next Y co-ord
                                        _b.coll = 1; // Set new collision status
                                        _b2.coll = 2; // Set new collision status
                                    }
                                }
                            }
                        }
                        if (circleCollide(_b.bowl.X, _b.bowl.Y, jack.X, jack.Y, (int)bSize.Height / 2, (int)bSize.Width / 2)) // math provided by Glenn
                        { // if the bowl colides with the jack
                            newDirect(_b.bowl.X, _b.bowl.Y, jack.X, jack.Y, _b.power); // calculate new direction
                            _b.power = pow; // add the power after calculation
                            jPower = pow; // add the power after calculation
                            _b.newY = passY; // add the new next Y co-ord
                            jNewY = passY; // add the new next Y co-ord
                            _b.coll = 1; // Set new collision status
                            jColl = 2; // Set new collision status
                        }
                        if (_b.coll == 2 && _b.coll != 3)
                        { // animate the Bowl in new direction after collision
                            _b.bowl.X = _b.bowl.X + bowlSpeed;
                            _b.power = _b.power - bowlSpeed;
                            _b.bowl.Y += (float)_b.newY;
                        }
                        else if (_b.coll == 1 && _b.coll != 3)
                        { // animate the Bowl in new direction after collision
                            _b.bowl.X = _b.bowl.X + bowlSpeed;
                            _b.power = _b.power - bowlSpeed;
                            _b.bowl.Y -= (float)_b.newY;
                        }
                        if (jColl == 2 && jColl != 3)
                        { // animate the Jack in new direction after collision
                            jack.X = jack.X + bowlSpeed;
                            jPower = jPower - bowlSpeed;
                            jack.Y += (float)jNewY;
                        }
                        else if (jColl == 1 && jColl != 3)
                        { // animate the Jack in new direction after collision
                            jack.X = jack.X + bowlSpeed;
                            jPower = jPower - bowlSpeed;
                            jack.Y -= (float)jNewY;
                        }
                        else
                        {
                            if (_b.power < 100)
                            { // if the power remaining is less then 100 then reduce or increase the bias depending on the starting Y co-ord
                                bowlTime.Interval = 17;
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
                            { // if the power remaining is less then 1.5x of the original porwer then reduce or increase the bias depending on the starting Y co-ord
                                bowlTime.Interval = 15;
                                if (_b.startY <= 0.00)
                                {
                                    _b.bias = 0.002;
                                }
                                else
                                {
                                    _b.bias = -0.002;
                                }
                            }
                            if (_b.bowl.X > screenWidth - ditchW) // testing a ditch effect
                            {
                                _b.bowl.Size = new SizeF(20, 20); // make the bowl appear smaller if it is in the ditch
                                _b.power = 0; // no power, can no longer move when in this area
                                _b.coll = 3; // set a status of 3 to the bowl if it falls in the ditch
                            }
                            if (jack.X > screenWidth - ditchW)
                            {
                                jack.Size = new SizeF(8, 8); // make the jack smaller if it is in the ditch
                                jPower = 0; // no power, can no longer move when in this area
                                jColl = 3;
                            }
                            _b.newY += _b.bias; // plus the calculated bias to the next Y co-ord
                            _b.bowl.X = _b.bowl.X + bowlSpeed; // move the bowl another 2 pixels to the right
                            _b.power = _b.power - bowlSpeed; // reduce the amount of remaining power by 2x
                            _b.bowl.Y += (float)_b.newY; // adjust the position to the new Y co-ord
                        }
                        
                        Refresh(); // refresh teh page graphics to update the above movements
                    }
                }
                int c = 0; // a count to compare
                int l = Bowls.Count(); // a count of the dynamic structure size
                foreach (theBowls _t in Bowls)
                {
                    if (_t.power <= 0)
                    { // if the bowl has stoped move
                        c++; // count
                    }
                }
                foreach (theBowls _cd in Bowls)
                {
                    if (_cd.power <= 0 && _cd.end == currEnd)
                    {
                        // the following calculates the distance from the bowl to the jack
                        _cd.distance = calcDistance(_cd.bowl.X + (bs / 2), _cd.bowl.Y + (bs / 2), jack.X + (js / 2), jack.Y + (js / 2)); // calculat the distance between the bowl and jack
                    }
                }
                if (c == l) // determines if all the bowls have stoped moving
                { // if all bowls have stopped moving
                    if (thisPlayer == 1) // the following changes to the next player and adjusts the colour
                    {
                        thisPlayer = 2;
                        matColour = p2Colour;
                    }
                    else
                    {
                        thisPlayer = 1;
                        matColour = p1Colour;
                    }
                    currBowl++; // move variable to the next bowl to be played
                    if (currBowl > 4) // have all the bowls been played?
                    { // if all bowls for an  end have been played 
                        foreach (theBowls _t in Bowls)
                        {
                            foreach (theBowls _s in Bowls)
                            {
                                if (_t != _s && _t.end == currEnd && _s.end == currEnd)
                                { // compare the distances to determine which is closest
                                    if (_t.distance < _s.distance)
                                    {
                                        _t.shot += 1;
                                    }
                                    else 
                                    {
                                        _s.shot += 1;
                                    }
                                }
                            }
                            foreach (theBowls _r in Bowls)
                            { // calculates the final score for an end who starts on the next end
                                if (_r.shot == 6 && _r.end == currEnd)
                                {
                                    if (_r.play == 1)
                                    {
                                        p1EndScore += 1;
                                        thisPlayer = 1;
                                        matColour = p1Colour;
                                    }
                                    else
                                    {
                                        p2EndScore += 1;
                                        thisPlayer = 2;
                                        matColour = p2Colour;
                                    }
                                }
                            }
                            foreach (theBowls _r2 in Bowls)
                            { // determine if the player has more then one pint
                                if (_r2.shot == 4 && _r2.end == currEnd)
                                {
                                    if (_r2.play == 1 && p1EndScore > 0)
                                    {
                                        p1EndScore += 1;
                                    }
                                    else if (_r2.play == 2 && p2EndScore > 0)
                                    {
                                        p2EndScore += 1;
                                    }
                                }
                            }
                        }
                        p1score += p1EndScore; // adds the tally score to the overall score
                        p2score += p2EndScore; // adds the tally score to the overall score
                        if (showOnce)
                        { 
                            bowlTime.Stop();
                            // displays the result of the end
                            DialogResult ee = MessageBox.Show("Well done, the results of end " + currEnd + " are as followed\n\nPlayer 1 Scored : " + p1EndScore + "\nPlayer 2 Scored : " + p2EndScore + "\n\n\nClick OK to continue to the next end", "Results", MessageBoxButtons.OK);
                            if (ee == DialogResult.OK)
                            {
                                p1EndScore = 0; // resets the tally score for the next end
                                p2EndScore = 0; // resets the tally score for the next end
                                currEnd++; // move to the next end
                                currBowl = 1; // reset first bowl to 1
                                if (p1score >= 7 || p2score >= 7)
                                { // determines if there is a victor
                                    string winner;
                                    if (p1score >= 7)
                                    {
                                        winner = "One";
                                    }
                                    else
                                    {
                                        winner = "Two";
                                    }
                                    // displays the final winn and asks if another game is to be played
                                    DialogResult end = MessageBox.Show("Congratulation\n\nPlayer " + winner + " is the WINNER\n\nWould you like to play again?", "WINNER!", MessageBoxButtons.YesNo);
                                    if (end == DialogResult.Yes)
                                    {
                                        newGame(); // resets for a new game
                                    }
                                    else
                                    {
                                        Application.Exit(); // closes the game
                                    }
                                }
                                
                                drawJack(); // if the jack is moved off position, this will recenter it for the next end
                                gameLoop(); // repeat game loop
                            }
                            showOnce = false;
                        }
                    }
                    else
                    {
                        bowlTime.Stop(); // stops the bowl animation
                        gameLoop(); // repeats gameloop
                    }
                }
            }
        }
        
        public bool circleCollide(float _X1, float _Y1, float _X2, float _Y2, int _r1, int _r2)
        { // the following code was provided by Glenn, calculates if two circles intersect
            return (Math.Sqrt(Math.Pow(Math.Abs(_X1 - _X2), 2) + Math.Pow(Math.Abs(_Y1 - _Y2), 2)) <= (_r1 + _r2));
        }

        public void newDirect(float _X1, float _Y1, float _X2, float _Y2, float _p)
        {
            double _A = _X1 - _X2; // calculates the adjacent
            double _O = _Y1 - _Y2; // calculates the opposite
            double _toa = _O / _A;
            double _deg = Math.Atan(_toa); // calculates the radians
            passY = Math.Tan(_deg) * bowlSpeed; // stores the new Y co-ord
            // the following offsets a collision so items no longer disappear off the screen at sharp angles
            if (passY > 4)
            {
                passY = 4;
            }
            else if (passY < -4)
            {
                passY = -4;
            }
            pow = _p / 2.5f; // reduces the amount of power remaining for each bowl
        }

        public double calcDistance(float _X1, float _Y1, float _X2, float _Y2)
        {
            double _A = _X1 - _X2; // calculates the adjacent
            double _O = _Y1 - _Y2; // calculates the opposite
            double _toa = _O / _A;
            double _H = Math.Sqrt(Math.Pow((_A), 2)) + Math.Sqrt(Math.Pow((_O), 2)); // calculates the hypotenuse
            return _H; // returns the hypotenuse as a value of distance
        }

        private void gameLoopy_Tick(object sender, EventArgs e)
        {
            // redundent, this didn't quite work the way I hoped
        }
    }
}
