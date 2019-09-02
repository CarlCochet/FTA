using System;
using System.Collections.Generic;
using System.Text;

namespace FTA.Fight
{
    public class Team
    {
        public List<Character> characters { get; set; }
        public int Money { get; set; }

        public Team()
        {
            characters = new List<Character>();
            this.Money = 1000000;
        }

        public void AddCharacter()
        {
            this.characters.Add(new Character());
            this.Money -= this.characters[characters.Count - 1].Cost;
        }

        public void AddItem2Character(int index, Item item)
        {
            if (item.Cost <= this.Money)
            {
                if (this.characters[index].AddItem(item))
                {
                    this.Money -= item.Cost;
                }
            }
        }

        public void AddSpell2Character(int index, Spell spell)
        {
            if (spell.Cost <= this.Money)
            {
                if (this.characters[index].AddSpell(spell))
                {
                    this.Money -= spell.Cost;
                }
            }
        }
    }
}
