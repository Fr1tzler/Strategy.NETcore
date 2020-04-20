using SFML.System;
using SFML.Window;

namespace strategy
{
    public static class Controls
    {
        public static Vector2f GetArrowsState()
        {
            var horizontal = 0;
            var vertical = 0;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.Up))
                vertical = -1;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S) || Keyboard.IsKeyPressed(Keyboard.Key.Down))
                vertical = 1;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.Left))
                horizontal = -1;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D) || Keyboard.IsKeyPressed(Keyboard.Key.Right))
                horizontal = 1;

            return new Vector2f(horizontal, vertical);
        }

        public static Vector2f GetMousePosition() => (Vector2f)(Mouse.GetPosition() - Game.Window.Position);
    }
}