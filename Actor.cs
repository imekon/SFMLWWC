using SFML.System;

namespace WWC
{
    internal class Actor : IGameContainer<Item>
    {
        private ActorType type;
        private bool awake;
        private bool dead;
        private int x;
        private int y;
        private int z;
        private int energy;
        private int minEnergy;
        private int maxEnergy;
        private int strength;
        private int dexterity;
        private int iq;
        private int gold;
        private int lighting;
        private List<Item> items;
        private Item? weapon;
        private Item? armour;

        private Time energyWhen;

        public Actor(ActorType type)
        {
            this.type = type;
            awake = false;
            dead = false;
            x = 0;
            y = 0;
            z = 0;
            energy = 100;
            minEnergy = 10;
            maxEnergy = 100;
            strength = 6;
            dexterity = 6;
            iq = 6;
            gold = 50;
            lighting = 0;
            items = new List<Item>();
            energyWhen = new Time();
            weapon = null;
            armour = null;
        }

        public ActorType ActorType => type;
        public bool Awake { get => awake; set { awake = value; } }
        public bool Dead { get => dead; set { dead = value; } }
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Z { get { return z; } set { z = value; } }
        public int Energy { get { return energy; } set { energy = value; } }
        public int MinEnergy => minEnergy;
        public int Strength { get => strength; set { strength = value; } }
        public int Dexterity { get => dexterity; set { dexterity = value; } }
        public int IQ { get => iq; set { iq = value; }  }
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

        public string Title
        {
            get
            {
                if (type == ActorType.Vendor)
                    return "Vendor";

                return "Inventory";
            }
        }
        public List<Item> Items => items;
        public Item? Weapon { get => weapon; set { weapon = value; } }
        public Item? Armour { get => armour; set { armour = value; } }

        private Item? FindLight()
        {
            foreach(var item in items)
            {
                if (item.Contents == Content.Torch || item.Contents == Content.Lantern)
                    return item;
            }

            return null;
        }

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
            if (energy < 0) energy = 0;

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
