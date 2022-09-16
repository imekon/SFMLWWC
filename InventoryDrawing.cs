using SFML.Graphics;
using SFML.Window;

namespace WWC
{
    internal class InventoryDrawing
    {
        private Font font;
        private Text text;

        public InventoryDrawing(Font font)
        {
            this.font = font;
            text = new Text("Inventory", font);
        }

        public void KeyPressed(Keyboard.Key key)
        {

        }

        public void Draw(RenderWindow window)
        {
            window.Draw(text);
        }
    }
}
