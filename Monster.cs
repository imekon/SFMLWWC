namespace WWC
{
    internal class Monster
    {
        private string name;
        private int strength;
        private int dexterity;
        private int iq;
        private int armour;
        private int lowest;
        private int highest;

        public Monster(string name, int strength, int dexterity, int iq, int armour, int lowest, int highest)
        {
            this.name = name;
            this.strength = strength;
            this.dexterity = dexterity;
            this.iq = iq;
            this.armour = armour;
            this.lowest = lowest;
            this.highest = highest;
        }

        public string Name => name;
        public int Strength => strength;
        public int Dexterity => dexterity;
        public int IQ => iq;
        public int Armour => armour;
        public int Lowest => lowest;
        public int Highest => highest;
    }
}
