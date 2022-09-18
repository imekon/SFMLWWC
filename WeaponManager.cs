namespace WWC
{
    internal class WeaponManager
    {
        private Dictionary<string, Weapon> weapons;

        public WeaponManager()
        {
            weapons = new Dictionary<string, Weapon>();
        }

        public List<Weapon> Weapons => weapons.Values.ToList();

        public void AddWeapon(Weapon weapon)
        {
            weapons.Add(weapon.Name, weapon);
        }

        public Weapon? FindWeapon(string name)
        {
            if (weapons.ContainsKey(name))
                return weapons[name];

            return null;
        }
    }
}
