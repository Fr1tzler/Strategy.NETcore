﻿using System;
 using System.Collections.Generic;
 using SFML.Audio;
using SFML.Graphics;
 using SFML.System;
 using SFML.Window;

namespace strategy
{
    internal static class Game
    {
        public static RenderWindow Window;
        public static uint ScreenWidth;
        public static uint ScreenHeight;
        private const string Title = "MARS CONQUEST";
        public static int CurrentState;
        
        /*
         * -1 - exit game
         * 0 - main menu
         * 1 - options
         * 2 - level select
         * 3 - arena
         * 4 - result
         */
        
        
        public static void Main()
        {
            ScreenWidth = VideoMode.DesktopMode.Width;
            ScreenHeight = VideoMode.DesktopMode.Height;
            Window = new RenderWindow(new VideoMode(ScreenWidth, ScreenHeight), Title, Styles.Fullscreen);
            Window.SetMouseCursorVisible(true);
            Window.Closed += Window_Closed;

            var sound = new Sound
            {
                SoundBuffer = new SoundBuffer("res/music/mainTrack.flac"),
                Loop = true
            };
            sound.Play();
            
            var model = new SceneModel(20, 20);
            var view = new View();
            
            SceneModel.PlayerUnits.Add(new Base(new Vector2f(150, 570), true));
            SceneModel.PlayerUnits.Add(new Scout(new Vector2f(300, 570), true));
            SceneModel.PlayerUnits.Add(new Scout(new Vector2f(240, 570), true));
            SceneModel.PlayerUnits.Add(new Tank(new Vector2f(280, 630), true));
            SceneModel.PlayerUnits.Add(new Tank(new Vector2f(280, 510), true));
            
            SceneModel.EnemyUnits.Add(new Base(new Vector2f(3000, 570), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(2800, 570), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(2600, 570), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(2700, 500), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(2700, 640), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(2700, 570), false));
            
            var menu = new MenuPage();
            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Window.Clear(Color.Black);
                
                switch (CurrentState)
                {
                    case -1:
                        Window.Close();
                        break;
                    case 0:
                        menu.Update();
                        menu.Display(Window);
                        break;
                    case 1:
                        CurrentState = 0;
                        break;
                    case 2:
                        CurrentState = 0;
                        break;
                    case 3:
                        model.Update(1);
                        view.Display(Window);
                        break;
                    case 4:
                        CurrentState = 0;
                        break;
                }

                Window.Display();
            }
        }

        static void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}