using System;
using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Unit
    {
        private int _health;
        private readonly int _damage;
        private readonly double _speed;
        private readonly double _maxRange;
        private readonly  double _minRange;
        private readonly  double _reloadTime;
        private DateTime _previousShotTime;
        private double _viewRadius;
        public CircleShape sprite;
        
        public Unit(int health, int damage, double speed, double maxRange, double minRange, Vector2f position, double reloadTime, double viewRadius)
        {
            _health = health;
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
            if (sprite.Position != destination)
            {
                
            }
        }
        
        public void Fire()
        {
            _previousShotTime = DateTime.Now;
        }

        public void GetShot(int incomingDamage) => _health -= incomingDamage;
        
        public bool Alive => _health > 0;
        
        public double ViewRadius => _viewRadius;
    }
}