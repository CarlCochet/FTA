using System;
using System.Collections.Generic;
using System.Text;

namespace FTA.Fight
{
    public class Team
    {
        public List<Character> characters;
        public int Money;

        public Team()
        {
            characters = new List<Character>();
            this.Money = 1000000;
        }
    }
}
