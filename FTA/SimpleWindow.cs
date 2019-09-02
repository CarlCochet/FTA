using System;
using System.Collections.Generic;
using System.Text;

using SFML.System;
using TGUI;

using FTA.Map;
using FTA.Fight;

namespace FTA
{
    class SimpleWindow
    {
        private Vector2i start = new Vector2i(0, 0);    // Used to store a starting point (pathfinding / LoS)
        private Vector2i target = new Vector2i(0, 0);   // Used to store a target point (pathfinding)
        private ArenaMap arenaMap = new ArenaMap();     // Arena object that stores the arena data and provides utils
        private SFML.Graphics.RenderWindow window;      // The main window
        private GameState state;                        // Game state to know what to display / process
        private Gui gui;                                // Stores all the UI elements
        
        private int aaQuality = 0;                      // Antialiasing parameter (for testing)
        private int effectQuality = 0;                  // Effect quality (for testing)

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
            gui = new Gui(window);
            CreateMainMenu();

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

                // If we are in a in-game menu, then the map should also be displayed in the background
                if (state == GameState.ESCAPEMENU || state == GameState.INGAMEOPTION)
                {
                    arenaMap.Render(window);
                    gui.Draw();
                }

                // Displays the general menus without the in-game context
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

            // Opens menus or get back to previous menu
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                if(this.state == GameState.OPTION || this.state == GameState.MANAGETEAM)
                {
                    SwitchState(GameState.MAINMENU);
                    CreateMainMenu();
                }
                if (this.state == GameState.INGAMEOPTION)
                {
                    SwitchState(GameState.ESCAPEMENU);
                    CreateEscapeMenu();
                }
                if (this.state == GameState.INGAME || this.state == GameState.PLACEMENT)
                {
                    SwitchState(GameState.ESCAPEMENU);
                    CreateEscapeMenu();
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

        // Changing the state of the game and updating the GUI
        public void SwitchState(GameState newState)
        {
            state = newState;

            // Update the GUI when switching states
            if (state == GameState.MAINMENU)
            {
                CreateMainMenu();
            }
            if (state == GameState.ESCAPEMENU)
            {
                CreateEscapeMenu();
            }
            if (state == GameState.OPTION || state == GameState.INGAMEOPTION)
            {
                CreateOptionMenu();
            }
            if (state == GameState.MANAGETEAM)
            {
                CreateTeamMenu();
            }

            Console.WriteLine(newState.ToString());
        }

        // Creates the main menu GUI
        public void CreateMainMenu()
        {
            if (gui != null)
                gui.RemoveAllWidgets();

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
        }

        // Creates the in-game menu GUI
        public void CreateEscapeMenu()
        {
            if (gui != null)
                gui.RemoveAllWidgets();

            var buttonStart = new Button("Resume")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 1 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonStart);

            buttonStart.Pressed += (s, e) => SwitchState(GameState.INGAME);

            var buttonOption = new Button("Options")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 2 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonOption);

            buttonOption.Pressed += (s, e) => SwitchState(GameState.INGAMEOPTION);

            var buttonQuit = new Button("Exit to main menu")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 3 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonQuit);

            buttonQuit.Pressed += (s, e) => SwitchState(GameState.MAINMENU);
        }

        // Creates the Option menu GUI
        public void CreateOptionMenu()
        {
            if (gui != null)
                gui.RemoveAllWidgets();

            Console.WriteLine("Creating option menu...");
            var buttonAA = new Button("")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 1 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10),
                Text = AA2STR()
            };
            gui.Add(buttonAA);

            buttonAA.Pressed += (s, e) => UpAA(ref buttonAA);

            var buttonEffect = new Button("")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 2 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10),
                Text = Effect2Str()
            };
            gui.Add(buttonEffect);

            buttonEffect.Pressed += (s, e) => UpEffect(ref buttonEffect);

            var buttonBack = new Button("Back")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 3 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonBack);

            Console.WriteLine(state.ToString());

            if (state == GameState.OPTION)
            {
                Console.WriteLine("BACK TO MAIN MENU");
                buttonBack.Pressed += (s, e) => SwitchState(GameState.MAINMENU);
            }
            else if (state == GameState.INGAMEOPTION)
            {
                Console.WriteLine("BACK TO IN-GAME MENU");
                buttonBack.Pressed += (s, e) => SwitchState(GameState.ESCAPEMENU);
            }

        }

        // Creates the Team management menu GUI
        public void CreateTeamMenu()
        {
            if (gui != null)
                gui.RemoveAllWidgets();

            Console.WriteLine("Creating option menu...");
            var buttonAA = new Button("")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 1 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10),
                Text = AA2STR()
            };
            gui.Add(buttonAA);

            buttonAA.Pressed += (s, e) => UpAA(ref buttonAA);

            var buttonEffect = new Button("")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 2 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10),
                Text = Effect2Str()
            };
            gui.Add(buttonEffect);

            buttonEffect.Pressed += (s, e) => UpEffect(ref buttonEffect);

            var buttonBack = new Button("Back")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 4, Utils.WINDOW_HEIGHT * 3 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 2, Utils.WINDOW_HEIGHT / 10),
                TextSize = (uint)(0.6 * Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonBack);

            Console.WriteLine(state.ToString());

            if (state == GameState.OPTION)
            {
                Console.WriteLine("BACK TO MAIN MENU");
                buttonBack.Pressed += (s, e) => SwitchState(GameState.MAINMENU);
            }
            else if (state == GameState.INGAMEOPTION)
            {
                Console.WriteLine("BACK TO IN-GAME MENU");
                buttonBack.Pressed += (s, e) => SwitchState(GameState.ESCAPEMENU);
            }

        }

        // Counts the AA quality
        public void UpAA(ref Button button)
        {
            aaQuality = aaQuality == 0 ? aaQuality + 1 : (aaQuality * 2) % 32; // 0, 1, 2, 4, 8, 16, 0... 
            String text = AA2STR();
            button.Text = text;
            this.gui.Add(button);
        }

        // Transforms the int AA value to String
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

        // Counts the effect quality
        public void UpEffect(ref Button button)
        {
            effectQuality = (effectQuality + 1) % 4; // 0, 1, 2, 3, 0...
            String text = Effect2Str();
            button.Text = text;
            this.gui.Add(button);
        }

        // Transforms the int effect value to String
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
