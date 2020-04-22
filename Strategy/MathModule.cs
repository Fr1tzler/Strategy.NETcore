using System;
using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public static class MathModule
    {
        public static double Length(Vector2f vector) => Hypot(vector.X, vector.Y);

        private static double Hypot(double x, double y) => Math.Sqrt(x * x + y * y);

        public static bool PointInsideTriangle(Vector2f point, Shape polygon)
        {
            var firstPoint = polygon.GetPoint(0);
            var secondPoint = polygon.GetPoint(1);
            var thirdPoint = polygon.GetPoint(2);
            return PointsOnOneSide(firstPoint, secondPoint, thirdPoint, point) &&
                   PointsOnOneSide(secondPoint, thirdPoint, firstPoint, point) &&
                   PointsOnOneSide(thirdPoint, firstPoint, secondPoint, point);
        }

        private static bool PointsOnOneSide(Vector2f firstPoint, Vector2f secondPoint, Vector2f thirdPoint,
            Vector2f point)
        {
            return RelativePointPosition(firstPoint, secondPoint, thirdPoint) *
                RelativePointPosition(firstPoint, secondPoint, point) >= 0;
        }

        private static double RelativePointPosition(Vector2f firstPoint, Vector2f secondPoint, Vector2f point)
        {
            return (point.X - firstPoint.X) * (secondPoint.Y - firstPoint.Y) -
                   (point.Y - firstPoint.Y) * (secondPoint.X - firstPoint.X);
        }

        public static bool PointInsideRectangle(Vector2f point, RectangleShape rectangle)
        {
            return (point.X >= rectangle.Position.X) && (point.Y >= rectangle.Position.Y) &&
                   (point.X <= rectangle.Position.X + rectangle.Size.X) &&
                   (point.Y <= rectangle.Position.Y + rectangle.Size.Y);
        }
        
        public static Vector2f ReverseVectorTransform(Vector2f vector) => new Vector2f(vector.X * -0.25f + vector.Y * 0.5f, vector.X * 0.25f + vector.Y * 0.5f);

    }
}