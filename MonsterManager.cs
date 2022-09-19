namespace WWC
{
    internal class MonsterManager
    {
        private Dictionary<string, MonsterTemplate> monsters;

        public MonsterManager()
        {
            monsters = new Dictionary<string, MonsterTemplate>();
        }

        public List<MonsterTemplate> Monsters => monsters.Values.ToList();

        public void AddMonster(MonsterTemplate monster)
        {
            monsters.Add(monster.Name, monster);
        }

        public MonsterTemplate? FindMonster(string name)
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
