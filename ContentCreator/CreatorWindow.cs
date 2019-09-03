using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using TGUI;
using SFML.System;

namespace ContentCreator
{
    class CreatorWindow
    {
        private SFML.Graphics.RenderWindow window;
        private Gui gui;
        private String path = "../Data/";
        private States state;
        private int currentId;

        private uint height = 800;
        private uint width = 800;

        private List<Spell> spells;
        private List<Item> items;


        public void Run()
        {
            var mode = new SFML.Window.VideoMode(width, height);
            window = new SFML.Graphics.RenderWindow(mode, "Content Creator");
            window.Closed += (_, __) => window.Close();

            gui = new Gui(window);
            state = States.MODE_SELECT;
            CreateSelectMenu();

            LoadItems();
            LoadSpells();

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();

                gui.Draw();

                window.Display();
            }
        }

        public void CloseWindow()
        {
            window.Close();
        }

        public void SwitchState(States newState)
        {
            state = newState;

        }

        // Creates the main menu GUI
        public void CreateSelectMenu()
        {
            if (gui != null)
                gui.RemoveAllWidgets();

            var buttonStart = new Button("Edit Items")
            {
                Position = new Vector2f(width / 3, height * 1 / 5),
                Size = new Vector2f(width  / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonStart);

            buttonStart.Pressed += (s, e) => SwitchState(States.ITEM_EDIT);


            var buttonTeam = new Button("Edit Spells")
            {
                Position = new Vector2f(width / 3, height * 2 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonTeam);

            buttonTeam.Pressed += (s, e) => SwitchState(States.SPELL_EDIT);


            var buttonQuit = new Button("Exit")
            {
                Position = new Vector2f(width / 3, height * 4 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonQuit);

            buttonQuit.Pressed += (s, e) => CloseWindow();
        }

        public void LoadItems()
        {
            string[][] data = ReadCsv(path + "item.csv");
            
            for (int i = 0; i < data.Length; i++)
            {
                for (int k = 0; k < data[0].Length; k++)
                {

                }
            }
        }

        public void SaveItems()
        {



        }

        public void LoadSpells()
        {
            string[][] data = ReadCsv(path + "spell.csv");

            for (int i = 0; i < data.Length; i++)
            {
                for (int k = 0; k < data[0].Length; k++)
                {

                }
            }
        }

        public void SaveSpells()
        {


            
        }

        public static string[][] ReadCsv(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd().Split('\n').Select(line => line.Split(',')).ToArray();
        }

        static void LineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
    }
}
