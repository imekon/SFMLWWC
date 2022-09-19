namespace WWC
{
    /// <summary>
    /// Weapon template
    /// </summary>
    internal class WeaponTemplate
    {
        private string name;
        private int damage;
        private bool magical;
        private bool weild;

        public WeaponTemplate(string name, int damage, bool magical, bool weild)
        {
            this.name = name;
            this.damage = damage;
            this.magical = magical;
            this.weild = weild;
        }

        public string Name => name;
        public int Damage => damage;
        public bool Magical => magical;
        public bool Weild => weild;
    }
}
