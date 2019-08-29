using System;
using System.Collections.Generic;
using System.Text;

namespace FTA
{
    public class Character
    {
        public int life { get; set; }
        public int fire { get; set; }
        public int water { get; set; }
        public int earth { get; set; }
        public int air { get; set; }
        public int electricity { get; set; }
        public int ap { get; set; }
        public int mp { get; set; }
        public int range { get; set; }
        public int invoc { get; set; }
        public int ret_mp { get; set; }
        public int ret_ap { get; set; }
        public int dodge_mp { get; set; }
        public int dodge_ap { get; set; }
        public int res_fire { get; set; }
        public int res_water { get; set; }
        public int res_earth { get; set; }
        public int res_air { get; set; }


        public Character()
        {
            this.life = 100;
            this.fire = 0;
            this.water = 0;
            this.earth = 0;
            this.air = 0;
            this.electricity = 0;
            
        }

        public Character(int life, int fire, int water, int earth, int air, int electricity)
        {
            this.life = life;
            this.fire = fire;
            this.water = water;
            this.earth = earth;
            this.air = air;
            this.electricity = electricity;
        }
    }
}
