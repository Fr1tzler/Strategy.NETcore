using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;

namespace strategy
{
    public class StateModel
    {
        public readonly int SizeX;
        public readonly int SizeY;
        public static List<Unit> PlayerUnits;
        public static List<Unit> EnemyUnits;
        public static List<Bullet> PlayerBullets;
        public static List<Bullet> EnemyBullets;
        public static List<bool> PlayerVisiblePolygons;
        
        public StateModel(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            PlayerBullets = new List<Bullet>();
            PlayerUnits = new List<Unit>();
            EnemyUnits = new List<Unit>();
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
                    if (unit.Visible(x, y))
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

        private static void Attack(List<Unit> attaker, List<Unit> defender, List<Bullet> bullets)
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

        private static void GetShots(List<Bullet> bullets, List<Unit> units)
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
        
        private Vector2f CorrectPosition(Vector2f position) => new Vector2f(Math.Min(Math.Max(position.X, 0), SizeX),
            Math.Min(Math.Max(position.Y, 0), SizeY));
    }
}