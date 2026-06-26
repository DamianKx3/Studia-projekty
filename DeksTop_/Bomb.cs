using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Line
{
    public class Bomb : GameObject
    {

        PointF[] model = new PointF[8];
        PointF[] Finalpoints = new PointF[8];

        public Bomb() : base()
        {
            Type = 1;
        }
        public Bomb(float x, float y) : base(x, y)
        {
            Type = 1;
        }
        public override void Start()
        {

            model[0] = new PointF(0, 1);
            model[1] = new PointF(0.2f, 0.2f);
            model[2] = new PointF(1f, 0);
            model[3] = new PointF(0.25f, -0.25f);
            model[4] = new PointF(0f, -1);
            model[5] = new PointF(-0.2f, -0.2f);
            model[6] = new PointF(-1, 0);
            model[7] = new PointF(-0.2f, 0.2f);




        }
        public override void Update()
        {
            angle = angle + 20;

        }

        public override void Render(PaintEventArgs e,float X1)
        {
            SolidBrush b = new SolidBrush(Color);


            e.Graphics.FillEllipse(b,new RectangleF((X - X1 - ScaleX * 0.5f / 2),(Y - ScaleY * 0.5f / 2), ScaleX * 0.5f, ScaleY * 0.5f));
            for (int i = 0; i < model.Length; i++)
            {
                float angleRad = angle * (float)Math.PI / 180f;

                float sx = model[i].X * ScaleX / 2;
                float sy = model[i].Y * ScaleY / 2;

                float rx = sx * (float)Math.Cos(angleRad) - sy * (float)Math.Sin(angleRad);
                float ry = sx * (float)Math.Sin(angleRad) + sy * (float)Math.Cos(angleRad);

                Finalpoints[i] = new PointF(X + rx - X1,Y + ry);
            }
            e.Graphics.FillPolygon(b, Finalpoints);
            b.Dispose();

        }
        public override bool Hitbox(PointF Player)
        {
            float dx = X - Player.X;
            float dy = Y - Player.Y;

            if ((float)Math.Sqrt(dx * dx + dy * dy) < ScaleX / 2)
            {
                return true;
            }
            return false;
        }
    }
}
