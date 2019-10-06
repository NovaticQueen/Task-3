using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    class MeleeUnit : Unit
    {
        public MeleeUnit(int x, int y, string team) :base (x, y, 200, 1, 15, 1, team , 'M', "Knight")
        {

        }

        public MeleeUnit(string values) : base(values)
        {

        }

        public override string Save()
        {
            return string.Format
                (
                 $"Melee,{x},{y},{health},{maxHealth},{speed},{attack},{attackRange}," +
                 $"{team},{symbol},{isDead},{name}"
                );
        }
        
    }
}
