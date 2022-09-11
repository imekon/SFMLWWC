namespace SFMLWWC
{
    internal class Room
    {
        private bool visited;
        private bool tripped;
        private List<Item> items;

        public Room()
        {
            visited = false;
            tripped = false;
            items = new List<Item>();
        }

        public bool Visited { get { return visited; } set { visited = value; } }
        public bool Tripped { get { return tripped; } set { tripped = value; } }
        public bool IsEmpty => !items.Any();
        public List<Item> Items => items;
    }
}
