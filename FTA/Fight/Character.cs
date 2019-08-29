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
        public Caracteristics Carateristics;

        public int X { get; set; }
        public int Y { get; set; }

        public List<CharacterState> States { get; set; }
        public List<Spell> Spells { get; set; }
        public List<Item> Items { get; set; }

        public bool Summoned;


        public Character()
        {
            
        }

        public Character(int life, int fire, int water, int earth, int air, int electricity)
        {

        }

        public void AddItem(Item item)
        {
            if (!Items.Exists(x => x.Stuff == item.Stuff))
            {
                Items.Add(item);
                this.Carateristics += item.Caracteristics;
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
