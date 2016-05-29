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

namespace BowlsSimulator
{
    public partial class frmMainGame : Form
    {
        public frmMainGame()
        {
            InitializeComponent();
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


        private void frmMainGame_Paint(object sender, PaintEventArgs e)
        {
            // Font ff = new Font("resources/Comfortaa-Regular.ttf", fs, FontStyle.Bold); // defines the font style for the graphic text used // No longer needed as it's now in its own function
            e.Graphics.FillRectangle(Brushes.LightGreen, 0, 0, screenWidth, bannerHeight); // draw the header banner
            e.Graphics.FillRectangle(Brushes.LightGreen, 0, bannerHeight + gameHeight, screenWidth, bannerHeight); // draw the footer banner
            e.Graphics.DrawString("Exit Game", ff, exitColour, new Point((screenWidth - 300), (screenHeight - bannerHeight / 2) - (fs / 2))); // draw the exit button
            e.Graphics.DrawString("Options", ff, optionColour, new Point(50, (screenHeight - (bannerHeight / 2)) - (fs / 2))); // draw the options button
            e.Graphics.FillRectangle(Brushes.Brown, 0,bannerHeight, ditchW,gameHeight); // draw the left ditch
            e.Graphics.FillRectangle(Brushes.Brown, screenWidth - ditchW, bannerHeight, ditchW, gameHeight); // draw the right ditch
            e.Graphics.FillRectangle(Brushes.PeachPuff, 0, bannerHeight + gameHeight, screenWidth, 30); // draw bar
            e.Graphics.FillRegion(Brushes.Transparent, exitButton);
            e.Graphics.FillRegion(Brushes.Transparent, optionsButtons);

        }

        private void frmMainGame_Load(object sender, EventArgs e)
        {
            screenSize(); // executes function to find out the form width and height at launch
            drawButtons(); // draws regions for generic layout
            customFont(); // executes function that adds a custom font family to be used regardless of whether the user has it installed or not
          
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
        bool testColour;
    
        private void frmMainGame_MouseMove(object sender, MouseEventArgs e)
        {   
            if (exitButton.IsVisible(e.Location) && !testColour)
            { // if the mouse cursor hovers over the exit button change the colour to Gold
                exitColour = Brushes.Gold;
                Refresh();
                testColour = true;

            }
            else if (testColour && !exitButton.IsVisible(e.Location))
            { // if it's anywhere else change it back to Red
                exitColour = Brushes.DarkRed;
                Refresh();
                testColour = false;
            }

           else if (optionsButtons.IsVisible(e.Location) && !testColour)
            {
                optionColour = Brushes.Red;
                Refresh();
               testColour = true;
            }
            else if (testColour && !optionsButtons.IsVisible(e.Location))
            {
                optionColour = Brushes.DarkBlue;
                testColour = false;
                Refresh();
            } 

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
    }
}
