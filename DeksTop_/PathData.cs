using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Line
{

    public class PathData
    {
        public float X;
        public float Y;
        public float Width;
        public float OffsetX;
        public float OffsetY;
        public PathData(float x, float y, float width, float offsetX, float offsetY)
        {
            X = x;
            Y = y;
            Width = width;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }
    }
}
