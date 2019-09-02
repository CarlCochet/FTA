using System;
using System.Collections.Generic;
using System.Text;

using FTA.Enums;

namespace FTA.Fight
{
    public class Item
    {
        public Caracteristics Caracteristics { get; set; }
        public Stuff Stuff;
        public String Name;
        public int Cost;

        public Item()
        {
            this.Caracteristics = new Caracteristics();
        }

        public Item(Caracteristics caracteristics, Stuff stuff, String name)
        {
            this.Caracteristics = caracteristics;
            this.Stuff = stuff;
            this.Name = name;
        }

    }
}
