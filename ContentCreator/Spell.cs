using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCreator
{
    class Spell
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int RangeMax { get; set; }
        public int RangeMin { get; set; }
        public int IsRangeMod { get; set; }
        public Zones ZoneType { get; set; }
        public int ZoneStart { get; set; }
        public int ZoneEnd { get; set; }

        public List<Effects> EffectsList { get; set; }
        public List<int> ValueList { get; set; }
        public List<Triggers> TriggersList { get; set; }

        public Spell()
        {

        }
        
    }
}
