using System;
using System.Collections.Generic;
using System.Text;

namespace FTA.Fight
{
    public class Caracteristics
    {
        public int Life { get; set; }
        public int Fire { get; set; }
        public int Water { get; set; }
        public int Earth { get; set; }
        public int Air { get; set; }
        public int Light { get; set; }
        public int Dark { get; set; }
        public int Ap { get; set; }
        public int Mp { get; set; }
        public int Range { get; set; }
        public int Invoc { get; set; }
        public int Ret_mp { get; set; }
        public int Ret_ap { get; set; }
        public int Dodge_mp { get; set; }
        public int Dodge_ap { get; set; }
        public int Res_fire { get; set; }
        public int Res_water { get; set; }
        public int Res_earth { get; set; }
        public int Res_air { get; set; }
        public int Res_light { get; set; }
        public int Res_dark { get; set; }

        public Caracteristics()
        {
            this.Life = 1000;
            this.Fire = 0;
            this.Water = 0;
            this.Earth = 0;
            this.Air = 0;
            this.Light = 0;
            this.Dark = 0;
            this.Ap = 0;
            this.Mp = 0;
            this.Range = 0;
            this.Invoc = 0;
            this.Ret_mp = 0;
            this.Ret_ap = 0;
            this.Dodge_mp = 0;
            this.Dodge_ap = 0;
            this.Res_fire = 0;
            this.Res_water = 0;
            this.Res_earth = 0;
            this.Res_air = 0;
            this.Res_light = 0;
            this.Res_dark = 0;
        }

        public Caracteristics(int life, int fire, int water, int earth, int air, int light, int dark, int ap, int mp, int range, int invoc, 
            int ret_mp, int ret_ap, int dodge_mp, int dodge_ap, int res_fire, int res_water, int res_earth, int res_air, int res_light, int res_dark)
        {
            this.Life = life;
            this.Fire = fire;
            this.Water = water;
            this.Earth = earth;
            this.Air = air;
            this.Light = light;
            this.Dark = dark;
            this.Ap = ap;
            this.Mp = mp;
            this.Range = range;
            this.Invoc = invoc;
            this.Ret_mp = ret_mp;
            this.Ret_ap = ret_ap;
            this.Dodge_mp = dodge_mp;
            this.Dodge_ap = dodge_ap;
            this.Res_fire = res_fire;
            this.Res_water = res_water;
            this.Res_earth = res_earth;
            this.Res_air = res_air;
            this.Res_light = res_light;
            this.Res_dark = res_dark;
        }

        public static Caracteristics operator +(Caracteristics c1, Caracteristics c2)
        {
            Caracteristics c3 = new Caracteristics()
            {
                Life = c1.Life + c2.Life,
                Fire = c1.Fire + c2.Fire,
                Water = c1.Water + c2.Water,
                Earth = c1.Earth + c2.Earth,
                Air = c1.Air + c2.Air,
                Light = c1.Light + c2.Light,
                Dark = c1.Dark + c2.Dark,
                Ap = c1.Ap + c2.Ap,
                Mp = c1.Mp + c2.Mp,
                Range = c1.Range + c2.Range,
                Invoc = c1.Invoc + c2.Invoc,
                Ret_mp = c1.Ret_mp + c2.Ret_mp,
                Ret_ap = c1.Ret_ap + c2.Ret_ap,
                Dodge_mp = c1.Dodge_mp + c2.Dodge_mp,
                Dodge_ap = c1.Dodge_ap + c2.Dodge_ap,
                Res_fire = c1.Res_fire + c2.Res_fire,
                Res_water = c1.Res_water + c2.Res_water,
                Res_earth = c1.Res_earth + c2.Res_earth,
                Res_air = c1.Res_air + c2.Res_air,
                Res_light = c1.Res_light + c2.Res_light,
                Res_dark = c1.Res_dark + c2.Res_dark,
            };

            return c3;
        }

    }
}
