﻿namespace SFMLWWC
{
    internal class Actor
    {
        private int x;
        private int y;
        private int z;
        private int energy;
        private int shields;
        private int gold;
        private List<Item> items;

        public Actor()
        {
            x = 0;
            y = 0;
            z = 0;
            energy = 100;
            shields = 100;
            gold = 50;
            items = new List<Item>();
        }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Z { get { return z; } set { z = value; } }
        public int Energy { get { return energy; } set { energy = value; } }
        public int Shields { get { return shields; } set { shields = value; } }
        public int Gold { get => gold; set { gold = value; } }
        public List<Item> Items => items;

        public void Move(int dx, int dy)
        {
            x = x + dx;
            y = y + dy;

            if (x < 0) x = Castle.WIDTH - 1;
            if (x >= Castle.WIDTH) x = 0;

            if (y < 0) y = Castle.HEIGHT - 1;
            if (y >= Castle.HEIGHT) y = 0;
        }
    }
}
