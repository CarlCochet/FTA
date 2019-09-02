using System;
using System.Collections.Generic;
using System.Text;
using FTA.Enums;

namespace FTA.Fight
{
    public class Spell
    {
        public int Id { get; set; }
        public List<Effects> EffectsList { get; set; }
        public List<int> ValueList { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int Cost { get; set; }

        public Spell()
        {
            this.EffectsList = new List<Effects>();
            this.ValueList = new List<int>();
            this.Name = "";
            this.Description = "";
        }

        public Spell(List<Effects> effects, List<int> values, String name, String description)
        {
            this.EffectsList = effects;
            this.ValueList = values;
            this.Name = name;
            this.Description = description;
        }
    }
}
