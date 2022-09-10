namespace SFMLWWC
{
    internal class Item
    {
        private Content contents;
        private float itemValue;

        public Item(Content contents, float value)
        {
            this.contents = contents;
            itemValue = value;
        }

        public Content Contents => contents;
        public float Value { get => itemValue; set { itemValue = value; } }
    }
}
