using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        //Public Variables Go Here
        public int fs = 36; // Define the size of the font used
        public int screenWidth, screenHeight; // Varable to hold the size of the form maximised on load
        public int bannerHeight, gameHeight; // Variables to gold the height of the game area and the heaight for the header and footers.

        private void frmMainGame_Paint(object sender, PaintEventArgs e)
        {
            Font ff = new Font("Comfortaa", fs, FontStyle.Bold); // defines the font style for the graphic text used
            gameHeight = (screenWidth / 7) + 200; // calculation to work out the game area
            bannerHeight = (screenHeight - gameHeight) / 2; // the height of the header and footer banners
            e.Graphics.FillRectangle(Brushes.LightGreen, 0, 0, screenWidth, bannerHeight); // draw the header banner
            e.Graphics.FillRectangle(Brushes.DarkOliveGreen, 0, bannerHeight + gameHeight, screenWidth, bannerHeight); // draw the footer banner
            e.Graphics.DrawString("Exit Game", ff, Brushes.DarkRed, new Point(screenWidth - 300, screenHeight - bannerHeight / 2)); // draw the exit button
            e.Graphics.DrawString("Option ", ff, Brushes.DarkBlue, new Point(50, screenHeight - bannerHeight / 2)); // draw the options button
           
        }

        private void frmMainGame_Load(object sender, EventArgs e)
        {
            screenSize(); // executes function to find out the form width and height at launch
        }

        public void screenSize()
        {
            screenWidth = this.ClientSize.Width; // populates variable screenWidth with the actual current form width
            screenHeight = this.ClientSize.Height; // populates variable screenHeight with the actual current form height
        }

       
    }
}
