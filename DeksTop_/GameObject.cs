using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Line
{
    public class GameObject
    {
        public float X;
        public float Y;
        public float speed;
        public float angle = 0;
        public float Scale = 1;
        public float ScaleX = 50;
        public float ScaleY = 50;
        public int Link;
        public int Type;
        public Color Color;
        public GameObject(){
            X = 0;
            Y = 0;
            Color = Color.Black;
            Start();
        }
        public GameObject(float x, float y)
        {
            X = x;
            Y = y;
            Color = Color.Black;
            Start();
        }
        public virtual void Start()
        {
            Color = Color.Black;
        }
        public virtual void Update()
        {

        }
        public virtual void OnKeyPressed(Keys Key)
        {
            
        }
        public virtual void Render(PaintEventArgs e, float X)
        {

        }
        public virtual bool Hitbox(PointF Player)
        {
            return false;
        }
        public static float ReturnX(GameObject obj)
        {
            return obj.X;
        }
    }
}
