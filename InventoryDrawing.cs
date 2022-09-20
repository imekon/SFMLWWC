using SFML.Graphics;
using SFML.Window;

namespace WWC
{
    internal class InventoryDrawing : SelectionDrawing<Item>
    {
        public InventoryDrawing(Font font) : base(font)
        {
        }

        protected override string GetText(Item item)
        {
            switch (item.Contents)
            {
                case Content.Boots:
                    return "Boots";

                case Content.Dagger:
                    return $"Dagger - {item.Value} damage";

                case Content.Sword:
                    return $"Sword - {item.Value} damage";

                case Content.Axe:
                    return "Axe";

                case Content.Scroll:
                    return "Scroll";
            }

            return "Unknown";
        }
    }
}
