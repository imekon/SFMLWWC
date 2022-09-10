namespace SFMLWWC
{
    internal class Room
    {
        private bool visited;
        private bool tripped;
        private Content contents;
        private Item? item;

        public Room()
        {
            visited = false;
            tripped = false;
            contents = Content.Empty;
            item = null;
        }

        public bool Visited { get { return visited; } set { visited = value; } }
        public bool Tripped { get { return tripped; } set { tripped = value; } }
        public Content Contents { get { return contents; } set { contents = value; } }
        public Item? Item { get => item; set { item = value; } }
    }
}
