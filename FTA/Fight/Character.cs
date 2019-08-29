using System;
using System.Collections.Generic;
using System.Text;
using FTA.Enums;

namespace FTA.Fight
{
    public class Character
    {
        public Caracteristics carateristics;

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
    }
}
