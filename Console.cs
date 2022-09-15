using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace WWC
{
    internal class Console
    {
        private Font font;
        private uint fontSize;
        private StringBuilder buffer;
        private int cursor;
        private float charWidth;
        private Action<string> execute;

        public Console(Font font, uint fontSize, Action<string> execute)
        {
            this.font = font;
            this.fontSize = fontSize;
            buffer = new StringBuilder();
            cursor = 0;
            this.execute = execute;

            var glyph = font.GetGlyph(65, fontSize, false, 1);
            charWidth = glyph.Advance;
        }

        public bool KeyPressed(Keyboard.Key key)
        {
            switch (key)
            {
                case Keyboard.Key.F5:
                    buffer.Clear();
                    return false;

                case Keyboard.Key.A:
                case Keyboard.Key.B:
                case Keyboard.Key.C:
                case Keyboard.Key.D:
                case Keyboard.Key.E:
                case Keyboard.Key.F:
                case Keyboard.Key.G:
                case Keyboard.Key.H:
                case Keyboard.Key.I:
                case Keyboard.Key.J:
                case Keyboard.Key.K:
                case Keyboard.Key.L:
                case Keyboard.Key.M:
                case Keyboard.Key.N:
                case Keyboard.Key.O:
                case Keyboard.Key.P:
                case Keyboard.Key.Q:
                case Keyboard.Key.R:
                case Keyboard.Key.S:
                case Keyboard.Key.T:
                case Keyboard.Key.U:
                case Keyboard.Key.V:
                case Keyboard.Key.W:
                case Keyboard.Key.X:
                case Keyboard.Key.Y:
                case Keyboard.Key.Z:
                    buffer.Append((char)(key + 'a'));
                    cursor++;
                    break;

                case Keyboard.Key.Num0:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                        buffer.Append(")");
                    else
                        buffer.Append("0");

                    cursor++;
                    break;

                case Keyboard.Key.Num2:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                        buffer.Append("\"");
                    else
                        buffer.Append("2");

                    cursor++;
                    break;

                case Keyboard.Key.Num1:
                case Keyboard.Key.Num3:
                case Keyboard.Key.Num4:
                case Keyboard.Key.Num5:
                case Keyboard.Key.Num6:
                case Keyboard.Key.Num7:
                case Keyboard.Key.Num8:
                    buffer.Append((char)(key + '0' - Keyboard.Key.Num0));
                    cursor++;
                    break;

                case Keyboard.Key.Num9:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                        buffer.Append("(");
                    else
                        buffer.Append("9");

                    cursor++;
                    break;

                case Keyboard.Key.Quote:
                    // NEITHER WORKS!!!
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                        buffer.Append("@");
                    else
                        buffer.Append("'");

                    cursor++;
                    break;

                case Keyboard.Key.LBracket:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                        buffer.Append("{");
                    else
                        buffer.Append("[");

                    cursor++;
                    break;

                case Keyboard.Key.RBracket:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                        buffer.Append("}");
                    else
                        buffer.Append("]");

                    cursor++;
                    break;

                case Keyboard.Key.Space:
                    buffer.Append(" ");
                    cursor++;
                    break;

                case Keyboard.Key.Comma:
                    buffer.Append(",");
                    cursor++;
                    break;

                case Keyboard.Key.Equal:
                    buffer.Append("=");
                    cursor++;
                    break;

                case Keyboard.Key.Backspace:
                    buffer.Remove(buffer.Length - 1, 1);
                    cursor--;
                    break;

                case Keyboard.Key.Enter:
                    execute(buffer.ToString());
                    buffer.Clear();
                    cursor = 0;
                    break;
            }

            return true;
        }

        public void Draw(RenderWindow window)
        {
            var text = new Text("> " + buffer.ToString(), font, fontSize);
            text.Position = new Vector2f(10, 10);

            var line = new RectangleShape(new Vector2f(30, 2));
            line.Rotation = 90;
            line.Position = new Vector2f(10 + 3 + (cursor + 2) * charWidth, 10);

            window.Draw(text);
            window.Draw(line);
        }
    }
}
