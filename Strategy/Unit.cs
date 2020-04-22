using System;
using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Unit
    {
        private int _health;
        private readonly int _maxHealth;
        private readonly int _damage;
        private readonly double _speed;
        private readonly double _maxRange;
        private readonly  double _minRange;
        private readonly  double _reloadTime;
        private DateTime _previousShotTime;
        private readonly double _viewRadius;
        public readonly RectangleShape Sprite;
        private readonly RectangleShape _healthBar;
        public Vector2f Destination;
        public readonly bool Friendly;
        public bool IsVisible;

        public Unit(int health, int damage, double speed, double maxRange, double minRange, Vector2f position, double reloadTime, double viewRadius, bool friendly)
        {
            IsVisible = true;
            _health = health;
            _maxHealth = health;
            _damage = damage;
            _speed = speed;
            _maxRange = maxRange;
            _minRange = minRange;
            _reloadTime = reloadTime;
            _previousShotTime = DateTime.Now;
            _viewRadius = viewRadius;
            Destination = position;
            Sprite = new RectangleShape
            {
                Size = new Vector2f(20,20),
                FillColor = Color.Black,
                OutlineColor = Color.Black,
                OutlineThickness = 2,
                Position = position
            };
            _healthBar = new RectangleShape
            {
                Size =  new Vector2f(50, 5),
                FillColor = Color.Green,
                Origin = new Vector2f(0, 20),
                Position = position,
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };
            Friendly = friendly;
        }

        public bool ReadyToFire(DateTime time)
        {
            return (time - _previousShotTime).TotalMilliseconds > _reloadTime;
        }

        public bool AbleToFire(Vector2f target)
        {
            var distance = MathModule.Length(target - Sprite.Position);
            return (distance > _minRange) && (distance < _maxRange);
        }

        public void Move()
        {
            var delta = Destination - Sprite.Position;
            var wayLength = MathModule.Length(delta);
            if (wayLength > 0.1)
            {
                if (wayLength < _speed)
                {
                    Sprite.Position = Destination;
                    _healthBar.Position = Destination;
                }
                else
                {
                    var move = delta * (float) (_speed / wayLength);
                    _healthBar.Position += move;
                    Sprite.Position += move;
                }
            }
        }

        public void Display(RenderWindow window)
        {
            if (!Alive) return;
            window.Draw(Sprite);
            window.Draw(_healthBar);
        }
        
        public void Fire()
        {
            _previousShotTime = DateTime.Now;
        }

        public void GetShot(int incomingDamage)
        {    
            _health -= incomingDamage;
            var healthCoefficient = _health / (float) _maxHealth;
            _healthBar.Size = new Vector2f(healthCoefficient * 50f, 5);
            if (healthCoefficient < 0.6) _healthBar.FillColor = Color.Yellow;
            if (healthCoefficient < 0.4) _healthBar.FillColor = Color.Red;
        }
        
        public bool Alive => _health > 0;
        
        public double ViewRadius => _viewRadius;

        public Vector2f Position
        {
            get => Sprite.Position;
            set
            {
                Sprite.Position = value;
                _healthBar.Position = value;
            }
        }
    }
}