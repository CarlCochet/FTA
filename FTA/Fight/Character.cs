using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using FTA.Enums;

namespace FTA.Fight
{
    public class Character
    {
        public Caracteristics Carateristics { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public List<CharacterState> States { get; set; }
        public List<Spell> Spells { get; set; }
        public List<Item> Items { get; set; }
        public int Cost { get; set; }

        public bool Summoned { get; set; }


        public Character()
        {
            this.Carateristics = new Caracteristics();
            this.X = 0;
            this.Y = 0;
            this.Cost = 100000;
            this.Summoned = false;
            this.States = new List<CharacterState>();
            this.Spells = new List<Spell>();
            this.Items = new List<Item>();
        }

        public Character(Caracteristics caracteristics) 
        {
            this.Carateristics = caracteristics;
        }

        public void AddItem(Item item)
        {
            if (!Items.Exists(x => x.Stuff == item.Stuff))
            {
                this.Items.Add(item);
                this.Carateristics += item.Caracteristics;
                this.Cost += item.Cost;
            }
        }

        public void AddSpell(Spell spell)
        {
            if (!Spells.Exists(x => x == spell))
            {
                this.Spells.Add(spell);
                this.Cost += spell.Cost;
            }
        }

        public void Item2Carac()
        {
            foreach (Item item in Items)
            {
                this.Carateristics += item.Caracteristics;
            }
        }

        
    }
}
