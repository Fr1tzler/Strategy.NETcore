using SFML.System;

namespace strategy
{
    public class Scout : UnitModel
    {
        public Scout(Vector2f position, bool friendly) : base(50, 10, 2, 7, 0, position, 800, 8)
        {
        }
    }
    
    public class Tank : UnitModel
    {
        public Tank(Vector2f position, bool friendly) : base(100, 15, 1, 5, 0, position, 1200, 6)
        {
        }
    }
    
    public class Base : UnitModel
    {
        public Base(Vector2f position, bool friendly) : base(1500, 0, 0, 0, 0, new Vector2f(100, 100), 1000, 5)
        {
            Position = position;
        }
    } 
}