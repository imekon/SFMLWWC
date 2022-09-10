using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLWWC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var font = new Font("pixuf.ttf");
            var text = new Text("Wandering Wizard's Castle", font, 24);
            text.Position = new Vector2f(10, 10);

            var player = new Actor();

            var castle = new Castle();

            var castleDrawing = new CastleDrawing(font);

            var window = new RenderWindow(new VideoMode(800, 600), "Wandering Wizard's Castle");

            window.Closed += (sender, args) => window.Close();

            var background = new Color(80, 80, 255);
            while(window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(background);
                castleDrawing.Draw(window, font, castle, player);
                window.Display();
            }
        }
    }
}