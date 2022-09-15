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
        private static MonsterManager? monsterManager = null;
        private static Text? statusText = null;
        private static Actor? player = null;
        private static Castle? castle = null;
        private static bool inCommand = false;
        private static Console? console = null;
        private static Script? script = null;

        static void Main(string[] args)
        {
            script = new Script();

            script.Globals["go"] = (Action<int, int, int>)Go;
            script.Globals["reset"] = (Action<int>)Reset;
            script.Globals["summon"] = (Action<string>)Summon;

            script.Options.DebugPrint = s => Print(s);

            var random = new Random();

            hub = new TinyMessengerHub();

            var clock = new Clock();
            var font = new Font("MODES___.ttf");

            player = new Actor(ActorType.Player);
            player.Awake = true;
            player.X = (int)random.Next(Castle.WIDTH);
            player.Y = (int)random.Next(Castle.HEIGHT);

            monsterManager = new MonsterManager();

            try
            {
                if (File.Exists("setup.lua"))
                {
                    script.DoFile("setup.lua");

                    var monsters = script.Globals.Get("monsters");

                    foreach (var pair in monsters.Table.Pairs)
                    {
                        var name = pair.Value.Table.Get("name");
                        var strength = pair.Value.Table.Get("strength");
                        var dexterity = pair.Value.Table.Get("dexterity");
                        var iq = pair.Value.Table.Get("iq");
                        var armour = pair.Value.Table.Get("armour");
                        var lowest = pair.Value.Table.Get("lowest");
                        var hightest = pair.Value.Table.Get("highest");
                        var monster = new Monster(name.String, (int)strength.Number, (int)dexterity.Number, (int)iq.Number, (int)armour.Number, (int)lowest.Number, (int)hightest.Number);
                        monsterManager.Add(monster);
                    }
                }
                else
                    hub.Publish(new StatusMessage(new object(), "Can't find setup.lua"));
            }
            catch (Exception ex)
            {
                hub.Publish(new StatusMessage(new object(), ex.Message));
            }

            castle = new Castle(hub, monsterManager);

            var castleDrawing = new CastleDrawing(font);

            var image = new Image("wwc.png");

            console = new Console(font, 24, ConsoleExecute);

            var window = new RenderWindow(new VideoMode(800, 600), "Wandering Wizard's Castle");

            window.SetIcon(32, 32, image.Pixels);

            window.Closed += (sender, args) => window.Close();
            
            window.SetKeyRepeatEnabled(false);
            window.KeyPressed += (sender, args) => 
            {
                if (inCommand)
                {
                    inCommand = console.KeyPressed(args.Code);
                }
                else
                {
                    switch (args.Code)
                    {
                        case Keyboard.Key.F5:
                            inCommand = !inCommand;
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

            while(window.IsOpen)
            {
                window.DispatchEvents();
                castle.Update(clock.ElapsedTime, player);
                window.Clear(background);
                if (inCommand)
                {
                    console.Draw(window);
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
            hub!.Publish(new StatusMessage(new object(), text));
        }

        private static void Go(int x, int y, int z)
        {
            player!.X = x;
            player.Y = y;
            player.Z = z;

            inCommand = false;
        }

        private static void Reset(int what = 0)
        {
            player!.Energy = 100;

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

                default:
                    {
                        var type = monsterManager!.GetActorType(what);
                        var monster = new Actor(type);
                        var x = player!.X - 1 + random.Next(2);
                        var y = player!.Y - 1 + random.Next(2);
                        (x, y) = castle!.Clamp(x, y);
                        room = castle!.GetRoom(x, y, player!.Z);
                        room.Monsters.Add(monster);
                    }
                    break;
            }

            inCommand = false;
        }

        private static void ConsoleExecute(string command)
        {
            try
            {
                script!.DoString(command);
                inCommand = false;
            }
            catch(Exception ex)
            {
                hub!.Publish(new StatusMessage(script!, ex.Message));
            }
        }
    }
}