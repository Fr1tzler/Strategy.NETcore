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
        private const float MapMovingVelocity = 10;
        private static byte[,] _depthMap;
        private Vector2f _pointOfView;
        private Vector2f _cursorPosition;
        private readonly List<ConvexShape> _map;
        private readonly List<ConvexShape> _warFog;
        private readonly List<Unit> _units;
        private const int HeightCoefficient = 5;
        private readonly Color _warFogColor;
        private readonly Color _orange;
        private Unit _activeUnit;
        
        public Scene(int seed)
        {
            _warFogColor = new Color(20, 20, 20, 160);
            _orange = new Color(178, 70, 0);
            _units = new List<Unit>();
            _units.Add(new Unit(30, 1, 5, 1, 1, new Vector2f(500, 500), 1, 150, true));
            _units.Add(new Unit(100, 1, 2, 1, 1, new Vector2f(350, 600), 1, 100, false));
            _units[0].Sprite.FillColor = Color.Green;
            _units[1].Sprite.FillColor = Color.Red;
            _depthMap = new byte[SizeX + 1, SizeY + 1];
            _pointOfView = new Vector2f(1000, 25);
            _cursorPosition = new Vector2f(Game.ScreenWidth / 2f, Game.ScreenHeight / 2f);
            var random = new Random(seed);
            for (var x = 0; x <= SizeX; x++)
            for (var y = 0; y <= SizeY; y++)
                _depthMap[x, y] = (byte) random.Next(MaxHeight);
            _map = GenerateMap(_orange);
            _warFog = GenerateMap(_warFogColor);
        }

        private List<ConvexShape> GenerateMap(Color defaultColor)
        {
            var result = new List<ConvexShape>();
            for (var x = 0; x < SizeX; x++)
            for (var y = 0; y < SizeY; y++)
                result.Add(GetPolygon(new Vector2i(x, y), defaultColor));
            return result;
        }

        private static ConvexShape GetPolygon(Vector2i self, Color defaultColor)
        {
            var polygon = new ConvexShape(4);
            var position = GetMapPoint(self.X, self.Y, _depthMap[self.X, self.Y]);
            polygon.SetPoint(0, new Vector2f(0, 0));
            polygon.SetPoint(1, GetMapPoint(self.X + 1, self.Y, _depthMap[self.X + 1, self.Y]) - position);
            polygon.SetPoint(2, GetMapPoint(self.X + 1, self.Y + 1, _depthMap[self.X + 1, self.Y + 1]) - position);
            polygon.SetPoint(3, GetMapPoint(self.X, self.Y + 1, _depthMap[self.X, self.Y + 1]) - position);
            polygon.FillColor = defaultColor;
            polygon.Position = position;
            polygon.OutlineColor = Color.Black;
            polygon.OutlineThickness = 1;
            return polygon;
        }

        private static Vector2f GetMapPoint(float x, float y, byte depth) => new Vector2f(2 * (-x + y) * PolygonSize,
            (x + y) * PolygonSize + depth * HeightCoefficient);

        private static Vector2f ReverseVectorTransform(Vector2f vector) =>
            new Vector2f(vector.X * -0.25f + vector.Y * 0.5f, vector.X * 0.25f + vector.Y * 0.5f);

        public void Update()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                Game.CurrentState = 0;
                return;
            }

            _cursorPosition = (Vector2f) Mouse.GetPosition();
            var movement = Controls.GetArrowsState() * -MapMovingVelocity;
            _pointOfView += movement;

            foreach (var polygon in _map)
            {
                polygon.Position += movement;
                polygon.OutlineColor = Color.Black;
            }

            foreach (var unit in _units)
            {
                if (!unit.Friendly) unit.IsVisible = false;
            }
            
            foreach (var unit in _units)
            {
                unit.Position += movement;
                unit.Destination += movement;
                if (!unit.Friendly) continue;
                unit.Sprite.OutlineColor = Color.Black;
                foreach (var enemy in _units)
                {
                    if (!enemy.Friendly && MathModule.Length(ReverseVectorTransform(unit.Position - enemy.Position)) <
                        unit.ViewRadius)
                        enemy.IsVisible = true;
                }
                if (MathModule.PointInsideRectangle(_cursorPosition, unit.Sprite) && Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    _activeUnit = unit;
                    _activeUnit.Sprite.OutlineColor = Color.White;
                }
            }
            
            if (_activeUnit != null)
            {
                _activeUnit.Sprite.OutlineColor = Color.White;
                if (Mouse.IsButtonPressed(Mouse.Button.Right))
                    _activeUnit.Destination = _cursorPosition;
            }

            foreach (var unit in _units)
            {
                unit.Move();
            }

            foreach (var polygon in _warFog)
            {
                polygon.Position += movement;
                polygon.FillColor = _warFogColor;
                foreach (var unit in _units)
                {
                    if (!unit.Alive || !unit.Friendly) continue;
                    if (MathModule.Length(ReverseVectorTransform(unit.Position - polygon.Position)) <
                        unit.ViewRadius)
                        polygon.FillColor = Color.Transparent;
                }
            }
        }

        public void Display(RenderWindow window)
        {
            foreach (var polygon in _map)
                window.Draw(polygon);
            foreach (var shape in _warFog)
                window.Draw(shape); ;
            foreach (var unit in _units)
                if (unit.IsVisible) 
                    unit.Display(window);
        }
    }
}