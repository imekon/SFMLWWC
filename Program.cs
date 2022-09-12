using SFML.Graphics;
using SFML.System;
using SFML.Window;
using TinyMessenger;

namespace SFMLWWC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();

            var hub = new TinyMessengerHub();

            var clock = new Clock();
            var font = new Font("MODES___.ttf");

            var player = new Actor(ActorType.Player);
            player.X = (int)random.NextInt64(Castle.WIDTH);
            player.Y = (int)random.NextInt64(Castle.HEIGHT);

            var castle = new Castle(hub);

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
                        castle.Execute(player);
                        break;

                    case Keyboard.Key.L:
                        player.Light();
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
                castle.Update(clock.ElapsedTime, player);
                window.Clear(background);
                castleDrawing.Draw(window, font, castle, player);
                window.Display();
            }
        }
    }
}