using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Line
{
    public partial class customize : Form
    {
        Form1 Main;
        Bitmap strzalka = new Bitmap(50, 50);
        public bool Mouse = false;
        int Scale1 = 8;
        public Color SelectedColor;
        int Mode = 0;
        public customize(Form1 Main)
        {
            this.Main = Main;
            InitializeComponent();
        }

        private void customize_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.customize = null;
        }

        private void customize_Load(object sender, EventArgs e)
        {
            Deafult();
        }
        public void Update1()
        {
            pictureBox1.Size = new Size(50 * Scale1,50 * Scale1);
            SelectedColor = Color.FromArgb(255,(int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value);
            pictureBox2.BackColor = SelectedColor;
            if(Mode == 0)
            {
                label2.Text = "rysowanie";
            }
            if (Mode == 1)
            {
                label2.Text = "wypelnianie";
            }
            if (Mode == 2)
            {
                label2.Text = "rysowanie lini";
            }
            if (Mode == 3)
            {
                label2.Text = "gumka";
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.InterpolationMode =
        System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            e.Graphics.ScaleTransform(Scale1, Scale1);
            e.Graphics.DrawImage(strzalka, 0, 0);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Mouse = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X / Scale1;
            int y = e.Y / Scale1;
            Mouse = true;
            if (Mode == 1)
            {
                Color target = strzalka.GetPixel(x, y);
                FloodFill(x, y, target, SelectedColor);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse == true)
            {
                int x = e.X / Scale1;
                int y = e.Y / Scale1;

                if (x >= 0 && x < 50  && y >= 0 && y < 50 )
                {
                    if(Mode == 0)
                    {
                        strzalka.SetPixel(x, y, SelectedColor);
                    }
                    else if(Mode == 3)
                    {
                        strzalka.SetPixel(x, y, Color.FromArgb(0,0,0,0));
                    }

                    pictureBox1.Invalidate();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mode = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mode = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Mode = 0;
        }
        private void FloodFill(int x, int y, Color targetColor, Color replacementColor)
        {
            if (x < 0 || x >= 50 || y < 0 || y >= 50)
                return;

            if (strzalka.GetPixel(x, y).ToArgb() != targetColor.ToArgb())
            {
                return;
            }


            if (targetColor.ToArgb() == replacementColor.ToArgb())
            {
                return;
            }


            strzalka.SetPixel(x, y, replacementColor);

            FloodFill(x + 1, y, targetColor, replacementColor);
            FloodFill(x - 1, y, targetColor, replacementColor);
            FloodFill(x, y + 1, targetColor, replacementColor);
            FloodFill(x, y - 1, targetColor, replacementColor);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Mode = 3;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Deafult();
        }
        public void Deafult()
        {
            
            Point[] points = new Point[3];
            points[0] = new Point(0, 0);
            points[1] = new Point(0, 45);
            points[2] = new Point(50, 25);
            Graphics g = Graphics.FromImage(strzalka);
            g.Clear(Color.Transparent);
            g.FillPolygon(Brushes.Red,points);
            pictureBox1.Invalidate();
        }
    }
}
