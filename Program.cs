using SFML.Graphics;
using SFML.Window;

namespace SFMLWWC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var font = new Font("pixuf.ttf");

            var player = new Actor();

            var castle = new Castle();

            var castleDrawing = new CastleDrawing(font);

            var window = new RenderWindow(new VideoMode(800, 600), "Wandering Wizard's Castle");

            window.Closed += (sender, args) => window.Close();
            
            window.SetKeyRepeatEnabled(false);
            window.KeyPressed += (sender, args) => 
            {
                switch(args.Code)
                {
                    case Keyboard.Key.Left:
                    case Keyboard.Key.A:
                        player.Move(-1, 0);
                        break;

                    case Keyboard.Key.Right:
                    case Keyboard.Key.D:
                        player.Move(1, 0);
                        break;

                    case Keyboard.Key.Up:
                    case Keyboard.Key.W:
                        player.Move(0, -1);
                        break;

                    case Keyboard.Key.Down:
                    case Keyboard.Key.S:
                        player.Move(0, 1);
                        break;

                    case Keyboard.Key.F:
                        Execute(castle, player);
                        break;

                    case Keyboard.Key.Escape:
                        window.Close();
                        break;
                }
            };

            var background = new Color(80, 80, 255);
            while(window.IsOpen)
            {
                window.DispatchEvents();
                castle.Update(player);
                window.Clear(background);
                castleDrawing.Draw(window, font, castle, player);
                window.Display();
            }
        }

        private static void Execute(Castle castle, Actor player)
        {
            var contents = castle.GetRoomContents(player);

            switch(contents)
            {
                case Content.StairsUp:
                    player.Z = player.Z - 1;
                    break;

                case Content.StairsDown:
                    player.Z = player.Z + 1;
                    break;
            }
        }
    }
}