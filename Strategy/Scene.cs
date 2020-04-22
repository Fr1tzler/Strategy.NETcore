using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace strategy
{
    public class Scene
    {
        private const float PolygonSize = 20;
        private const int SizeX = 40;
        private const int SizeY = 40;
        private const float MapMovingVelocity = 20;
        private readonly List<ConvexShape> _map;
        private readonly List<ConvexShape> _warFog;
        private List<Unit> _playerUnits;
        private List<Unit> _enemyUnits;
        private List<Bullet> _bullets;
        private Unit _activeUnit;

        public Scene(int seed)
        {
            _bullets = new List<Bullet>();
            _playerUnits = new List<Unit>();
            _playerUnits.Add(new Base(new Vector2f(150, 570), true));
            _playerUnits.Add(new Scout(new Vector2f(300, 570), true));
            _playerUnits.Add(new Scout(new Vector2f(240, 570), true));
            _playerUnits.Add(new Tank(new Vector2f(280, 630), true));
            _playerUnits.Add(new Tank(new Vector2f(280, 510), true));

            _enemyUnits = new List<Unit>();
            _enemyUnits.Add(new Base(new Vector2f(3000, 570), false));
            _enemyUnits.Add(new Tank(new Vector2f(2800, 570), false));
            _enemyUnits.Add(new Tank(new Vector2f(2600, 570), false));
            _enemyUnits.Add(new Tank(new Vector2f(2700, 500), false));
            _enemyUnits.Add(new Tank(new Vector2f(2700, 640), false));
            _enemyUnits.Add(new Tank(new Vector2f(2700, 570), false));
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

        private static Vector2f GetMapPoint(float x, float y) => new Vector2f(2 * (-x + y) * PolygonSize, (x + y) * PolygonSize);
        
        public void Update()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                Game.CurrentState = 0;
                return;
            }

            var cursorPosition = (Vector2f) Mouse.GetPosition();
            var movement = Controls.GetArrowsState() * -MapMovingVelocity;

            foreach (var polygon in _map) 
                polygon.Position += movement;

            foreach (var unit in _enemyUnits) 
                unit.IsVisible = false;
            
            foreach (var unit in _enemyUnits)
            {
                unit.Position += movement;
                unit.Destination += movement;
            }

            foreach (var unit in _playerUnits)
            {
                unit.Position += movement;
                unit.Destination += movement;
            }

            foreach (var unit in _playerUnits)
            {
                var gridPosition = MathModule.ReverseVectorTransform(unit.Position - _map[0].Position);
                var x = Math.Min(Math.Max(gridPosition.X, 30), 750);
                var y = Math.Min(Math.Max(gridPosition.Y, 20), 750);
                Console.WriteLine(x + " " + y);
                unit.Position = new Vector2f(2 * (-x + y), x + y) + _map[0].Position;
            }

            foreach (var unit in _enemyUnits)
            {
                var gridPosition = MathModule.ReverseVectorTransform(unit.Position - _map[0].Position);
                var x = Math.Min(Math.Max(gridPosition.X, 30), 750);
                var y = Math.Min(Math.Max(gridPosition.Y, 20), 750);
                unit.Position = new Vector2f(2 * (-x + y), x + y) + _map[0].Position;
            }

            
            foreach (var unit in _playerUnits)
            {
                unit.Sprite.OutlineColor = Color.Black;
                foreach (var enemy in _enemyUnits)
                    if (MathModule.Length(MathModule.ReverseVectorTransform(unit.Position - enemy.Position)) < unit.ViewRadius)
                        enemy.IsVisible = true;
                if (MathModule.PointInsideRectangle(cursorPosition, unit.Sprite) && Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    _activeUnit = unit;
                    _activeUnit.Sprite.OutlineColor = Color.White;
                }
            }
            
            if (_activeUnit != null)
            {
                _activeUnit.Sprite.OutlineColor = Color.White;
                if (Mouse.IsButtonPressed(Mouse.Button.Right))
                    _activeUnit.Destination = cursorPosition;
            }

            foreach (var unit in _playerUnits)
                unit.Move();
            foreach (var unit in _enemyUnits)
                unit.Move();

            var warFogColor = new Color(20, 20, 20, 160);
            foreach (var polygon in _warFog)
            {
                polygon.Position += movement;
                polygon.FillColor = warFogColor;
                foreach (var unit in _playerUnits)
                {
                    if (!unit.Alive) continue;
                    if (MathModule.Length(MathModule.ReverseVectorTransform(unit.Position - polygon.Position)) <
                        unit.ViewRadius)
                        polygon.FillColor = Color.Transparent;
                }
            }
            
            foreach (var bullet in _bullets)
            {
                bullet.Update();
            }
            
            _playerUnits = _playerUnits
                .Where(unit => unit.Alive)
                .ToList();
            _enemyUnits = _enemyUnits
                .Where(unit => unit.Alive)
                .ToList();
            _bullets = _bullets
                .Where(unit => unit.Exist)
                .ToList();
            
            foreach (var playerUnit in _playerUnits)
            {
                if (playerUnit.Damage == 0) continue;
                if (!playerUnit.ReadyToFire()) continue;
                foreach (var enemyUnit in _enemyUnits)
                {
                    if (playerUnit.AbleToFire(enemyUnit.Position))
                    {
                        playerUnit.Fire();
                        _bullets.Add(new Bullet(playerUnit.Position, enemyUnit.Position, true));
                        enemyUnit.GetShot(playerUnit.Damage);
                        break;
                    }
                }
            }
            
            foreach (var enemyUnit in _enemyUnits)
            {
                if (enemyUnit.Damage == 0) continue;
                if (!enemyUnit.ReadyToFire()) continue;
                foreach (var playerUnit in _playerUnits)
                {
                    if (enemyUnit.AbleToFire(playerUnit.Position))
                    {
                        enemyUnit.Fire();
                        _bullets.Add(new Bullet(enemyUnit.Position, playerUnit.Position, false));
                        playerUnit.GetShot(enemyUnit.Damage);
                        break;
                    }
                }
            }
            
        }

        public void Display(RenderWindow window)
        {
            foreach (var polygon in _map)
                window.Draw(polygon);
            foreach (var shape in _warFog)
                window.Draw(shape); ;
            foreach (var unit in _playerUnits)
                unit.Display(window);
            foreach (var unit in _enemyUnits)//.Where(unit => unit.IsVisible))
                unit.Display(window);
            foreach (var bullet in _bullets)
                bullet.Display(window);
        }
    }
}