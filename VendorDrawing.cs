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
                case Keyboard.Key.B:
                    // TODO buy
                    break;

                case Keyboard.Key.S:
                    // TODO sell
                    break;

                case Keyboard.Key.A:
                    // TODO set vendor to hostile
                    return false;

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
                text.Position = new Vector2f(10, 40 + line * 26); line++;
                window.Draw(text);
            }

            text.DisplayedString = "(B)uy (S)ell (A)ttack";
            text.Position = new Vector2f(10, 500);
            window.Draw(text);
        }
    }
}
