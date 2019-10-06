using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    abstract class Unit
    {
        protected int x, y, health, maxHealth, speed, attack, attackRange; //Declared all required variables 
        protected string team;
        protected char symbol;
        protected string name; //Add in a Name field to store unit type
        protected bool isAttacking = false;
        protected bool isDead = false;
        Random r = new Random();

        public Unit(int x, int y, int health, int speed, int attack, int attackRange, string team, char symbol, string name)
        {
            this.x = x;
            this.y = y;
            this.health = health;
            maxHealth = health;
            this.speed = speed;
            this.attack = attack;
            this.attackRange = attackRange;
            this.team = team;
            this.symbol = symbol;
            this.name = name;
        }

        public Unit(string values)
        {
            string[] parameters = values.Split(',');

            x = int.Parse(parameters[1]);
            y = int.Parse(parameters[2]);
            health = int.Parse(parameters[3]);
            maxHealth = int.Parse(parameters[4]);
            speed = int.Parse(parameters[5]);
            attack = int.Parse(parameters[6]);
            attackRange = int.Parse(parameters[7]);
            team = parameters[8];
            symbol = parameters[9][0];
            name = parameters[10];
            isDead = parameters[11] == "True" ? true : false;
        }

        public abstract string Save();
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
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
        public string Team
        {
            get { return team; }
        }
        public char Symbol
        {
            get { return symbol; }
        }
        public bool IsDead
        {
            get { return isDead; }
        }
        public string Name
        {
            get { return name; }
        }

        public virtual void Attack(Unit otherUnit)
        {
            isAttacking = true;
            otherUnit.Health -= attack;
            

            if (otherUnit.Health <= 0 )
            {
                otherUnit.Health = 0;
                otherUnit.Destroy();
            }
        }

        public virtual void Destroy()
        {
            isDead = true;
            isAttacking = false;
            symbol = 'X';
        }
        protected double GetDistance(Unit otherUnit)
        {
            double xDistance = otherUnit.X - X;
            double yDistance = otherUnit.Y - Y;            

            return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }             
        
        public virtual Unit GetClosestUnit(Unit[] units)
        {
            double closestDistance = int.MaxValue;
            Unit closestUnit = null;

            foreach (Unit otherUnit in units)
            {
                if (otherUnit == this || otherUnit.Team == Team || otherUnit.IsDead)
                {
                    continue;
                }

                double distance = GetDistance(otherUnit);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestUnit = otherUnit;
                }
            }
            return closestUnit;
        }        

        public virtual bool IsInRange(Unit otherunit)
        {
            return GetDistance(otherunit) <= attackRange;
        }

        public virtual void MovePosition(Unit closestUnit) //Moves units around map if they arent attacking
        {
            isAttacking = false;
            int xDirection = closestUnit.X - X;
            int yDirection = closestUnit.Y - Y;

            if (Math.Abs(xDirection) > Math.Abs(yDirection))
            {
                x += Math.Sign(xDirection);
            }
            else
            {
                y += Math.Sign(yDirection);
            }
        }

        public virtual void RunAway() //Moves units away from each other 
        {
            isAttacking = false;
            int direction = r.Next(0, 4);
            if (direction == 0)
            {
                x += 1;
            }
            else if (direction == 1)
            {
                x -= 1;
            }
            else if (direction == 2)
            {
                y += 1;
            }
            else
            {
                y -= 1;
            }
        }

        public override string ToString()
        {
            return "````````````````````````````````````````````" + "\n" +
                    name + " (" + symbol + "|" + team[0] + ")" + "\n" +
                    "````````````````````````````````````````````" + "\n" +
                    "Team: " + team + "\n" +
                    "Positon: " + x + "," + y + "\n" +
                    "Health: " + health + "|" + maxHealth + "\n";
        }
    }
}
