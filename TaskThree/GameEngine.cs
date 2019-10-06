using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;

namespace TaskThree
{
    class GameEngine
    {
        Random r = new Random();
        const string UNITS_FILENAME = "units.txt";
        const string BUIDLINGS_FILENAME = "buildings.txt";
        const string ROUND_FILENAME = "rounds.txt";

        Map map;

        int height, width;
        int numUnits, numBuildings;

        bool isGameOver = false;
        string winningTeam;
        int round = 0;

        public GameEngine(int height, int width, int numUnits, int numBuildings)
        {
            this.numUnits = numUnits;
            this.numBuildings = numBuildings;
            this.height = height;
            this.width = width;
            
            map = new Map(this.height,this.width,10,4);
        }
        public bool IsGameOver
        {
            get { return isGameOver; }
        }
        public string WinningTeam
        {
            get { return winningTeam; }
        }
        public int Round
        {
            get { return round; }
        }
        private void StayInBounds(Unit unit, int height, int width) //Used so units dont go off the map
        {
            if (unit.X < 0)
            {
                unit.X = 0;
            }
            else if (unit.X >= width)
            {
                unit.X = width- 1;
            }
            if (unit.Y < 0)
            {
                unit.Y = 0;
            }
            else if (unit.Y >= height)
            {
                unit.Y = height - 1;
            }
        }

        void UpdateUnits()
        {
            foreach (Building building in map.Buildings)
            {
                if (building.IsDestroyed)
                {
                    continue;
                }

                Building closestBuilding = building.GetClosestBuilding(map.Buildings);

                if (closestBuilding == null)
                {
                    isGameOver = true;
                    winningTeam = building.Team;
                    map.UpdateMap();
                    return;
                }

                double healthPercentageB = building.Health / building.MaxHealth;
                if (healthPercentageB <= 0.25)
                {
                    continue;
                }
                else if (building.IsBuildingInRange(closestBuilding))
                {

                }

            }

            foreach (Unit unit in map.Units)
            {
                if (unit.IsDead)
                {
                    continue;
                }

                Unit closestUnit = unit.GetClosestUnit(map.Units);               

                if (closestUnit == null)
                {
                    isGameOver = true;
                    winningTeam = unit.Team;
                    map.UpdateMap();
                    return;
                }

                double healthPercentage = unit.Health / unit.MaxHealth; //Determines units health
                if (healthPercentage <= 0.25)
                {
                    unit.RunAway(); //If a units health is 25% then they will run away
                }
                else if (unit.IsInRange(closestUnit))
                {
                    unit.Attack(closestUnit); //If units are in range of other then they will attack
                    
                }
                else
                {
                    unit.MovePosition(closestUnit); //If they have more than 25% health and there isnt a unit in range then they will move
                }

                StayInBounds(unit, map.height, map.width); //Makes sure that units stay on the map
            }
        }

        void UpdateBuildings()
        {
            foreach (Building building in map.Buildings)
            {
                if (building is FactoryBuilding)
                {
                    FactoryBuilding factoryBuilding = (FactoryBuilding)building;
                    if (round % factoryBuilding.ProductionSpeed == 0)
                    {
                        Unit newUnit = factoryBuilding.SpawnUnit();
                        map.AddInUnits(newUnit);
                    }
                }
                else if (building is ResourceBuilding)
                {
                    ResourceBuilding resourceBuilding = (ResourceBuilding)building;
                    resourceBuilding.GenerateResources();
                }
            }
        }
        public void GameLoop()
        {
            UpdateUnits();
            UpdateBuildings();
            map.UpdateMap();
            round++;
        }

        public int NumUnits
        {
            get { return map.Units.Length; }
        }
        public int NumBuildings
        {
            get { return map.Buildings.Length; }
        }
        public string MapDisplay
        {
            get { return map.MapDisplay(); }
        }

        public string BuildingInfo() //Updates the second textbox dedicated for building information 
        {
            string buildingInfo = "";
            foreach (Building building in map.Buildings)
            {
                buildingInfo += building + "\n";
            }
            return buildingInfo;
        }
        public string UnitInfo() //Updates textbox with unit information
        {
            string unitInfo = "";
            foreach (Unit unit in map.Units)
            {
                unitInfo += unit + "\n";
            }
            return unitInfo;
        }

        public void Reset() //Resets the game
        {
            map.Reset();
            isGameOver = false;
            round = 0;
        }
        private void Save(string filename, object[] objects)
        {
            FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            foreach (object obj in objects)
            {
                if (obj is Unit)
                {
                    Unit unit = (Unit)obj;
                    writer.WriteLine(unit.Save());
                }
                else if (obj is Building)
                {
                    Building building = (Building)obj;
                    writer.WriteLine(building.Save());
                }
            }
            writer.Close();
            stream.Close();
        }

        private void SaveRound()
        {
            FileStream stream = new FileStream(ROUND_FILENAME, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine(round);
            writer.Close();
            stream.Close();
        }
        public void SaveGame()
        {           
            Save(UNITS_FILENAME, map.Units);
            Save(BUIDLINGS_FILENAME, map.Buildings);
            SaveRound();
        }

        private void Load(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            string recordIn;
            recordIn = reader.ReadLine();
            while (recordIn !=null )
            {
                int length = recordIn.IndexOf(",");
                string firstField = recordIn.Substring(0, length);

                switch (firstField)
                {
                    case "Melee": map.AddInUnits(new MeleeUnit(recordIn)); break;
                    case "Ranged": map.AddInUnits(new RangedUnit(recordIn)); break;
                    case "Factory": map.AddInBuilding(new FactoryBuilding(recordIn));break;
                    case "Resource": map.AddInBuilding(new ResourceBuilding(recordIn)); break;
                }

                recordIn = reader.ReadLine();
            }

            reader.Close();
            stream.Close();
        }

        private void LoadRound()
        {
            FileStream stream = new FileStream(ROUND_FILENAME, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            round = int.Parse(reader.ReadLine());

            reader.Close();
            stream.Close();
        }

        public void LoadGame()
        {
            map.Clear();
            Load(UNITS_FILENAME);
            Load(BUIDLINGS_FILENAME);
            LoadRound();
            map.UpdateMap();
        }

    }
}
