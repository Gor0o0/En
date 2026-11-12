
using System;

namespace GeometryLibrary
{
    public static class Geometry
    {
        public static double SquareArea(double side)
        {
            return side * side;
        }
        public static double RectangleArea(double width, double height)
        {
            return width * height;
        }
        public static double TriangleArea(double a, double b, double c)
        {
            double p = (a + b + c) / 2;

            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
}
