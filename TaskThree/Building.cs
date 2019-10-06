using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    public enum ResourceType
    {
        WOOD,
        ROCK,
        GEMS,
        GOLD,
        FOOD
    }
    abstract class Building
    {
        protected int x;
        protected int y;
        protected int health;
        protected int maxHealth;
        protected string team;
        protected char symbol;
        protected bool isDestroyed = false;

        public Building(int x, int y, int health, string team, char symbol)
        {
            this.x = x;
            this.y = y;
            this.health = health;
            this.maxHealth = health;
            this.team = team;
            this.symbol = symbol;
        }
        public Building() { }

        public int X
        {
            get { return x; }
        }
        public int Y
        {
            get { return y; }
        }
        public string Team
        {
            get { return team; }
        }
        public char Symbol
        {
            get { return symbol; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public bool IsDestroyed
        {
            get { return isDestroyed; }
        }

        public abstract void Destroy();
        public abstract string Save();
        public override string ToString()
        {
            return
                "Team: " + team + "\n" +
                "Position: " + x + "," + y + "\n" +
                "Health: " + health + "|" + maxHealth + "\n";
        }
        protected double GetDistanceBuilding(Building otherbuilding)
        {
            double buildingXDistance = otherbuilding.X - X;
            double buildingYDistance = otherbuilding.Y - Y;

            return Math.Sqrt(buildingXDistance * buildingXDistance + buildingYDistance * buildingYDistance);
        }
        public virtual bool IsBuildingInRange(Building otherbuilding)
        {
            return GetDistanceBuilding(otherbuilding) <= health; //////
        }

        public virtual Building GetClosestBuilding(Building[] buildings)
        {
            double bClosestDistance = int.MaxValue;
            Building closestBuilding = null;

            foreach (Building otherbuilding in buildings)
            {
                if (otherbuilding.Team == Team || otherbuilding.IsDestroyed)
                {
                    continue;
                }

                double bDistance = GetDistanceBuilding(otherbuilding);
                if (bDistance < bClosestDistance)
                {
                    bClosestDistance = bDistance;
                    closestBuilding = otherbuilding;
                }
            }

            return closestBuilding;
        }

    }
}
