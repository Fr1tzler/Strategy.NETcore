using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace strategy
{
    public class View
    {
        private const float PolygonSize = 20;
        private const int SizeX = 40;
        private const int SizeY = 40;
        private const float MapMovingVelocity = 20;
        private readonly List<ConvexShape> _map;
        private readonly List<ConvexShape> _warFog;
        
        public View()
        {
            _map = GenerateMap(new Color(178, 70, 0));
            _warFog = GenerateMap(new Color(20, 20, 20, 160));
        }

        private static List<ConvexShape> GenerateMap(Color defaultColor)
        {
            var result = new List<ConvexShape>();
            for (var x = 0; x < SizeX; x++)
            for (var y = 0; y < SizeY; y++)
                result.Add(GetPolygon(new Vector2i(x, y), defaultColor));
            var shift = new Vector2f(1600, -200);
            foreach (var shape in result)
                shape.Position += shift;
            return result;
        }

        private static ConvexShape GetPolygon(Vector2i self, Color defaultColor)
        {
            var polygon = new ConvexShape(4)
            {
                Position = GetMapPoint(self.X, self.Y),
                FillColor = defaultColor,
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };
            polygon.SetPoint(0, new Vector2f(0, 0));
            polygon.SetPoint(1, GetMapPoint(self.X + 1, self.Y) - polygon.Position);
            polygon.SetPoint(2, GetMapPoint(self.X + 1, self.Y + 1) - polygon.Position);
            polygon.SetPoint(3, GetMapPoint(self.X, self.Y + 1) - polygon.Position);
            return polygon;
        }

        private static Vector2f GetMapPoint(float x, float y) =>
            new Vector2f(2 * (-x + y) * PolygonSize, (x + y) * PolygonSize);

        public void Update(float dt)
        {
            var state = Controls.GetArrowsState();
            foreach (var shape in _map)
            {
                shape.Position -= state * MapMovingVelocity * dt;
            }

            foreach (var shape in _warFog)
            {
                shape.Position -= state * MapMovingVelocity * dt;
            }
        }
        
        public void Display(RenderWindow window)
        {
            Update(1f);
            window.Clear(Color.Black);
            foreach (var polygon in _map)
                window.Draw(polygon);
            foreach (var shape in _warFog)
                window.Draw(shape);
            foreach (var unit in SceneModel.PlayerUnits)
                DisplayUnit(window, unit, true);
            foreach (var unit in SceneModel.EnemyUnits.Where(unit => unit.IsVisible))
                DisplayUnit(window, unit, false);
            foreach (var bullet in SceneModel.PlayerBullets)
                bullet.Display(window);
            foreach (var bullet in SceneModel.EnemyBullets)
                bullet.Display(window);
        }

        private void DisplayUnit(RenderWindow window, UnitModel unit, bool friendly)
        {
            window.Draw(new CircleShape
            {
                Radius = 5,
                FillColor = Color.Yellow,
                Position = unit.Position
            });
        }
    }
}