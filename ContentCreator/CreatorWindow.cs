﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using TGUI;
using SFML.System;

using CsvHelper;

namespace ContentCreator
{
    class CreatorWindow
    {
        private SFML.Graphics.RenderWindow window;
        private Gui gui;
        private String path = "../../../../Data/";
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

            spells = new List<Spell>();
            items = new List<Item>();

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

            if (state == States.MODE_SELECT)
            {
                CreateSelectMenu();
            }
            if(state == States.ITEM_EDIT)
            {

            }
            if(state == States.SPELL_EDIT)
            {

            }
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

        public void CreateSpellMenu()
        {
            if (gui != null)
                gui.RemoveAllWidgets();

            var buttonBack = new Button("Edit Items")
            {
                Position = new Vector2f(width / 3, height * 1 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonBack);

            buttonBack.Pressed += (s, e) => SwitchState(States.MODE_SELECT);
        }

        public void CreateItemMenu()
        {
            if (gui != null)
                gui.RemoveAllWidgets();

            var buttonNew = new Button("New Item")
            {
                Position = new Vector2f(width / 3, height * 1 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonNew);

            buttonNew.Pressed += (s, e) => SwitchState(States.MODE_SELECT);

            var buttonBack = new Button("Edit Items")
            {
                Position = new Vector2f(width / 3, height * 1 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonBack);

            buttonBack.Pressed += (s, e) => SwitchState(States.MODE_SELECT);
        }

        public void AddItem()
        {
            items.Add(new Item() { Id = items.Count() });
        }

        public void DeleteItem()
        {

        }

        public void AddSpell()
        {
            spells.Add(new Spell() { Id = spells.Count() });
        }

        public void DeleteSpell()
        {

        }

        public void LoadItems()
        {
            using (var reader = new StreamReader(path + "items.csv"))
            using (var csv = new CsvReader(reader))
            {
                while (csv.Read())
                {
                    var records = csv.GetRecord<Item>();

                    items.Add(records);
                }
            }
        }

        public void SaveItems()
        {

        }

        public void LoadSpells()
        {
            string[][] data = ReadCsv(path + "spells.csv");

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
