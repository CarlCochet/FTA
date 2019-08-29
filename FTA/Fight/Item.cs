using System;
using System.Collections.Generic;
using System.Text;

namespace FTA.Fight
{
    public class Item
    {
        public Caracteristics Caracteristics { get; set; }

        public Item()
        {
            this.Caracteristics = new Caracteristics();
        }

        public Item(Caracteristics caracteristics)
        {
            this.Caracteristics = caracteristics;
        }

    }
}
