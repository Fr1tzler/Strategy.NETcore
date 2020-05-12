using SFML.Graphics;
using SFML.System;

namespace strategy
{
    public class Bullet
    {
        private Vector2f _from;
        private Vector2f _to;
        private CircleShape _self;
        private int _iteration;
        public bool Exist;
        public readonly int Damage;
        
        public Bullet(Vector2f from, Vector2f to, int damage, bool friendly)
        {
            _self = new CircleShape{
                Position =  from,
                Radius = 3,
                Origin = new Vector2f(3,3),
                FillColor = friendly ? Color.Green : Color.Red,
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };
            _iteration = 1;
            _from = from;
            _to = to;
            Exist = true;
            Damage = damage;
        }

        public void Update()
        {
            _self.Position = _from * (3 - _iteration) / 3 + _to * _iteration / 3;
            _iteration++;
            if (_iteration > 5) 
                Exist = false;
        }

        public void Display(RenderWindow window) => window.Draw(_self);

        public Vector2f Position
        {
            get => _self.Position;
        }
    }
}