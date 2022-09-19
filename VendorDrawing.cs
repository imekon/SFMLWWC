using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace WWC
{
    internal class VendorDrawing
    {
        private Font font;
        private Text text;

        public VendorDrawing(Font font)
        {
            this.font = font;
            text = new Text("Vendor", font);
        }

        public bool KeyPressed(Keyboard.Key key)
        {
            switch(key)
            {
                case Keyboard.Key.Escape:
                    return false;
            }

            return true;
        }

        public void Draw(RenderWindow window, Castle castle, Actor player, Actor vendor)
        {
            text.DisplayedString = "Vendor";
            text.Position = new Vector2f(10, 10);
            window.Draw(text);

            int line = 0;
            foreach(var item in vendor.Items)
            {
                text.DisplayedString = Item.GetItemName(item.Contents);
                text.Position = new Vector2f(10, 30 + line * 20);
                window.Draw(text);
            }
        }
    }
}
