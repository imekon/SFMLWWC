namespace WWC
{
    internal class WeaponManager
    {
        private Dictionary<string, WeaponTemplate> weapons;

        public WeaponManager()
        {
            weapons = new Dictionary<string, WeaponTemplate>();
        }

        public List<WeaponTemplate> Weapons => weapons.Values.ToList();

        public void AddWeapon(WeaponTemplate weapon)
        {
            weapons.Add(weapon.Name, weapon);
        }

        public WeaponTemplate? FindWeapon(string name)
        {
            if (weapons.ContainsKey(name))
                return weapons[name];

            return null;
        }
    }
}
