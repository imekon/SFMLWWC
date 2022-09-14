namespace WWC
{
    internal class Monster
    {
        private string name;
        private int strength;
        private int armour;
        private int lowest;

        public Monster(string name, int strength, int armour, int lowest)        {
            this.name = name;
            this.strength = strength;
            this.armour = armour;
            this.lowest = lowest;
        }

        public string Name => name;
        public int Strength => strength;
        public int Armour => armour;
        public int Lowest => lowest;
    }
}
