﻿using System;
using SFML.Audio;
using SFML.Graphics;
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
        
        /*
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
            
            var arena = new Scene();
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
                        arena.Update();
                        arena.Display(Window);
                        break;
                    case 4:
                        CurrentState = 0;
                        break;
                }

                Window.Display();
            }
        }
*/
        static void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}