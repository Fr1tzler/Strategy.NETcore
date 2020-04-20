using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace strategy
{
    public class Scene
    {
        private const float PolygonSize = 20;
        private const int MaxHeight = 2;
        private const int SizeX = 40;
        private const int SizeY = 40;
        private const int ShiftX = 960;
        private const int ShiftY = 100;
        private const float MapMovingVelocity = 10;
        private static byte[,] _depthMap;
        private Vector2f _pointOfView;
        private Vector2f _cursorPosition;
        private List<ConvexShape> _map;
        private ConvexShape _activePolygon;
        private List<ConvexShape> _warFog;
        private List<Unit> _units;
        private const int HeightCoefficient = 5;
        private static Color warFogColor;
        private static Color orange;
        
        
        public Scene(int seed)
        {
            warFogColor = new Color(20, 20, 20, 160);
            orange = new Color(178, 70, 0);
            _units = new List<Unit>();
            _units.Add(new Unit(100, 1, 1, 1, 1, new Vector2f(500, 500), 1, 200));
            _units.Add(new Unit(100, 1, 1, 1, 1, new Vector2f(250, 900), 1, 350));
            _depthMap = new byte[SizeX + 1, SizeY + 1];
            _pointOfView = new Vector2f(0, 25);
            _cursorPosition = new Vector2f(Game.ScreenWidth / 2f, Game.ScreenHeight / 2f);
            var random = new Random(seed);
            for (var x = 0; x <= SizeX; x++)
                for (var y = 0; y <= SizeY; y++)
                    _depthMap[x, y] = (byte) random.Next(MaxHeight);
            GenerateMap();
            GenerateWarFog();
        }

        private void GenerateMap()
        {
            _map = new List<ConvexShape>();
            for (var x = 0; x < SizeX; x++)
                for (var y = 0; y < SizeY; y++)
                    _map.Add(GetPolygon(new Vector2i(x, y), true));
        }

        private void GenerateWarFog()
        {
            _warFog = new List<ConvexShape>();
            for (var x = 0; x < SizeX; x++)
                for (var y = 0; y < SizeY; y++)
                    _warFog.Add(GetPolygon(new Vector2i(x, y), false)); 
        }
        
        private static ConvexShape GetPolygon(Vector2i self, bool notWarFog)
        {
            var polygon = new ConvexShape(4);
            polygon.SetPoint(0, GetMapPoint(self.X, self.Y, _depthMap[self.X, self.Y]));
            polygon.SetPoint(1, GetMapPoint(self.X + 1, self.Y, _depthMap[self.X + 1, self.Y]));
            polygon.SetPoint(2, GetMapPoint(self.X + 1, self.Y + 1, _depthMap[self.X + 1, self.Y + 1]));
            polygon.SetPoint(3, GetMapPoint(self.X, self.Y + 1, _depthMap[self.X, self.Y + 1]));
            polygon.FillColor = notWarFog ? orange : warFogColor;
            polygon.OutlineColor = Color.Black;
            polygon.OutlineThickness = 1;
            return polygon;
        }

        private static Vector2f GetMapPoint(float x, float y, byte depth)
        {
            return new Vector2f(2 * (-x + y) * PolygonSize + ShiftX, (x + y) * PolygonSize + ShiftY + depth * HeightCoefficient);
        }

        public void Update()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                Game.CurrentState = 0; 
                return;
            }

            var color = new Color(20, 20, 20, 160);
            
            foreach (var shape in _warFog)
                shape.FillColor = color;

            for (var x = 0; x < SizeX; x++) 
                for (var y = 0; y < SizeY; y++) 
                    foreach (var unit in _units)
                        if (MathModule.Length(GetMapPoint(x + 0.5f, y + 0.5f, _depthMap[x, y]) - unit.sprite.Position + _pointOfView) < unit.ViewRadius)
                            _warFog[SizeY * x + y].FillColor = Color.Transparent;
            _cursorPosition = (Vector2f) Mouse.GetPosition();
            var movement = Controls.GetArrowsState() * -MapMovingVelocity;
            _pointOfView += movement;

            foreach (var polygon in _map)
            {
                polygon.Position += movement;
                polygon.OutlineColor = Color.Black;
                if (!MathModule.PointInsideTriangle(_cursorPosition - _pointOfView, polygon)) continue;
                _activePolygon = polygon;
                _activePolygon.OutlineColor = Color.White;
            }

            foreach (var polygon in _warFog)
                polygon.Position += movement;

            foreach (var unit in _units)
            {
                unit.sprite.Position += movement;
            }
        }

        public void Display(RenderWindow window)
        {
            foreach (var polygon in _map)
                window.Draw(polygon);
            foreach (var shape in _warFog)
                window.Draw(shape);

            if (_activePolygon != null)
                window.Draw(_activePolygon);
            foreach (var unit in _units)
            {
                window.Draw(unit.sprite);
            }
        }
    }
}