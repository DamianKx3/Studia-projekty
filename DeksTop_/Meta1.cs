using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Line
{
    public  class Meta1 : GameObject
    {

        public Meta1() : base()
        {

        }
        public Meta1(float x, float y) : base(x, y)
        {
        }
        public override void Start()
        {

        }
        public override void Update()
        {

        }
        public override void Render(PaintEventArgs e, float X1)
        {
            e.Graphics.FillRectangle(Brushes.Magenta, new RectangleF(X - X1, 0, 1000, 1000));

        }

    }
}
