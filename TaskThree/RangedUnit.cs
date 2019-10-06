using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    class RangedUnit : Unit
    {
        public RangedUnit(int x, int y, string team) : base(x, y, 100, 3, 10, 1, team, 'R', "Archer")
        {

        }

        public RangedUnit(string values) : base(values)
        {

        }

        public override string Save()
        {
            return string.Format
                (
                 $"Ranged,{x},{y},{health},{maxHealth},{speed},{attack},{attackRange}," +
                 $"{team},{symbol},{isDead},{name}"
                );
        }


    }
}
