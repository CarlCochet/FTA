using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

namespace FTA
{
    class SimpleWindow
    {
        Vector2i start = new Vector2i(0, 0);
        Vector2i target = new Vector2i(0, 0);
        ArenaMap arenaMap = new ArenaMap();

        public void Run()
        {
            var mode = new SFML.Window.VideoMode(Utils.WINDOW_WIDTH, Utils.WINDOW_HEIGHT);
            var window = new SFML.Graphics.RenderWindow(mode, "Fantasy Tactic Arena");

            window.Closed += (_, __) => window.Close();
            window.KeyPressed += Window_KeyPressed;
            window.MouseMoved += Window_MouseMoved;
            window.MouseButtonPressed += Window_MouseButtonPressed;

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear();

                arenaMap.Render(window);
                if (start.X >= 0 && start.X < Utils.SIZE_MAP_X && start.Y >= 0 && start.Y < Utils.SIZE_MAP_Y && target.X >= 0 && target.X < Utils.SIZE_MAP_X && target.Y >= 0 && target.Y < Utils.SIZE_MAP_Y)
                {
                    arenaMap.RenderPath(window, start.X, start.Y, target.X, target.Y);
                }

                window.Display();
            }
            
        }

        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

        private void Window_MouseMoved(object sender, SFML.Window.MouseMoveEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            target.X = e.X / Utils.SQUARE_SIZE;
            target.Y = e.Y / Utils.SQUARE_SIZE;
        }

        private void Window_MouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                start.X = e.X / Utils.SQUARE_SIZE;
                start.Y = e.Y / Utils.SQUARE_SIZE;
            }
        }
    }
}
