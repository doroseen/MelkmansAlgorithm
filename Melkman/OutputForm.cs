using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Melkman
{
    public partial class OutputForm : Form
    {
        private const int WND_CLIENT_SIZE = 900;
        private Bitmap bmp;
        private System.Drawing.Graphics graphics;

        public OutputForm()
        {
            InitializeComponent();
        }
        
        ~OutputForm()
        {
            graphics.Dispose();
        }

        private void OutputForm_Paint(object sender, PaintEventArgs e)
        {
            BackgroundImage = bmp;
        }

        private void OutputForm_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(WND_CLIENT_SIZE, WND_CLIENT_SIZE);/*redim fereastra*/
            bmp = new Bitmap(WND_CLIENT_SIZE, WND_CLIENT_SIZE);
            graphics = Graphics.FromImage(bmp);
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            graphics.FillRectangle(brush, new Rectangle(0, 0, WND_CLIENT_SIZE, WND_CLIENT_SIZE));
            brush.Dispose();
        }

        public Graphics GetGraphics()
        {
            return graphics;
        }

        public int get_twnd_client_size()
        {
            return WND_CLIENT_SIZE;
        }
    }
}
