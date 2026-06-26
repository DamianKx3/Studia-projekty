using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Line
{
    public class Rectangle1 : GameObject
    {
        public Rectangle1() : base()
        {
            Type = 2;
        }
        public Rectangle1(float x, float y) : base(x, y)
        {
            Type = 2;
        }
        public override void Start()
        {

        }
        public override void Update()
        {

        }
        public override bool Hitbox(PointF Player)
        {

            if(Math.Abs(Player.X - X) < ScaleX / 2 && Math.Abs(Player.Y - Y) < ScaleY / 2)
            {
                return true;
            }
            return false;
        }
        public override void Render(PaintEventArgs e,float X1)
        {
            SolidBrush b = new SolidBrush(Color);
            e.Graphics.FillRectangle(b, new RectangleF(X - X1 - ScaleX / 2, Y - ScaleY / 2, ScaleX, ScaleY));
            b.Dispose();

        }
    }
}
