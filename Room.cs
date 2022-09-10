namespace SFMLWWC
{
    internal class Room
    {
        private bool visited;
        private bool tripped;
        private Content contents;

        public Room()
        {
            visited = false;
            tripped = false;
            contents = Content.Empty;
        }

        public bool Visited { get { return visited; } set { visited = value; } }
        public bool Tripped { get { return tripped; } set { tripped = value; } }
        public Content Contents { get { return contents; } set { contents = value; } }
    }
}
