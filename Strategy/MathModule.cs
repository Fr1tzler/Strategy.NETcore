using System;
using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public static class MathModule
    {
        public static double Length(Vector2f vector) => Hypot(vector.X, vector.Y);

        public static double Hypot(double x, double y) => Math.Sqrt(x * x + y * y);

        public static bool PointInsideRectangle(Vector2f point, RectangleShape rectangle)
        {
            return (point.X >= rectangle.Position.X) && (point.Y >= rectangle.Position.Y) &&
                   (point.X <= rectangle.Position.X + rectangle.Size.X) &&
                   (point.Y <= rectangle.Position.Y + rectangle.Size.Y);
        }

        public static Vector2f ReverseVectorTransform(Vector2f vector) =>
            new Vector2f(vector.X * -0.25f + vector.Y * 0.5f, vector.X * 0.25f + vector.Y * 0.5f);
    }
}