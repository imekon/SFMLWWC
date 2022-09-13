namespace WWC
{
    internal class Room
    {
        private bool visited;
        private bool tripped;
        private List<Item> items;
        private List<Actor> monsters;

        public Room()
        {
            visited = false;
            tripped = false;
            items = new List<Item>();
            monsters = new List<Actor>();
        }

        public bool Visited { get { return visited; } set { visited = value; } }
        public bool Tripped { get { return tripped; } set { tripped = value; } }
        public bool IsEmpty => !items.Any();
        public List<Item> Items => items;
        public List<Actor> Monsters => monsters;
    }
}
