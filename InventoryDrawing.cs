using SFML.Graphics;
using SFML.System;
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

        public bool KeyPressed(Keyboard.Key key)
        {
            switch(key)
            {
                case Keyboard.Key.Escape:
                    return false;
            }

            return true;
        }

        public void Draw(RenderWindow window, Castle castle, Actor player)
        {
            var line = 0;
            var count = 0;

            text.DisplayedString = "Inventory";
            text.Position = new Vector2f(10, 10 + line * 26);
            line += 2;
            window.Draw(text);

            foreach(var item in player.Items)
            {
                switch(item.Contents)
                {
                    case Content.Boots:
                        text.DisplayedString = "Boots";
                        text.Position = new Vector2f(10, 10 + line * 26); line++;
                        window.Draw(text);
                        break;

                    case Content.Dagger:
                        text.DisplayedString = $"Dagger - {item.Value} damage";
                        text.Position = new Vector2f(10, 10 + line * 26); line++;
                        window.Draw(text);
                        break;

                    case Content.Sword:
                        text.DisplayedString = $"Sword - {item.Value} damage";
                        text.Position = new Vector2f(10, 10 + line * 26); line++;
                        window.Draw(text);
                        break;

                    case Content.Axe:
                        text.DisplayedString = "Axe";
                        text.Position = new Vector2f(10, 10 + line * 26); line++;
                        window.Draw(text);
                        break;

                    case Content.Scroll:
                        text.DisplayedString = "Scroll";
                        text.Position = new Vector2f(10, 10 + line * 26); line++;
                        window.Draw(text);
                        break;
                }

                count++;

                if (count >= 10)
                    break;
            }
        }
    }
}
