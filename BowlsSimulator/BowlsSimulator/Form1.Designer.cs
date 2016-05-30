namespace BowlsSimulator
{
    partial class frmMainGame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.powerTime = new System.Windows.Forms.Timer(this.components);
            this.xHairTime = new System.Windows.Forms.Timer(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.playerColour = new System.Windows.Forms.ColorDialog();
            this.colorDialog3 = new System.Windows.Forms.ColorDialog();
            this.colorDialog4 = new System.Windows.Forms.ColorDialog();
            this.gameLoopy = new System.Windows.Forms.Timer(this.components);
            this.bowlTime = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // powerTime
            // 
            this.powerTime.Interval = 10;
            this.powerTime.Tick += new System.EventHandler(this.powerTime_Tick);
            // 
            // xHairTime
            // 
            this.xHairTime.Interval = 10;
            this.xHairTime.Tick += new System.EventHandler(this.xHairTime_Tick);
            // 
            // gameLoopy
            // 
            this.gameLoopy.Tick += new System.EventHandler(this.gameLoopy_Tick);
            // 
            // bowlTime
            // 
            this.bowlTime.Interval = 10;
            this.bowlTime.Tick += new System.EventHandler(this.bowlTime_Tick);
            // 
            // frmMainGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(460, 309);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMainGame";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMainGame_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMainGame_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmMainGame_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer powerTime;
        private System.Windows.Forms.Timer xHairTime;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ColorDialog playerColour;
        private System.Windows.Forms.ColorDialog colorDialog3;
        private System.Windows.Forms.ColorDialog colorDialog4;
        private System.Windows.Forms.Timer gameLoopy;
        private System.Windows.Forms.Timer bowlTime;
    }
}

