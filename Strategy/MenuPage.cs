using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace strategy
{
    public class MenuPage
    {
        private readonly Text _label;
        private readonly RectangleShape _background;
        private readonly Text _startLabel;
        private readonly Text _settingsLabel;
        private readonly Text _exitLabel;
        private readonly RectangleShape _startButton;
        private readonly RectangleShape _settingsButton;
        private readonly RectangleShape _exitButton;
        private readonly DateTime _startTime;


        public MenuPage()
        {
            _startTime = DateTime.Now;
            var font = new Font("res/fonts/labelFont.otf");

            _background = new RectangleShape
            {
                Size = new Vector2f(2880, 1800),
                Texture = new Texture("res/images/background.png"),
                Position = new Vector2f(-100, -100)
            };

            _label = new Text
            {
                Font = font,
                DisplayedString = "MARS CONQUEST",
                Position = ScaleConvert(new Vector2f(200, 200)),
                CharacterSize = 200 * Game.ScreenHeight / 1080,
                FillColor = Color.White,
            };

            _startLabel = new Text
            {
                Font = font,
                DisplayedString = "Start",
                Position = ScaleConvert(new Vector2f(200, 750)),
                CharacterSize = 100 * Game.ScreenHeight / 1080,
                FillColor = Color.White,
            };

            _settingsLabel = new Text
            {
                Font = font,
                DisplayedString = "Settings",
                Position = ScaleConvert(new Vector2f(200, 900)),
                CharacterSize = 60 * Game.ScreenHeight / 1080,
                FillColor = Color.White,
            };

            _exitLabel = new Text
            {
                Font = font,
                DisplayedString = "exit",
                Position = ScaleConvert(new Vector2f(1800, 910)),
                CharacterSize = 60 * Game.ScreenHeight / 1080,
                FillColor = Color.White,
            };

            _startButton = new RectangleShape
            {
                FillColor = new Color(20, 20, 20, 190),
                Size = ScaleConvert(new Vector2f(200, 110)),
                Position = ScaleConvert(new Vector2f(185, 760))
            };

            _settingsButton = new RectangleShape
            {
                FillColor = new Color(20, 20, 20, 190),
                Size = ScaleConvert(new Vector2f(250, 70)),
                Position = ScaleConvert(new Vector2f(185, 900))
            };

            _exitButton = new RectangleShape
            {
                FillColor = new Color(20, 20, 20, 190),
                Size = ScaleConvert(new Vector2f(200, 80)),
                Position = ScaleConvert(new Vector2f(1790, 910))
            };
        }

        private static Vector2f ScaleConvert(Vector2f input) => input * Game.ScreenWidth / 1920;

        public void Update()
        {
            var timeElapsed = (DateTime.Now - _startTime).Seconds;
            var shift = new Vector2f(-1, -1);
            _background.Position += shift * (float) Math.Sin(timeElapsed / 10d) / 25f;
            if (!Mouse.IsButtonPressed(Mouse.Button.Left)) return;
            var mousePosition = Controls.GetMousePosition();
            if (MathModule.PointInsideRectangle(mousePosition, _settingsButton))
                Game.CurrentState = 1;
            if (MathModule.PointInsideRectangle(mousePosition, _startButton))
                Game.CurrentState = 3;
            if (MathModule.PointInsideRectangle(mousePosition, _exitButton))
                Game.CurrentState = -1;
        }

        public void Display(RenderWindow window)
        {
            window.Draw(_background);
            window.Draw(_label);
            window.Draw(_startButton);
            window.Draw(_settingsButton);
            window.Draw(_exitButton);
            window.Draw(_startLabel);
            window.Draw(_settingsLabel);
            window.Draw(_exitLabel);
        }
    }
}