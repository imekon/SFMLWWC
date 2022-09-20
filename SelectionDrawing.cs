using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace WWC
{
    internal abstract class SelectionDrawing<T>
    {
        private Font font;
        protected int selected;
        protected Text text;

        public SelectionDrawing(Font font)
        {
            this.font = font;
            selected = -1;
            text = new Text("Title", font);
        }

        public virtual bool KeyPressed(Keyboard.Key key)
        {
            switch (key)
            {
                case Keyboard.Key.Up:
                    selected--;
                    break;

                case Keyboard.Key.Down:
                    selected++;
                    break;

                case Keyboard.Key.Escape:
                    return false;
            }

            return true;
        }

        protected abstract string GetText(T item);

        public void Draw(RenderWindow window, IGameContainer<T> gameContainer)
        {
            text.DisplayedString = gameContainer.Title;
            text.Position = new Vector2f(10, 10);
            window.Draw(text);

            if (selected >= 0)
            {
                text.DisplayedString = ">";
                text.Position = new Vector2f(10, selected * 28 + 46);
                window.Draw(text);
            }

            int line = 0;

            foreach (var item in gameContainer.Items)
            {
                text.DisplayedString = GetText(item);
                text.Position = new Vector2f(30, 46 + 28 * line++);
                window.Draw(text);
            }
        }
    }
}
