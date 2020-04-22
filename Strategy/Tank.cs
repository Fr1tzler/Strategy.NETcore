using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Tank : Unit
    {
        public Tank(Vector2f position, bool friendly) : base(100, 15, 3, 90, 0, position, 1200, 100)
        {
            Sprite.FillColor = friendly ? Color.Green : Color.Red;
            Sprite.Texture = new Texture("res/images/tank.png");
        }
    }
}