using System;
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
        private uint width = 1200;

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
                CreateItemMenu();
            }
            if(state == States.SPELL_EDIT)
            {
                CreateSpellMenu();
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

        // Creating the spell menu
        public void CreateSpellMenu()
        {
            if (gui != null)
                gui.RemoveAllWidgets();

            var buttonNew = new Button("New Spell")
            {
                Position = new Vector2f(width / 3, height * 1 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonNew);

            buttonNew.Pressed += (s, e) => SwitchState(States.MODE_SELECT);

            var buttonBack = new Button("Back")
            {
                Position = new Vector2f(width * 1 / 20, height * 13 / 16),
                Size = new Vector2f(width * 1 / 8, height * 1 / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonBack);

            buttonBack.Pressed += (s, e) => SwitchState(States.MODE_SELECT);
        }

        // Creating the item menu
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

            buttonNew.Pressed += (s, e) => AddItem();

            var buttonDelete = new Button("Delete Item")
            {
                Position = new Vector2f(width / 3, height * 1 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonDelete);

            buttonDelete.Pressed += (s, e) => DeleteItem(currentId);

            var buttonAdd = new Button("Add")
            {
                Position = new Vector2f(width / 3, height * 1 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonNew);

            buttonNew.Pressed += (s, e) => AddItem();

            var buttonRemove = new Button("Remove")
            {
                Position = new Vector2f(width / 3, height * 1 / 5),
                Size = new Vector2f(width / 3, height / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonDelete);

            buttonDelete.Pressed += (s, e) => DeleteItem(currentId);

            var buttonBack = new Button("Back")
            {
                Position = new Vector2f(width * 1 / 20, height * 13 / 16),
                Size = new Vector2f(width * 1 / 8, height * 1 / 10),
                TextSize = (uint)(0.6 * height / 10)
            };
            gui.Add(buttonBack);

            buttonBack.Pressed += (s, e) => SwitchState(States.MODE_SELECT);
        }

        // Adding an item to the array
        public void AddItem()
        {
            items.Add(new Item() { Id = items.Count() });
            CreateItemMenu();
        }

        // Remove an item from the array by its Id. Also updates all Ids to keep consistency
        public void DeleteItem(int id)
        {
            items.RemoveAt(id);

            for (int i = 0; i < items.Count(); i++)
            {
                items[i].Id = i;
            }
            CreateItemMenu();
        }

        // Adding a spell to the array
        public void AddSpell()
        {
            spells.Add(new Spell() { Id = spells.Count() });
            CreateSpellMenu();
        }

        // Remove a spell from the array by its Id. Also updates all Ids to keep consistency
        public void DeleteSpell(int id)
        {
            spells.RemoveAt(id);

            for (int i = 0; i < spells.Count(); i++)
            {
                spells[i].Id = i;
            }
            CreateSpellMenu();
        }

        // Loading items from datafile to an array
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

        // Saving items from array to a datafile
        public void SaveItems()
        {
            /*System.IO.StreamReader file = new System.IO.StreamReader(@"c:\test.txt");
            List<string> lines = new List<string>();
            lines.Add(file.ReadLine());

            for (int i = 0; i < items.Count(); i++)
            {

            }*/

            using (var writer = new StreamWriter(path + "items.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(items);
            }
        }

        // Loading spells from datafile to an array
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

        // Saving spells from array to a datafile
        public void SaveSpells()
        {


            
        }

        // Function to read CSV into a 2D string array
        public static string[][] ReadCsv(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd().Split('\n').Select(line => line.Split(',')).ToArray();
        }

        // Changing a single line in the datafile 
        static void LineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
    }
}
