namespace WWC
{
    /// <summary>
    /// Weapon template
    /// </summary>
    internal class Weapon
    {
        private string name;
        private int damage;

        public Weapon(string name, int damage)
        {
            this.name = name;
            this.damage = damage;
        }

        public string Name => name;
        public int Damage => damage;
    }
}
