using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrigTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        public static double findDegree(float _startX, float _startY, float _markX, float _markY)
        {
            float _o = _startY - _markY;
            float _a = _startX - _markX;
            double _toa = _o / _a;
            // double _x = Math.Tan(_toa); // need to work out how to "Tan-1"
            double _x = Math.Atan(_toa) * 180 / Math.PI; // apparently using the above method will return radiuns not degrees
            return _x; // This will need some testing!!! But this should be the basic template
        }

        public static double findNewPosition(double _deg, int _nextX)
        {
            double _y = Math.Tan(_deg) * _nextX;
            return _y;
        }
    }
}
