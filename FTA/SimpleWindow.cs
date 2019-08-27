﻿using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using TGUI;

namespace FTA
{
    class SimpleWindow
    {
        private Vector2i start = new Vector2i(0, 0);    // Used to store a starting point (pathfinding / LoS)
        private Vector2i target = new Vector2i(0, 0);   // Used to store a target point (pathfinding)
        private ArenaMap arenaMap = new ArenaMap();     // Arena object that stores the arena data and provides utils
        private SFML.Graphics.RenderWindow window;      // The main window
        private GameState state;                        // Game state to know what to display / process
        private Gui gui;
        
        private int aaQuality = 0;
        private int effectQuality = 0;

        public void Run()
        {
            // For now, size is fixed
            var mode = new SFML.Window.VideoMode(Utils.WINDOW_WIDTH, Utils.WINDOW_HEIGHT);
            window = new SFML.Graphics.RenderWindow(mode, "Fantasy Tactic Arena");

            // Get the keyboard/mouse events
            window.Closed += (_, __) => window.Close();
            window.KeyPressed += Window_KeyPressed;
            window.MouseMoved += Window_MouseMoved;
            window.MouseButtonPressed += Window_MouseButtonPressed;

            // Set initial state and create the main menu
            state = GameState.MAINMENU;
            gui = CreateMainMenu();

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear();

                // If in a game, display the map and process game mechanics
                if (state == GameState.INGAME)
                {
                    arenaMap.Render(window);
                    if (start.X >= 0 && start.X < Utils.SIZE_MAP_X && start.Y >= 0 && start.Y < Utils.SIZE_MAP_Y && target.X >= 0 && target.X < Utils.SIZE_MAP_X && target.Y >= 0 && target.Y < Utils.SIZE_MAP_Y)
                    {
                        arenaMap.RenderLOS(window, target.X, target.Y, 8);
                    }
                }

                if (state == GameState.ESCAPEMENU || state == GameState.INGAMEOPTION)
                {
                    arenaMap.Render(window);
                    gui.Draw();
                }

                // If in main menu, display the main menu (BROKEN, do not know why...)
                if (state == GameState.MAINMENU || state == GameState.OPTION || state == GameState.MANAGETEAM)
                {
                    gui.Draw();
                }

                window.Display();
            }
            
        }

        // Close the program
        public void CloseWindow()
        {
            window.Close();
        }

        // Manage Keyboard events
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;

            // Does not work, do not know why...
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                if(this.state == GameState.OPTION || this.state == GameState.MANAGETEAM)
                {
                    SwitchState(GameState.MAINMENU);
                    gui = CreateMainMenu();
                }
                if (this.state == GameState.INGAME || this.state == GameState.PLACEMENT)
                {
                    SwitchState(GameState.ESCAPEMENU);
                    gui = CreateEscapeMenu();
                }
            }
        }

        // Manage Mouse Move events
        private void Window_MouseMoved(object sender, SFML.Window.MouseMoveEventArgs e)
        {
            var window = (SFML.Window.Window)sender;

            // Just set the new target coords everytime the mouse move
            target.X = e.X / Utils.SQUARE_SIZE;
            target.Y = e.Y / Utils.SQUARE_SIZE;
        }

        // Manage Mouse Button Pressed events
        private void Window_MouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            var window = (SFML.Window.Window)sender;

            // Just set the new start coords eveytime the Left mouse button is pressed
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                start.X = e.X / Utils.SQUARE_SIZE;
                start.Y = e.Y / Utils.SQUARE_SIZE;
            }
        }

        // Changing the state of the game. Will probably implement an object creation for GUIs
        public void SwitchState(GameState newState)
        {
            if (newState == GameState.MAINMENU)
            {
                this.gui = CreateMainMenu();
            }
            if (newState == GameState.ESCAPEMENU)
            {
                this.gui = CreateEscapeMenu();
            }
            if (newState == GameState.OPTION || newState == GameState.INGAMEOPTION)
            {
                this.gui = CreateOptionMenu();
            }

            this.state = newState;
        }

        // Creates the main menu GUI
        public Gui CreateMainMenu()
        {
            Gui gui = new Gui(this.window);

            var buttonStart = new Button("Launch fight")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonStart);

            buttonStart.Pressed += (s, e) => SwitchState(GameState.INGAME);


            var buttonTeam = new Button("Manage Team")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 2 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonTeam);

            buttonTeam.Pressed += (s, e) => SwitchState(GameState.MANAGETEAM);

            var buttonOption = new Button("Options")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 3 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonOption);

            buttonOption.Pressed += (s, e) => SwitchState(GameState.OPTION);

            var buttonQuit = new Button("Exit")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 4 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonQuit);

            buttonQuit.Pressed += (s, e) => CloseWindow();


            return gui;
        }

        // Creates the in-game menu GUI
        public Gui CreateEscapeMenu()
        {
            Gui gui = new Gui(this.window);

            var buttonStart = new Button("Resume")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 2 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonStart);

            buttonStart.Pressed += (s, e) => SwitchState(GameState.INGAME);

            var buttonOption = new Button("Options")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 3 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonOption);

            buttonOption.Pressed += (s, e) => SwitchState(GameState.INGAMEOPTION);

            var buttonQuit = new Button("Exit to main menu")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 4 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonQuit);

            buttonQuit.Pressed += (s, e) => SwitchState(GameState.MAINMENU);


            return gui;
        }

        // Creates the Option menu GUI (not working properly)
        public Gui CreateOptionMenu()
        {
            Gui gui = new Gui(this.window);

            var buttonAA = new Button("")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 2 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10),
                Text = AA2STR()
            };
            gui.Add(buttonAA);

            buttonAA.Pressed += (s, e) => UpAA(ref buttonAA);

            var buttonEffect = new Button("")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 3 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10),
                Text = Effect2Str()
            };
            gui.Add(buttonEffect);

            buttonEffect.Pressed += (s, e) => UpEffect(ref buttonEffect);

            var buttonQuit = new Button("Back")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 4 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonQuit);
            
            if (state == GameState.OPTION)
                buttonQuit.Pressed += (s, e) => SwitchState(GameState.MAINMENU);
            else
                buttonQuit.Pressed += (s, e) => SwitchState(GameState.ESCAPEMENU);


            return gui;
        }

        public void UpAA(ref Button button)
        {
            aaQuality = aaQuality == 0 ? aaQuality + 1 : (aaQuality * 2) % 32;
            Console.WriteLine(effectQuality);
            Console.WriteLine(aaQuality);
            Console.WriteLine("---------------------------");
            String text = AA2STR();
            button.Text = text;
            this.gui.Add(button);
        }

        public String AA2STR()
        {
            StringBuilder str = new StringBuilder("AA Quality: ");
            
            if (aaQuality == 0)
            {
                str.Append("OFF");
            }
            else
            {
                str.Append(aaQuality);
                str.Append("x");
            }

            return str.ToString();
        }

        public void UpEffect(ref Button button)
        {
            effectQuality = (effectQuality + 1) % 4;
            Console.WriteLine(effectQuality);
            Console.WriteLine(aaQuality);
            Console.WriteLine("---------------------------");
            String text = Effect2Str();
            button.Text = text;
            this.gui.Add(button);
        }

        public String Effect2Str()
        {
            StringBuilder str = new StringBuilder("Effect Quality: ");

            if (effectQuality == 0)
            {
                str.Append("Low");
            }
            else if (effectQuality == 1)
            {
                str.Append("Medium");
            }
            else if (effectQuality == 2)
            {
                str.Append("High");
            }
            else
            {
                str.Append("Ultra");
            }

            return str.ToString();
        }
    }
}
