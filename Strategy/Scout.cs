using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Scout : Unit
    {
        public Scout(Vector2f position, bool friendly) : base(30, 1, 5, 100, 0, position, 1, 150, friendly)
        {
            Sprite.FillColor = friendly ? Color.Green : Color.Red;
            Sprite.Texture = new Texture("res/images/tank.png");
        }
    }
}