using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void frmMainGame_Paint(object sender, PaintEventArgs e)
        {
            // Font ff = new Font("resources/Comfortaa-Regular.ttf", fs, FontStyle.Bold); // defines the font style for the graphic text used // No longer needed as it's now in its own function
            gameHeight = (screenWidth / 7) + 200; // calculation to work out the game area // increased from the design to 200 from 100 to make the game area bigger then the banners
            bannerHeight = (screenHeight - gameHeight) / 2; // the height of the header and footer banners
            e.Graphics.FillRectangle(Brushes.LightGreen, 0, 0, screenWidth, bannerHeight); // draw the header banner
            e.Graphics.FillRectangle(Brushes.LightGreen, 0, bannerHeight + gameHeight, screenWidth, bannerHeight); // draw the footer banner
            e.Graphics.DrawString("Exit Game", ff, Brushes.DarkRed, new Point(screenWidth - 300, (screenHeight - bannerHeight / 2) -(fs / 2))); // draw the exit button
            e.Graphics.DrawString("Options", ff, Brushes.DarkBlue, new Point(50, (screenHeight - bannerHeight / 2) - (fs / 2))); // draw the options button
            e.Graphics.FillRectangle(Brushes.Brown, 0,bannerHeight, ditchW,gameHeight); // draw the left ditch
            e.Graphics.FillRectangle(Brushes.Brown, screenWidth - ditchW, bannerHeight, ditchW, gameHeight); // draw the right ditch
            e.Graphics.FillRectangle(Brushes.PeachPuff, 0, bannerHeight + gameHeight, screenWidth, 30); // draw bar
        }

        private void frmMainGame_Load(object sender, EventArgs e)
        {
            screenSize(); // executes function to find out the form width and height at launch
            customFont(); // executes function that adds a custom font family to be used regardless of whether the user has it installed or not
        }

        public void screenSize()
        {
            screenWidth = this.ClientSize.Width; // populates variable screenWidth with the actual current form width
            screenHeight = this.ClientSize.Height; // populates variable screenHeight with the actual current form height
        }

        public void customFont()
        {
            PrivateFontCollection pfc = new PrivateFontCollection(); // Creates a new instance of a Private Font Collection
            pfc.AddFontFile("resources/Comfortaa-bold.ttf"); // Adds a font held in the application resources into the collection pfc
            ff = new Font(pfc.Families[0], fs, FontStyle.Bold); // defines the variable ff with the font family, size and style
        }
    }
}
