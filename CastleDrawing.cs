using SFML.Graphics;
using SFML.System;

namespace WWC
{
    internal class CastleDrawing
    {
        private const int LEFT_MARGIN = 250;
        private const int TOP_MARGIN = 50;
        private const int HORZ_SPACING = 40;
        private const int VERT_SPACING = 30;
        private const int CURSOR_OFFSET = 15;

        private Text cursor1;
        private Text cursor2;
        private Text playerText;
        private Text text;

        public CastleDrawing(Font font)
        {
            cursor1 = new Text("<", font);
            cursor2 = new Text(">", font);
            playerText = new Text("@", font);
            text = new Text("?", font);
        }

        private static string ConvertContents(Content contents, Actor? monster, bool visible, bool tripped)
        {
            if (!visible)
                return "?";

            if (monster != null)
            {
                switch(monster.ActorType)
                {
                    case ActorType.Player:
                    case ActorType.Wizard:
                    case ActorType.WanderingWizard:
                        return "@";

                    case ActorType.Vendor:
                        return "V";

                    case ActorType.Mouse:
                    case ActorType.Rat:
                    case ActorType.Dog:
                        return "m";

                    case ActorType.Vampire:
                    case ActorType.Wyvern:
                        return "M";
                }
            }

            switch(contents)
            {
                case Content.Empty:
                    return ".";

                case Content.StairsUp:
                    return "<";

                case Content.StairsDown:
                    return ">";

                case Content.Gold:
                    return "$";

                case Content.Food:
                    return ",";

                case Content.Torch:
                    return "T";

                case Content.Scroll:
                    return "s";

                case Content.Sink:
                    if (tripped)
                        return " ";
                    else
                        return ".";

                case Content.Warp:
                    if (tripped)
                        return "W";
                    else
                        return ".";

                case Content.Dagger:
                case Content.Sword:
                    return "w";

                default:
                    return "X";
            }
        }

        public void Draw(RenderWindow window, Castle castle, Actor player)
        {
            for(var y = 0; y < Castle.HEIGHT; y++)
            {
                for (var x = 0; x < Castle.WIDTH; x++)
                {
                    var contents = castle.GetRoomContents(x, y, player.Z);
                    var visible = castle.GetVisible(x, y, player.Z);
                    var tripped = castle.GetTripped(x, y, player.Z);
                    var monster = castle.GetRoomMonster(x, y, player.Z);
                    text.DisplayedString = ConvertContents(contents, monster, visible, tripped);
                    text.Position = new Vector2f(x * HORZ_SPACING + LEFT_MARGIN, y * VERT_SPACING + TOP_MARGIN);

                    window.Draw(text);
                }
            }

            cursor1.Position = new Vector2f(player.X * HORZ_SPACING - CURSOR_OFFSET + LEFT_MARGIN, player.Y * VERT_SPACING + TOP_MARGIN);
            window.Draw(cursor1);

            playerText.Position = new Vector2f(player.X * HORZ_SPACING + LEFT_MARGIN, player.Y * VERT_SPACING + TOP_MARGIN);
            window.Draw(playerText);

            cursor2.Position = new Vector2f(player.X * HORZ_SPACING + CURSOR_OFFSET + LEFT_MARGIN, player.Y * VERT_SPACING + TOP_MARGIN);
            window.Draw(cursor2);

            int line = 0;
            text.DisplayedString = $"Level:   {player.Z + 1}";
            text.Position = new Vector2f(10, 300 + line * 30); line++;
            window.Draw(text);

            text.DisplayedString = $"Energy:  {player.Energy}";
            if (player.Energy < player.MinEnergy)
                text.FillColor = Color.Red;

            text.Position = new Vector2f(10, 300 + line * 30); line++;
            window.Draw(text);
            text.FillColor = Color.White;

            text.DisplayedString =$"Gold:    {player.Gold}";
            text.Position = new Vector2f(10, 300 + line * 30); line++;
            window.Draw(text);

            text.DisplayedString = $"Torches: {player.TorchCount} ({player.Lighting})";
            text.Position = new Vector2f(10, 300 + line * 30); line++;
            window.Draw(text);

            text.DisplayedString = castle.Status;
            text.Position = new Vector2f(10, 550);
            window.Draw(text);
        }
    }
}
