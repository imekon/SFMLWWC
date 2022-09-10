using SFML.Graphics;
using SFML.System;

namespace SFMLWWC
{
    internal class CastleDrawing
    {
        private const int LEFT_MARGIN = 200;
        private const int TOP_MARGIN = 50;
        private const int HORZ_SPACING = 40;
        private const int VERT_SPACING = 30;
        private const int CURSOR_OFFSET = 15;

        private Text cursor1;
        private Text cursor2;

        public CastleDrawing(Font font)
        {
            cursor1 = new Text("[", font);
            cursor2 = new Text("]", font);
        }

        private static string ConvertContents(Content contents, bool visible)
        {
            if (!visible)
                return "?";

            switch(contents)
            {
                case Content.Empty:
                    return ".";

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
                    text.Position = new Vector2f(x * HORZ_SPACING + LEFT_MARGIN, y * VERT_SPACING + TOP_MARGIN);

                    window.Draw(text);
                }
            }

            cursor1.Position = new Vector2f(player.X * HORZ_SPACING - CURSOR_OFFSET + LEFT_MARGIN, player.Y * VERT_SPACING + TOP_MARGIN);
            window.Draw(cursor1);

            cursor2.Position = new Vector2f(player.X * HORZ_SPACING + CURSOR_OFFSET + LEFT_MARGIN, player.Y * VERT_SPACING + TOP_MARGIN);
            window.Draw(cursor2);
        }
    }
}
