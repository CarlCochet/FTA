using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using TGUI;

namespace FTA
{
    class SimpleWindow
    {
        private Vector2i start = new Vector2i(0, 0);
        private Vector2i target = new Vector2i(0, 0);
        private ArenaMap arenaMap = new ArenaMap();
        private SFML.Graphics.RenderWindow window;
        private GameState state;

        public void Run()
        {
            var mode = new SFML.Window.VideoMode(Utils.WINDOW_WIDTH, Utils.WINDOW_HEIGHT);
            window = new SFML.Graphics.RenderWindow(mode, "Fantasy Tactic Arena");

            window.Closed += (_, __) => window.Close();
            window.KeyPressed += Window_KeyPressed;
            window.MouseMoved += Window_MouseMoved;
            window.MouseButtonPressed += Window_MouseButtonPressed;

            state = GameState.MAINMENU;

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear();

                Gui gui = CreateMainMenu();

                if (state == GameState.INGAME)
                {
                    arenaMap.Render(window);
                    if (start.X >= 0 && start.X < Utils.SIZE_MAP_X && start.Y >= 0 && start.Y < Utils.SIZE_MAP_Y && target.X >= 0 && target.X < Utils.SIZE_MAP_X && target.Y >= 0 && target.Y < Utils.SIZE_MAP_Y)
                    {
                        arenaMap.RenderLOS(window, target.X, target.Y, 8);
                    }
                }
                if (state == GameState.MAINMENU)
                {
                    gui.Draw();
                }

                window.Display();
            }
            
        }

        public void CloseWindow()
        {
            window.Close();
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

        public void SwitchState(GameState newState)
        {
            this.state = newState;
        }

        public Gui CreateMainMenu()
        {
            Gui gui = new Gui();

            /*var picture = new Picture("background.jpg");
            picture.Size = new Vector2f(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y);
            gui.Add(picture);*/

            var buttonStart = new Button("Launch fight")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonStart);

            buttonStart.Pressed += (s, e) => SwitchState(GameState.PLACEMENT);


            var buttonTeam = new Button("Manage Team")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 2 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonTeam);

            buttonTeam.Pressed += (s, e) => SwitchState(GameState.MANAGETEAM);

            var buttonOption = new Button("Options")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 3 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonOption);

            buttonOption.Pressed += (s, e) => SwitchState(GameState.OPTION);

            var buttonQuit = new Button("Exit")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 4 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonQuit);

            buttonQuit.Pressed += (s, e) => CloseWindow();


            return gui;
        }
    }
}
