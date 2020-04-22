using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Tank : Unit
    {
        public Tank(Vector2f position, bool friendly) : base(100, 15, 2, 90, 0, position, 800, 100, friendly)
        {
            Sprite.FillColor = friendly ? Color.Green : Color.Red;
            Sprite.Texture = new Texture("res/images/tank.png");
        }
    }
}