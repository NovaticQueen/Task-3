using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    public enum FactoryType
    {
        MELEE,
        RANGED
    }

    class FactoryBuilding : Building
    {
        private FactoryType type;            
        int productionSpeed, spawnY;
        Random r = new Random();

        Map map;
        int height, width;
        int numUnits, numBuidlings;

        public FactoryBuilding(int x, int y, string team) : base (x, y, 100, team, 'ƒ')
        {
            map = new Map(height,width,numUnits,numBuidlings);

            if (y >= map.height - 1)
            {
                spawnY = y - 1;
            }
            else
            {
                spawnY = y + 1;
            }

            type = (FactoryType) r.Next(0, 2);
            productionSpeed = r.Next(3, 7);
        }

        public FactoryBuilding(string values)
        {
            string[] parameters = values.Split(',');

            x = int.Parse(parameters[1]);
            y = int.Parse(parameters[2]);
            health = int.Parse(parameters[3]);
            maxHealth = int.Parse(parameters[4]);
            type = (FactoryType)int.Parse(parameters[5]);
            productionSpeed = int.Parse(parameters[6]);
            spawnY= int.Parse(parameters[7]);            
            symbol = parameters[8][0];
            team = parameters[9];
            isDestroyed = parameters[10] == "True" ? true : false;
        }
        public int ProductionSpeed
        {
            get { return productionSpeed; }
        }

        public override void Destroy()
        {
            isDestroyed = true;
            symbol = '_';
        }

        public override string Save()
        {
            return string.Format
                 (
                  $"Factory,{x},{y},{health},{maxHealth},{(int)type}," +
                  $"{productionSpeed},{spawnY}," +
                  $"{team},{symbol},{isDestroyed}"
                 );
        }

        public string GetFactoryType()
        {
            return new string[] { "Melee", "Ranged" }[(int)type];
        }

        public Unit SpawnUnit()
        {
            Unit unit;

            if (type == FactoryType.MELEE)
            {
                unit = new MeleeUnit(x, spawnY, team);
            }
            else
            {
                unit = new RangedUnit(x, spawnY, team);
            }
            return unit;
        }
        public override string ToString()
        {
            return "````````````````````````````````````````````" + "\n" +
                   "Factory Building: (" + symbol + "|" + team[0] + ")" + "\n" +
                   "````````````````````````````````````````````" + "\n" +
                   "Type: " + GetFactoryType() + "\n" +
                   base.ToString() + "\n";
        }
    }
}
