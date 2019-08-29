using System;
using System.Collections.Generic;
using System.Text;
using FTA.Enums;

namespace FTA.Fight
{
    public class Character
    {
        public Characteristics charateristics;

        public int X { get; set; }
        public int Y { get; set; }
        public List<CharacterState> states { get; set; }
        public List<Spell> spells { get; set; }
        public List<Item> items { get; set; }


        public Character()
        {
            this.Life = 100;
            this.Fire = 0;
            this.Water = 0;
            this.Earth = 0;
            this.Air = 0;
            this.Electricity = 0;
            
        }

        public Character(int life, int fire, int water, int earth, int air, int electricity)
        {
            this.Life = life;
            this.Fire = fire;
            this.Water = water;
            this.Earth = earth;
            this.Air = air;
            this.Electricity = electricity;
        }
    }
}
