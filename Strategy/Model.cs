using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;

namespace strategy
{
    public class SceneModel
    {
        public static int SizeX;
        public static int SizeY;
        public static List<UnitModel> PlayerUnits;
        public static List<UnitModel> EnemyUnits;
        public static List<Bullet> PlayerBullets;
        public static List<Bullet> EnemyBullets;
        public static List<bool> PlayerVisiblePolygons;

        public SceneModel(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            PlayerBullets = new List<Bullet>();
            PlayerUnits = new List<UnitModel>();
            EnemyUnits = new List<UnitModel>();
            EnemyBullets = new List<Bullet>();
            PlayerVisiblePolygons = new List<bool>();
            for (var i = 0; i < SizeX * SizeY; i++)
                PlayerVisiblePolygons.Add(false);
        }

        public void Update(double deltaTime)
        {
            // Пересчёт движений юнитов
            foreach (var unit in PlayerUnits)
                unit.Update(deltaTime);
            foreach (var unit in EnemyUnits)
                unit.Update(deltaTime);
            foreach (var bullet in PlayerBullets)
                bullet.Update();
            foreach (var bullet in EnemyBullets)
                bullet.Update();
            // Корректировка позиций юнитов
            foreach (var unit in PlayerUnits)
                unit.Position = CorrectPosition(unit.Position);
            foreach (var unit in EnemyUnits)
                unit.Position = CorrectPosition(unit.Position);
            // Определение карты видимости
            for (var i = 0; i < SizeX * SizeY; i++)
                PlayerVisiblePolygons[i] = false;
            for (var x = 0; x < SizeX; x++)
            for (var y = 0; y < SizeY; y++)
                foreach (var unit in PlayerUnits)
                    if (unit.PointVisible(x, y))
                        PlayerVisiblePolygons[y * SizeX + x] = true;
            // Произведение выстрелов в сторону противника
            Attack(PlayerUnits, EnemyUnits, PlayerBullets);
            Attack(EnemyUnits, PlayerUnits, EnemyBullets);
            // Фиксация попаданий
            GetShots(PlayerBullets, EnemyUnits);
            GetShots(EnemyBullets, PlayerUnits);
            // Удаление уничтоженных юнитов и снарядов
            PlayerBullets = PlayerBullets
                .Where(bullet => bullet.Exist)
                .ToList();
            EnemyBullets = EnemyBullets
                .Where(bullet => bullet.Exist)
                .ToList();
            PlayerUnits = PlayerUnits
                .Where(unit => unit.Alive)
                .ToList();
            EnemyUnits = EnemyUnits
                .Where(unit => unit.Alive)
                .ToList();
        }

        private static void Attack(List<UnitModel> attaker, List<UnitModel> defender, List<Bullet> bullets)
        {
            foreach (var unit in attaker.Where(unit => unit.ReadyToFire()))
            {
                foreach (var enemyUnit in defender.Where(enemyUnit => enemyUnit.IsVisible))
                    if (unit.AbleToFire(enemyUnit.Position))
                    {
                        bullets.Add(new Bullet(unit.Position, enemyUnit.Position, unit.Damage, true));
                        break;
                    }
            }
        }

        private static void GetShots(List<Bullet> bullets, List<UnitModel> units)
        {
            foreach (var bullet in bullets)
            {
                foreach (var unit in units)
                {
                    if (MathModule.Length(bullet.Position - unit.Position) < 10)
                    {
                        bullet.Exist = false;
                        unit.GetShot(bullet.Damage);
                    }
                }
            }
        }

        public static Vector2f CorrectPosition(Vector2f position) => new Vector2f(Math.Min(Math.Max(position.X, 0), SizeX),
            Math.Min(Math.Max(position.Y, 0), SizeY));
    }

    public class UnitModel
    {
        private int _health;
        private readonly int _maxHealth;
        public readonly int Damage;
        private readonly double _speed;
        private readonly double _maxRange;
        private readonly double _minRange;
        public readonly double ReloadTime;
        private DateTime _previousShotTime;
        private readonly double _viewRadius;
        public Vector2f Position;
        public Vector2f Destination;
        public bool IsVisible;

        public UnitModel(int health, int damage, double speed, double maxRange, double minRange, Vector2f position,
            double reloadTime, double viewRadius)
        {
            IsVisible = true;
            _health = health;
            _maxHealth = health;
            Damage = damage;
            _speed = speed;
            _maxRange = maxRange;
            _minRange = minRange;
            ReloadTime = reloadTime;
            _previousShotTime = DateTime.Now;
            _viewRadius = viewRadius;
            Destination = position;
            Position = position;
        }

        public bool ReadyToFire() => (DateTime.Now - _previousShotTime).TotalMilliseconds > ReloadTime;

        public bool AbleToFire(Vector2f target)
        {
            var distance = MathModule.Length(target - Position);
            return (distance > _minRange) && (distance < _maxRange);
        }

        public bool PointVisible(double x, double y) =>
            MathModule.Hypot(this.Position.X - x, this.Position.Y - y) < this._viewRadius;

        public void Move()
        {
            var delta = Destination - Position;
            var wayLength = MathModule.Length(delta);
            if (wayLength < 0.01) return;
            if (wayLength < _speed)
            {
                Position = Destination;
            }
            else
            {
                var move = delta * (float) (_speed / wayLength);
                Position += move;
            }
        }

        public void SetDestination(Vector2f to)
        {
            Destination = to;
        }
        
        public void Update(double deltaTime)
        {
        }

        public void Fire() => _previousShotTime = DateTime.Now;

        public void GetShot(int incomingDamage) => _health -= incomingDamage;

        public bool Alive => _health > 0;

        public double ViewRadius => _viewRadius;
    }
}