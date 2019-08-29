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
        public List<CharacterState> states { get; set; }
        public List<Spell> spells { get; set; }
        public List<Item> items { get; set; }


        public Character()
        {
            
        }

        public Character(int life, int fire, int water, int earth, int air, int electricity)
        {

        }
    }
}
