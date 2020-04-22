using SFML.System;

namespace strategy
{
    public class Bullet
    {
        public bool friendly;
        public Vector2f origin;
        public Vector2f direction;
        public Vector2f position;
        public double maxDistance;
        public float speed;
        public bool exist;
        
        public Bullet(bool a)
        {
            friendly = a;
        }

        public void Move()
        {
            position += speed * direction;
            if (MathModule.Length(position - origin) > maxDistance)
            {
                exist = false;
            }
        }
    }
}