using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Tank : Unit
    {
        private Color color;
        public Tank(Vector2f position, bool friendly) : base(100, 1, 2, 90, 0, position, 1, 100, friendly)
        {
            Sprite.FillColor = friendly ? Color.Green : Color.Red;
        }
    }
}