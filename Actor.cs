using System.Numerics;
using SFML.System;

namespace SFMLWWC
{
    internal class Actor
    {
        private ActorType type;
        private int x;
        private int y;
        private int z;
        private int energy;
        private int minEnergy;
        private int maxEnergy;
        private int shields;
        private int gold;
        private int lighting;
        private List<Item> items;

        private Time energyWhen;

        public Actor(ActorType type)
        {
            this.type = type;
            x = 0;
            y = 0;
            z = 0;
            energy = 100;
            minEnergy = 10;
            maxEnergy = 100;
            shields = 100;
            gold = 50;
            lighting = 0;
            items = new List<Item>();
            energyWhen = new Time();
        }

        public ActorType ActorType => type;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Z { get { return z; } set { z = value; } }
        public int Energy { get { return energy; } set { energy = value; } }
        public int MinEnergy => minEnergy;
        public int Shields { get { return shields; } set { shields = value; } }
        public int Gold { get => gold; set { gold = value; } }
        public int Lighting => lighting;

        public int TorchCount
        {
            get
            {
                int count = 0;

                foreach(var item in items)
                {
                    if (item.Contents == Content.Torch)
                        count++;
                }

                return count;
            }
        }

        private Item? FindLight()
        {
            foreach(var item in items)
            {
                if (item.Contents == Content.Torch || item.Contents == Content.Lantern)
                    return item;
            }

            return null;
        }

        public List<Item> Items => items;

        public void Move(int dx, int dy)
        {
            if (energy < minEnergy)
                return;

            x = x + dx;
            y = y + dy;

            if (x < 0) x = Castle.WIDTH - 1;
            if (x >= Castle.WIDTH) x = 0;

            if (y < 0) y = Castle.HEIGHT - 1;
            if (y >= Castle.HEIGHT) y = 0;

            energy--;

            if (lighting > 0)
                lighting--;
        }

        public void Update(Time elapsed)
        {
            var energyDur = elapsed - energyWhen;
            if (energyDur.AsSeconds() > 1)
            {
                energyWhen = elapsed;
                energy++;
                if (energy > maxEnergy)
                    energy = maxEnergy;
            }
        }

        public void Light()
        {
            var light = FindLight();
            if (light == null)
                return;

            items.Remove(light);
            lighting += light.Value;
        }
    }
}
