using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace WWC
{
    internal class VendorDrawing : SelectionDrawing<Item>
    {
        public VendorDrawing(Font font) : base(font, "Vendor")
        {
        }

        protected override string GetText(Item item)
        {
            return Item.GetItemName(item.Contents);
        }
    }
}
