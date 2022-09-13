using System.Text;
using MoonSharp.Interpreter;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using TinyMessenger;

namespace WWC
{
    internal class Program
    {
        private static TinyMessengerHub? hub = null;
        private static Text? commandText = null;
        private static Text? statusText = null;
        private static Actor? player = null;
        private static Castle? castle = null;
        private static int cursor = 0;
        private static bool inCommand = false;

        static void Main(string[] args)
        {
            var script = new Script();

            script.Globals["go"] = (Action<int, int, int>)Go;
            script.Globals["reset"] = (Action<int>)Reset;
            script.Globals["summon"] = (Action<string>)Summon;

            script.Options.DebugPrint = s => Print(s);

            var commandBuffer = new StringBuilder();

            var random = new Random();

            hub = new TinyMessengerHub();

            var clock = new Clock();
            var font = new Font("MODES___.ttf");

            player = new Actor(ActorType.Player);
            player.Awake = true;
            player.X = (int)random.Next(Castle.WIDTH);
            player.Y = (int)random.Next(Castle.HEIGHT);

            castle = new Castle(hub);

            var castleDrawing = new CastleDrawing(font);

            var image = new Image("wwc.png");

            var window = new RenderWindow(new VideoMode(800, 600), "Wandering Wizard's Castle");

            window.SetIcon(32, 32, image.Pixels);

            window.Closed += (sender, args) => window.Close();
            
            window.SetKeyRepeatEnabled(false);
            window.KeyPressed += (sender, args) => 
            {
                if (inCommand)
                {
                    switch (args.Code)
                    {
                        case Keyboard.Key.F5:
                            inCommand = !inCommand;
                            commandBuffer.Clear();
                            break;

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
                            commandBuffer.Append((char)(args.Code + 'a'));
                            cursor++;
                            break;

                        case Keyboard.Key.Num0:
                            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                                commandBuffer.Append(")");
                            else
                                commandBuffer.Append("0");

                            cursor++;
                            break;

                        case Keyboard.Key.Num2:
                            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                                commandBuffer.Append("\"");
                            else
                                commandBuffer.Append("2");

                            cursor++;
                            break;

                        case Keyboard.Key.Num1:
                        case Keyboard.Key.Num3:
                        case Keyboard.Key.Num4:
                        case Keyboard.Key.Num5:
                        case Keyboard.Key.Num6:
                        case Keyboard.Key.Num7:
                        case Keyboard.Key.Num8:
                            commandBuffer.Append((char)(args.Code + '0' - Keyboard.Key.Num0));
                            cursor++;
                            break;

                        case Keyboard.Key.Num9:
                            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                                commandBuffer.Append("(");
                            else
                                commandBuffer.Append("9");

                            cursor++;
                            break;

                        case Keyboard.Key.Quote:
                            // NEITHER WORKS!!!
                            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                                commandBuffer.Append("@");
                            else
                                commandBuffer.Append("'");

                            cursor++;
                            break;

                        case Keyboard.Key.LBracket:
                            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                                commandBuffer.Append("{");
                            else
                                commandBuffer.Append("[");

                            cursor++;
                            break;

                        case Keyboard.Key.RBracket:
                            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                                commandBuffer.Append("}");
                            else
                                commandBuffer.Append("]");

                            cursor++;
                            break;

                        case Keyboard.Key.Space:
                            commandBuffer.Append(" ");
                            cursor++;
                            break;

                        case Keyboard.Key.Comma:
                            commandBuffer.Append(",");
                            cursor++;
                            break;

                        case Keyboard.Key.Backspace:
                            commandBuffer.Remove(commandBuffer.Length - 1, 1);
                            cursor--;
                            break;

                        case Keyboard.Key.Enter:
                            try
                            {
                                script.DoString(commandBuffer.ToString());
                            }
                            catch(Exception ex)
                            {
                                hub.Publish(new StatusMessage(new object(), ex.Message));
                            }
                            commandBuffer.Clear();
                            cursor = 0;
                            break;
                    }

                    commandText = new Text("> " + commandBuffer.ToString(), font);
                    commandText.Position = new Vector2f(10, 10);
                }
                else
                {
                    Actor? monster = null;

                    switch (args.Code)
                    {
                        case Keyboard.Key.F5:
                            inCommand = !inCommand;
                            commandBuffer.Clear();
                            break;

                        case Keyboard.Key.Left:
                        case Keyboard.Key.A:
                            castle.MoveOrAttack(player, -1, 0);
                            break;

                        case Keyboard.Key.Right:
                        case Keyboard.Key.D:
                            castle.MoveOrAttack(player, 1, 0);
                            break;

                        case Keyboard.Key.Up:
                        case Keyboard.Key.W:
                            castle.MoveOrAttack(player, 0, -1);
                            break;

                        case Keyboard.Key.Down:
                        case Keyboard.Key.S:
                            castle.MoveOrAttack(player, 0, 1);
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
                }
            };

            var background = new Color(80, 80, 255);
            commandText = new Text("> ", font);
            commandText.Position = new Vector2f(10, 10);

            try
            {
                if (File.Exists("setup.lua"))
                    script.DoFile("setup.lua");
                else
                    hub.Publish(new StatusMessage(new object(), "Can't find setup.lua"));
            }
            catch(Exception ex)
            {
                hub.Publish(new StatusMessage(new object(), ex.Message));
            }

            while(window.IsOpen)
            {
                window.DispatchEvents();
                castle.Update(clock.ElapsedTime, player);
                window.Clear(background);
                if (inCommand)
                {
                    window.Draw(commandText);
                    statusText = new Text(castle.Status, font);
                    statusText.Position = new Vector2f(10, 500);
                    window.Draw(statusText);
                }
                else
                    castleDrawing.Draw(window, font, castle, player);

                window.Display();
            }
        }

        private static void Print(string text)
        {
            hub.Publish(new StatusMessage(new object(), text));
        }

        private static void Go(int x, int y, int z)
        {
            player.X = x;
            player.Y = y;
            player.Z = z;

            inCommand = false;
        }

        private static void Reset(int what = 0)
        {
            player.Energy = 100;
            player.Shields = 100;

            inCommand = false;
        }

        private static void Summon(string what)
        {
            Item? item = null;
            Room? room = null;

            var random = new Random();

            switch(what)
            {
                case "torch":
                    {
                        item = Item.CreateTorch(50);
                        var x = player!.X - 1 + random.Next(2);
                        var y = player!.Y - 1 + random.Next(2);
                        (x, y) = castle!.Clamp(x, y);
                        room = castle!.GetRoom(x, y, player!.Z);
                        room.Items.Add(item);
                    }
                    break;

                case "food":
                    {
                        item = Item.CreateFood(50);
                        var x = player!.X - 1 + random.Next(2);
                        var y = player!.Y - 1 + random.Next(2);
                        (x, y) = castle!.Clamp(x, y);
                        room = castle!.GetRoom(x, y, player!.Z);
                        room.Items.Add(item);
                    }
                    break;
            }

            inCommand = false;
        }
    }
}