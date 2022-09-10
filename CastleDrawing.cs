using SFML.Graphics;
using SFML.System;

namespace SFMLWWC
{
    internal class CastleDrawing
    {
        private Text cursor1;
        private Text cursor2;

        public CastleDrawing(Font font)
        {
            cursor1 = new Text(">", font);
            cursor2 = new Text("<", font);
        }

        private static string ConvertContents(Content contents, bool visible)
        {
            if (!visible)
                return "?";

            switch(contents)
            {
                case Content.StairsUp:
                    return ">";

                case Content.StairsDown:
                    return "<";

                default:
                    return "X";
            }
        }

        public void Draw(RenderWindow window, Font font, Castle castle, Actor player)
        {
            for(var y = 0; y < Castle.HEIGHT; y++)
            {
                for (var x = 0; x < Castle.WIDTH; x++)
                {
                    var contents = castle.GetDrawingContents(x, y, player.Z);
                    var visible = castle.GetVisible(x, y, player.Z);
                    var text = new Text(ConvertContents(contents, visible), font);
                    text.Position = new Vector2f(x * 30 + 100, y * 30 + 50);

                    window.Draw(text);
                }
            }

            cursor1.Position = new Vector2f(player.X * 30 - 15 + 100, player.Y * 30 + 50);
            window.Draw(cursor1);

            cursor2.Position = new Vector2f(player.X * 30 + 15 + 100, player.Y * 30 + 50);
            window.Draw(cursor2);
        }
    }
}
