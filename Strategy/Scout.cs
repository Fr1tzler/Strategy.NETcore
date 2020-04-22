using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Scout : Unit
    {
        public Scout(Vector2f position, bool friendly) : base(30, 10, 5, 130, 0, position, 500, 150, friendly)
        {
            Sprite.FillColor = friendly ? Color.Green : Color.Red;
            Sprite.Texture = new Texture("res/images/tank.png");
        }
    }
}