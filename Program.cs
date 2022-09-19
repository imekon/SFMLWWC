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
        private static WeaponManager? weaponManager = null;
        private static Text? statusText = null;
        private static Actor? player = null;
        private static Castle? castle = null;
        private static Console? console = null;
        private static Script? script = null;
        private static MessageDrawing? messageDrawing = null;

        static void Main(string[] args)
        {
            script = new Script();

            script.Globals["go"] = (Action<int, int, int>)Go;
            script.Globals["reset"] = (Action<int>)Reset;
            script.Globals["summon"] = (Action<string>)Summon;
            script.Globals["light"] = (Action)Light;
            script.Globals["dark"] = (Action)Dark;

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
            weaponManager = new WeaponManager();

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
                        var monster = new MonsterTemplate(name.String, (int)strength.Number, (int)dexterity.Number, (int)iq.Number, (int)armour.Number, (int)lowest.Number, (int)hightest.Number);
                        monsterManager.AddMonster(monster);
                    }

                    var weapons = script.Globals.Get("weapons");

                    foreach(var pair in weapons.Table.Pairs)
                    {
                        var name = pair.Value.Table.Get("name");
                        var damage = pair.Value.Table.Get("damage");
                        var magical = pair.Value.Table.Get("magical");
                        var weild = pair.Value.Table.Get("weild");
                        var weapon = new WeaponTemplate(name.String, (int)damage.Number, magical.Boolean, weild.Boolean);
                        weaponManager.AddWeapon(weapon);
                    }
                }
                else
                    hub.Publish(new StatusMessage(new object(), "Can't find setup.lua"));
            }
            catch (Exception ex)
            {
                hub.Publish(new StatusMessage(new object(), ex.Message));
            }

            castle = new Castle(hub, monsterManager, weaponManager);

            var castleDrawing = new CastleDrawing(font);
            var inventoryDrawing = new InventoryDrawing(font);
            var vendorDrawing = new VendorDrawing(font);

            var image = new Image("wwc.png");

            console = new Console(font, 24, ConsoleExecute);

            var window = new RenderWindow(new VideoMode(800, 600), "Wandering Wizard's Castle");

            window.SetIcon(32, 32, image.Pixels);

            window.Closed += (sender, args) => window.Close();
            
            window.SetKeyRepeatEnabled(false);
            window.KeyPressed += (sender, args) => 
            {
                switch (castle.State)
                {
                    case CommandState.Command:
                        if (!console.KeyPressed(args.Code))
                            castle.State = CommandState.Playing;
                        break;

                    case CommandState.Inventory:
                        if (!inventoryDrawing.KeyPressed(args.Code))
                            castle.State = CommandState.Playing;
                        break;

                    case CommandState.Vendor:
                        if (!vendorDrawing.KeyPressed(args.Code))
                            castle.State = CommandState.Playing;
                        break;

                    case CommandState.DisplayMessage:
                        if (messageDrawing != null)
                            messageDrawing.KeyPressed(args.Code);
                        break;

                    case CommandState.Playing:
                        // TODO: move into CastleDrawing
                        switch (args.Code)
                        {
                            case Keyboard.Key.F5:
                                castle.State = CommandState.Command;
                                break;

                            case Keyboard.Key.I:
                                castle.State = CommandState.Inventory;
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
                        break;
                }
            };

            var background = new Color(80, 80, 255);

            while(window.IsOpen)
            {
                window.DispatchEvents();

                if (player.Dead)
                    DisplayMessage("Do you wish to become a zombie?", font);

                castle.Update(clock.ElapsedTime, player);
                window.Clear(background);
                
                switch (castle.State)
                {
                    case CommandState.Command:
                        console.Draw(window);
                        statusText = new Text(castle.Status, font);
                        statusText.Position = new Vector2f(10, 500);
                        window.Draw(statusText);
                        break;

                    case CommandState.Inventory:
                        inventoryDrawing.Draw(window, castle, player);
                        break;

                    case CommandState.Playing:
                        castleDrawing.Draw(window, castle, player);
                        break;

                    case CommandState.Vendor:
                        vendorDrawing.Draw(window);
                        break;

                    case CommandState.DisplayMessage:
                        if (messageDrawing != null)
                            messageDrawing.Draw(window);
                        break;
                }

                window.Display();
            }
        }

        private static void DisplayMessage(string message, Font font)
        {
            messageDrawing = new MessageDrawing(message, font);
            castle!.State = CommandState.DisplayMessage;
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

            castle!.State = CommandState.Playing;
        }

        private static void Reset(int what = 0)
        {
            player!.Energy = 100;

            castle!.State = CommandState.Playing;
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

            castle.State = CommandState.Playing;
        }

        private static void Light()
        {
            castle!.Light(player!.Z);
        }

        private static void Dark()
        {
            castle!.Dark(player!.Z);
        }

        private static void ConsoleExecute(string command)
        {
            try
            {
                script!.DoString(command);
                castle!.State = CommandState.Playing;
            }
            catch (Exception ex)
            {
                hub!.Publish(new StatusMessage(script!, ex.Message));
            }
        }
    }
}