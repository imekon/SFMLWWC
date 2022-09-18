namespace WWC
{
    internal class Item
    {
        private Content contents;
        private List<Property> properties;

        public Item(Content contents)
        {
            this.contents = contents;
            properties = new List<Property>();
        }

        public Content Contents => contents;

        public int Value
        {
            get
            {
                var property = FindType(PropertyType.IntValue);
                if (property != null)
                    return ((Property<int>)property).Value;

                return 0;
            }
        }

        private Property? FindType(PropertyType type)
        {
            foreach (var property in properties)
            {
                if (property.Type == PropertyType.IntValue)
                {
                    return property;
                }
            }

            return null;
        }

        public static Item CreateStairsDown()
        {
            var item = new Item(Content.StairsDown);
            return item;
        }

        public static Item CreateStairsUp()
        {
            var item = new Item(Content.StairsUp);
            return item;
        }

        public static Item CreateGold(int value)
        {
            var item = new Item(Content.Gold);
            item.properties.Add(new Property<int>(PropertyType.IntValue, value));
            return item;
        }

        public static Item CreateFood(int value)
        {
            var item = new Item(Content.Food);
            item.properties.Add(new Property<int>(PropertyType.IntValue, value));
            return item;
        }

        public static Item CreateTorch(int value)
        {
            var item = new Item(Content.Torch);
            item.properties.Add(new Property<int>(PropertyType.IntValue, value));
            return item;
        }

        public static Item CreateSinkRoom()
        {
            var item = new Item(Content.Sink);
            return item;
        }

        public static Item CreateWarpRoom()
        {
            var item = new Item(Content.Warp);
            return item;
        }

        public static Item? CreateDagger(WeaponManager weaponManager)
        {
            var dagger = weaponManager.FindWeapon("dagger");
            if (dagger == null)
                return null;

            var item = new Item(Content.Dagger);
            item.properties.Add(new Property<int>(PropertyType.IntValue, dagger.Damage));
            return item;
        }

        public static Item? CreateSword(WeaponManager weaponManager)
        {
            var sword = weaponManager.FindWeapon("sword");
            if (sword == null)
                return null;

            var item = new Item(Content.Sword);
            item.properties.Add(new Property<int>(PropertyType.IntValue, sword.Damage));
            return item;
        }
    }
}
