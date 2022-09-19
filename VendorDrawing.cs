using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
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

        public void Draw(RenderWindow window)
        {
            window.Draw(text);
        }
    }
}
