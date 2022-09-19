using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace WWC
{
    internal class SelectionDrawingBase
    {
        private Font font;
        private string title;
        protected int selected;
        protected Text text;

        public SelectionDrawingBase(Font font, string title)
        {
            this.font = font;
            this.title = title;
            selected = -1;
            text = new Text(title, font);
        }

        public virtual bool KeyPressed(Keyboard.Key key)
        {
            switch(key)
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

        public virtual void Draw(RenderWindow window)
        {
            text.DisplayedString = title;
            text.Position = new Vector2f(10, 10);
            window.Draw(text);
        }
    }

    internal abstract class SelectionDrawing<T> : SelectionDrawingBase
    {
        private List<T> items;

        public SelectionDrawing(Font font, string title) : base(font, title)
        {
            items = new List<T>();
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }

        protected abstract string GetText(T item);

        public override void Draw(RenderWindow window)
        {
            base.Draw(window);

            int line = 0;

            foreach(var item in items)
            {
                text.DisplayedString = GetText(item);
                text.Position = new Vector2f(10, 30 + 26 * line++);
                window.Draw(text);
            }
        }
    }
}
