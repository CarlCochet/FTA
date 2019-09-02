using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCreator
{
    class Item
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Stuff Stuff { get; set; }

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

        public Item()
        {

        }
    }
}
