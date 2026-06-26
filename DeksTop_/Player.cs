using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Line
{
    public class Player : GameObject
    {
        public bool Gravity = true;
        PointF[] Finalpoints = new PointF[3];
        public List<PointF> model = new List<PointF>();
        public List<PointF> points = new List<PointF>();
        public List<PointF> trail = new List<PointF>();
        public Label label;
        public float HeighLimit;
        public float LowLimit;
        public GameController Controller;
        public bool Won = false;
        public bool Dead = false;
        public Player() : base()
        {

        }
        public Player(float x, float y) : base(x, y)
        {
        }
        public override void Start()
        {
            Type = 0;
            base.Start();
            Scale = 10;

            model.Add(new PointF(-1, 1));
            model.Add(new PointF(2, 0));
            model.Add(new PointF(-1, -1));

            points.Add(new PointF(0, 0));
            points.Add(new PointF(0, 0));
            points.Add(new PointF(0, 0));
            Finalpoints = points.ToArray();
        }
        public override void Update()
        {
            if(Gravity == true)
            {
                if (Y + Scale  < HeighLimit)
                {
                    angle = 45;
                    Y = Y + speed;
                }
                else
                {
                    Y = HeighLimit - Scale;
                    if(angle == 45)
                    {
                        Controller.TouchedCeil();
                    }
                    angle = 0;
                }

                


            }
            else
            {
                if (Y - Scale > LowLimit)
                {
                    angle = -45;
                    Y = Y - speed;
                }
                else
                {

                    Y = LowLimit + Scale;
                    if (angle == -45)
                    {
                        Controller.TouchedCeil();
                    }
                    angle = 0;
                }

            }


           

            for (int i = 0; i < Finalpoints.Length; i++)
            {
                Finalpoints[i].X = X + points[i].X;
                Finalpoints[i].Y = Y + points[i].Y;
            }
            if (Won == true)
            {
                angle = 0;
            }
            Transform();

        }
        public override void OnKeyPressed(Keys Key)
        {
            if(Key == Keys.Space)
            {
                if(Gravity == true)
                {
                    Gravity = false;
                }
                else
                {
                    Gravity = true;
                }
            }
        }
        public override void Render(PaintEventArgs e,float X1)
        {
            if(Dead == false)
            {
                e.Graphics.FillPolygon(Brushes.Red, Finalpoints);
                trail.Add(new PointF(X, Y));
                while (trail.Count > 0 && trail[0].X < 0)
                {
                    trail.RemoveAt(0);
                }
                for (int i = 0; i < trail.Count; i++)
                {
                    trail[i] = new PointF(trail[i].X - speed, trail[i].Y);
                }
            }
            else
            {
                PointF[] cross = new PointF[2];
                PointF[] cross2 = new PointF[2];
                cross[0] = new PointF(X- 5, Y- 5); 
                cross[1] = new PointF(X + 5, Y + 5); 
                cross2[0] = new PointF(X - 5, Y + 5); 
                cross2[1] = new PointF(X + 5, Y - 5);
                e.Graphics.DrawLines(Pens.Red,cross);
                e.Graphics.DrawLines(Pens.Red,cross2);
            }
            
            if (trail.Count > 1)
            {
                e.Graphics.DrawLines(Pens.Red, trail.ToArray());
            }

        }
        public void Transform()
        {
            for (int i = 0; i < points.Count; i++)
            {
                float angle1 = angle * (float)(Math.PI / 180);
                float newX = model[i].X * (float)Math.Cos(angle) - model[i].Y * (float)Math.Sin(angle);
                float newY = model[i].X * (float)Math.Sin(angle) + model[i].Y * (float)Math.Cos(angle);
                points[i] = new PointF(newX * Scale, newY * Scale);
            }
        }
    }
}
