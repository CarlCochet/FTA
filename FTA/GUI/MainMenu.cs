using System;
using System.Collections.Generic;
using System.Text;

using SFML.System;
using SFML.Graphics;
using SFML.Window;

using TGUI;

namespace FTA.GUI
{
    class MainMenu
    {
        private Gui gui;

        public MainMenu(in SimpleWindow simpleWindow, ref SFML.Graphics.RenderWindow window)
        {
            gui = new Gui(window);
        }

        public Gui CreateMainMenu(Gui gui)
        {
            var picture = new Picture("bg_menu.png");
            picture.Size = new Vector2f(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y);
            gui.Add(picture);

            var buttonStart = new Button("Launch fight") {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonStart);

            var buttonTeam = new Button("Manage Team")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 2/ 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonTeam);

            var buttonOption = new Button("Options")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 3 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonOption);

            var buttonQuit = new Button("Exit")
            {
                Position = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT * 4 / 5),
                Size = new Vector2f(Utils.WINDOW_WIDTH / 3, Utils.WINDOW_HEIGHT / 10)
            };
            gui.Add(buttonQuit);


            return gui;
        }
    }
}
