using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Base : Unit
    {
        public Base(Vector2f position, bool friendly) : base(1500, 0, 0, 0, 0, new Vector2f(100, 100), 1000, 100, friendly)
        {
            Sprite.FillColor = friendly ? Color.Green : Color.Red;
            Sprite.Position = position;
        }
    }
}