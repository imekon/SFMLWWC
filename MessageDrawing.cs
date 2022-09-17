using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace WWC
{
    internal class MessageDrawing
    {
        private Font font;
        private Text text;
        private MessageResult result;

        public MessageDrawing(string message, Font font)
        {
            this.font = font;
            text = new Text(message, font);
            text.Position = new Vector2f(30, 300);
            result = MessageResult.Cancel;
        }

        public MessageResult Result => result;

        public void KeyPressed(Keyboard.Key key)
        {
            switch(key)
            {
                case Keyboard.Key.Escape:
                    result = MessageResult.Cancel;
                    break;

                case Keyboard.Key.N:
                    result = MessageResult.No;
                    break;

                case Keyboard.Key.Y:
                    result = MessageResult.Yes;
                    break;
            }
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(text);
        }
    }
}
