namespace WWC
{
    /// <summary>
    /// Weapon template
    /// </summary>
    internal class WeaponTemplate
    {
        private string name;
        private int damage;

        public WeaponTemplate(string name, int damage)
        {
            this.name = name;
            this.damage = damage;
        }

        public string Name => name;
        public int Damage => damage;
    }
}
