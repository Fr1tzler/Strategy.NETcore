using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Scout : Unit
    {
        public Scout(Vector2f position, bool friendly) : base(50, 10, 50, 130, 0, position, 800, 150)
        {
            Sprite.FillColor = friendly ? Color.Green : Color.Red;
            Sprite.Texture = new Texture("res/images/scout.png");
        }
    }
}