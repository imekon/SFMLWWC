namespace WWC
{
    internal class Property
    {
        private PropertyType propertyType;

        public Property(PropertyType type)
        {
            propertyType = type;
        }

        public PropertyType Type => propertyType;
    }

    internal class Property<T> : Property
    {
        private T propertyValue;

        public Property(PropertyType type, T value) : base(type)
        { 
            propertyValue = value;
        }

        public T Value
        {
            get => propertyValue;
            set
            {
                propertyValue = value;
            }
        }
    }
}
