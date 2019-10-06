using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    class ResourceBuilding : Building
    {
        private ResourceType type;
        private int generatedPerRound;
        private int generated;
        private int resourcePool;
        Random r = new Random();

        public ResourceBuilding(int x, int y, string team) : base (x, y, 100, team, '■')
        {
            generatedPerRound = r.Next(1, 6);
            generated = 0;
            resourcePool = r.Next(0, 4);
            type = (ResourceType) r.Next(0, 4);
        }

        public ResourceBuilding (string values)
        {
            string[] parameters = values.Split(',');

            x = int.Parse(parameters[1]);
            y = int.Parse(parameters[2]);
            health = int.Parse(parameters[3]);
            maxHealth = int.Parse(parameters[4]);
            type = (ResourceType)int.Parse(parameters[5]);
            generatedPerRound = int.Parse(parameters[6]);
            generated = int.Parse(parameters[7]);
            resourcePool = int.Parse(parameters[8]);
            symbol = parameters[9][0];
            team = parameters[10];
            isDestroyed = parameters[11] == "True" ? true : false;
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
                  $"Resource,{x},{y},{health},{maxHealth},{(int)type}," +
                  $"{generatedPerRound},{generated},{resourcePool}," +
                  $"{team},{symbol},{isDestroyed}"
                 );
        }

        public void GenerateResources()
        {
            if (isDestroyed)
            {
                return;
            }

            if (resourcePool > 0) //can't generate if pool = 0
            {
                int resourcesGenerated = Math.Min(resourcePool, generatedPerRound);
                generated += resourcesGenerated;
                resourcePool -= resourcesGenerated;
            }
        }

        public string GetResourceType() //Converts enum back into a string
        {
            return new string[] { "Wood", "Food", "Rock", "Gems", "Gold" }[(int)type];
        }

        public override string ToString()
        {
            return "````````````````````````````````````````````" + "\n" +
                   "Resource Building: (" + symbol + "|" + team[0] + ")" + "\n" +
                   "````````````````````````````````````````````" + "\n" +
                   GetResourceType() + ": " + generated + "\n" +
                   "Pool: " + resourcePool + "\n" +
                   base.ToString() + "\n";
        }

    }
}
