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
            CurrentState = 3;
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
            
            var model = new SceneModel(40, 40);
            var view = new View();
            
            SceneModel.PlayerUnits.Add(new Base(new Vector2f(1, 1), true));
            SceneModel.PlayerUnits.Add(new Tank(new Vector2f(6, 1), true));
            SceneModel.PlayerUnits.Add(new Tank(new Vector2f(1, 4), true));
            SceneModel.PlayerUnits.Add(new Scout(new Vector2f(12, 12), true));
            SceneModel.PlayerUnits.Add(new Scout(new Vector2f(1, 8), true));
            
            SceneModel.EnemyUnits.Add(new Base(new Vector2f(39, 39), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(32, 39), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(39, 32), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(34, 34), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(35, 37), false));
            SceneModel.EnemyUnits.Add(new Tank(new Vector2f(37, 35), false));
            
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
                        model.Update(0.1f);
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