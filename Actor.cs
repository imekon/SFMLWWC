namespace SFMLWWC
{
    internal class Actor
    {
        private int x;
        private int y;
        private int z;
        private int energy;
        private int shields;

        public Actor()
        {
            x = 0;
            y = 0;
            z = 0;
            energy = 100;
            shields = 100;
        }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Z { get { return z; } set { z = value; } }
        public int Energy { get { return energy; } set { energy = value; } }
        public int Shields { get { return shields; } set { shields = value; } }
    }
}
