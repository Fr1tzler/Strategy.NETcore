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
        private double _viewRadius;
        public CircleShape sprite;
        public RectangleShape healthBar;

        public Unit(int health, int damage, double speed, double maxRange, double minRange, Vector2f position, double reloadTime, double viewRadius)
        {
            _health = health;
            _maxHealth = health;
            _damage = damage;
            _speed = speed;
            _maxRange = maxRange;
            _minRange = minRange;
            _reloadTime = reloadTime;
            _previousShotTime = DateTime.Now;
            _viewRadius = viewRadius;
            sprite = new CircleShape
            {
                Radius = 10,
                FillColor = Color.Cyan,
                OutlineColor = Color.Black,
                OutlineThickness = 2,
                Position = position
            };
            healthBar = new RectangleShape
            {
                Size =  new Vector2f(50, 5),
                FillColor = Color.Green,
                Origin = new Vector2f(0, 20),
                Position = position - new Vector2f(10, 20),
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };
        }

        public bool ReadyToFire(DateTime time)
        {
            return (time - _previousShotTime).TotalMilliseconds > _reloadTime;
        }

        public bool AbleToFire(Vector2f target)
        {
            var distance = MathModule.Length(target - sprite.Position);
            return (distance > _minRange) && (distance < _maxRange);
        }

        public void Move(Vector2f destination)
        {
            var delta = destination - sprite.Position;
            var wayLength = MathModule.Length(delta);
            if (wayLength > 0.1)
            {
                if (wayLength < _speed)
                {
                    sprite.Position = destination;
                    healthBar.Position = destination;
                }
                else
                {
                    var move = delta * (float) (_speed / wayLength);
                    healthBar.Position += move;
                    sprite.Position += move;
                }
            }
        }

        public void Display(RenderWindow window)
        {
            if (!Alive) return;
            window.Draw(sprite);
            window.Draw(healthBar);
        }
        
        public void Fire()
        {
            _previousShotTime = DateTime.Now;
        }

        public void GetShot(int incomingDamage)
        {    
            _health -= incomingDamage;
            var healthCoefficient = _health / (float) _maxHealth;
            healthBar.Size = new Vector2f(healthCoefficient * 50f, 5);
            if (healthCoefficient < 0.6f) healthBar.FillColor = Color.Yellow;
            if (healthCoefficient < 0.2) healthBar.FillColor = Color.Red;
        }
        
        public bool Alive => _health > 0;
        
        public double ViewRadius => _viewRadius;

        public Vector2f Position
        {
            get => sprite.Position;
            set
            {
                sprite.Position = value;
                healthBar.Position = value;
            }
        }
    }
}