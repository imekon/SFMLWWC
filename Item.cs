namespace SFMLWWC
{
    internal class Item
    {
        private ItemType itemType;
        private float itemValue;

        public Item(ItemType itemType, float value)
        {
            this.itemType = itemType;
            itemValue = value;
        }

        public ItemType ItemType => itemType;
        public float Value { get => itemValue; set { itemValue = value; } }
    }
}
