namespace WWC
{
    internal class MonsterManager
    {
        private Dictionary<string, Monster> monsters;

        public MonsterManager()
        {
            monsters = new Dictionary<string, Monster>();
        }

        public List<Monster> Monsters => monsters.Values.ToList();

        public void Add(Monster monster)
        {
            monsters.Add(monster.Name, monster);
        }

        public Monster? FindMonster(string name)
        {
            if (monsters.ContainsKey(name))
                return monsters[name];

            return null;
        }

        public ActorType GetActorType(string name)
        {
            switch(name)
            {
                case "vendor":
                    return ActorType.Vendor;

                case "mouse":
                    return ActorType.Mouse;

                case "mimic":
                    return ActorType.Mimic;

                case "vampire":
                    return ActorType.Vampire;

                default:
                    return ActorType.Unknown;
            }
        }
    }
}
