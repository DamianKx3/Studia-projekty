using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Line
{
    internal class Trigger : GameObject
    {
        public int Param1;
        public int Param2;
        public int Param3;
        public int Param4;
        public int TriggerType;
        public bool Triggered = false;
        public GameController Controller;
        public Form3 win;
        public Trigger() : base()
        {

        }
        public Trigger(float x, float y) : base(x, y)
        {

        }
        public override void Start()
        {
            if(Type == 0)
            {
                Param1 = 10;
            }
        }
        public override void Update()
        {
            if(Controller != null && Controller.Player1.X + Controller.X >= X)
            {
                Trigger1();
            }
   

        }
       
        public override void Render(PaintEventArgs e, float X1)
        {

            if (Controller == null)
            {
                e.Graphics.FillEllipse(Brushes.Black, new RectangleF(X - X1, Y, 20, 20));
                e.Graphics.DrawLine(Pens.Black, new PointF(X - X1, 0), new PointF(X - X1, 1000));

            }
            else
            {
                if (Type == 0 && Triggered == false)
                {
                    if (Param1 > Controller.speed)
                    {
                        e.Graphics.DrawLine(Pens.Blue, new PointF(X - X1, 0), new PointF(X - X1, 1000));
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Yellow, new PointF(X - X1, 0), new PointF(X - X1, 1000));
                    }

                }else if (Type == 1)
                {

                }else if (Type == 5)
                {
                    e.Graphics.DrawLine(Pens.DarkOrange, new PointF(X - X1, 0), new PointF(X - X1, 1000));
                }
            }




        }
        public void Trigger1()
        {
            if(Triggered == false)
            {
                Triggered = true;
                if (Type == 0)
                {

                    Controller.speed = Param1;
                }else if(Type == 1)
                {

                    Controller.TargetWinSizeX = Param1;
                    Controller.TargetWinSizeY = Param2;
                    Controller.WindowScrollSpeed = Param3;
                }else if(Type == 2 || Type == 3)
                {

                    int tr = Param1;
                    int tg = Param2;
                    int tb = Param3;
                    int step = Param4;
                    if(step == 0)
                    {
                        if(Type == 2)
                        {
                            Controller.ObstaclesColor = Color.Magenta;
                        }
                        else
                        {
                            Controller.PathColor = Color.Magenta;
                        }

                        return;
                        
                    }
                    int r = 0;
                    int b = 0;
                    int g = 0;
                    if(Type == 2)
                    {
                        r = Controller.ObstaclesColor.R;
                        g = Controller.ObstaclesColor.G;
                        b = Controller.ObstaclesColor.B;
                    }
                    else
                    {
                        r = Controller.PathColor.R;
                        g = Controller.PathColor.G;
                        b = Controller.PathColor.B;
                    }

                    if(r < tr)
                    {
                        if (Math.Abs(r - tr) > step)
                        {
                            r = r + step;
                            Triggered = false;
                        }
                        else
                        {
                            r = tr;
                        }
                    }

                    if (r > tr)
                    {
                        if (Math.Abs(r - tr) > step)
                        {
                            r = r - step;
                            Triggered = false;
                        }
                        else
                        {
                            r = tr;
                        }
                    }

                    if (g < tg)
                    {
                        if (Math.Abs(g - tg) > step)
                        {
                            g = g + step;
                            Triggered = false;
                        }
                        else
                        {
                            g = tg;
                        }
                    }

                    if (g > tg)
                    {
                        if (Math.Abs(g - tg) > step)
                        {
                            g = g - step;
                            Triggered = false;
                        }
                        else
                        {
                            g = tg;
                        }
                    }

                    if (b < tb)
                    {
                        if (Math.Abs(b - tb) > step)
                        {
                            b = b + step;
                            Triggered = false;
                        }
                        else
                        {
                            b = tb;
                        }
                    }
                    if (b > tb)
                    {
                        if (Math.Abs(b - tb) > step)
                        {
                            b = b - step;
                            Triggered = false;
                        }
                        else
                        {
                            b = tb;
                        }
                    }
                    if (Type == 2)
                    {
                        Controller.ObstaclesColor = Color.FromArgb(r, g, b);
                    }
                    else
                    {
                        Controller.PathColor = Color.FromArgb(r, g, b);
                    }

                }else if(Type == 4)
                {
                    Controller.TargetWinX = Param1;
                    Controller.TargetWinY = Param2;
                    Controller.WindowScrollSpeed = Param3;

                }
                else if (Type == 5)
                {
                    if(Controller.FlipGraphics == true)
                    {
                        Controller.FlipGraphics = false;
                    }
                    else
                    {
                        Controller.FlipGraphics = true;

                    }

                }


            }
        }
    }
}
