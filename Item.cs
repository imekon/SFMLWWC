namespace SFMLWWC
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
    }
}
