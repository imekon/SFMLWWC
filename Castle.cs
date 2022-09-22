using System.Text.Json;
using System.Text.Json.Serialization;
using SFML.System;
using TinyMessenger;

namespace WWC
{
    internal class Castle
    {
        public const int WIDTH = 8;
        public const int HEIGHT = 8;
        public const int DEPTH = 50;

        private TinyMessengerHub messengerHub;
        private MonsterManager monsterManager;
        private WeaponManager weaponManager;
        private string status;
        private Random random;
        private Room[,,] rooms;
        private List<Actor> monsters;
        private Actor? vendor;
        private Time elapsedTime;
        private Time statusWhen;
        private CommandState state;

        public Castle(TinyMessengerHub hub, MonsterManager monstreManager, WeaponManager weaponManager)
        {
            this.monsterManager = monstreManager;
            this.weaponManager = weaponManager;

            messengerHub = hub;

            status = "Ready";

            elapsedTime = new Time();
            statusWhen = new Time();

            random = new Random();

            monsters = new List<Actor>();
            rooms = new Room[WIDTH, HEIGHT, DEPTH];
            vendor = null;

            // Empty all the rooms
            for (var z = 0; z < DEPTH; z++)
                for (var y = 0; y < HEIGHT; y++)
                    for (var x = 0; x < WIDTH; x++)
                    {
                        var room = new Room();
                        rooms[x, y, z] = room;
                    }

            // Set the stairs down
            for (var z = 0; z < DEPTH; z++)
            {
                var x = random.Next(WIDTH);
                var y = random.Next(HEIGHT);

                var room = rooms[x, y, z];
                room.Items.Add(Item.CreateStairsDown());
            }

            // Set stairs up
            for (var z = 1; z < DEPTH - 1; z++)
            {
                var room = GetEmptyRoom(z);
                room.Items.Add(Item.CreateStairsUp());
            }

            for(var z = 0; z < DEPTH; z++)
            {
                var room = GetEmptyRoom(z);

                // Gold!
                room.Items.Add(Item.CreateGold(10 + z * 10));

                // Food
                room = GetEmptyRoom(z);
                if (random.Next(100) < 70 - z / 2)
                    room.Items.Add(Item.CreateFood(30));

                // Torch
                room = GetEmptyRoom(z);
                if (random.Next(100) < 70 - z / 2)
                    room.Items.Add(Item.CreateTorch(50));

                // Sink room
                room = GetEmptyRoom(z);
                if (random.Next(100) < 20 + z)
                    room.Items.Add(Item.CreateSinkRoom());

                // Warp room
                room = GetEmptyRoom(z);
                if (random.Next(100) < 10 + z / 2)
                    room.Items.Add(Item.CreateWarpRoom());

                // Scrolls
                room = GetEmptyRoom(z);
                if (random.Next(100) < 10 + z / 2)
                    room.Items.Add(Item.CreateScroll());

                room = GetEmptyRoom(z);
                if (random.Next(100) < 40)
                {
                    var item = Item.CreateDagger(weaponManager);
                    if (item != null)
                        room.Items.Add(item);
                }

                room = GetEmptyRoom(z);
                if (random.Next(100) < 30)
                {
                    var item = Item.CreateSword(weaponManager);
                    if (item != null)
                        room.Items.Add(item);
                }

                room = GetEmptyRoom(z);
                CreateMonster(room, z);
            }

            state = CommandState.Playing;

            hub.Subscribe<StatusMessage>(OnStatusMessage);
        }

        [JsonIgnore]
        public CommandState State { get => state; set { state = value; } }

        [JsonIgnore]
        public string Status => status;

        public Room[,,] Rooms => rooms;

        [JsonIgnore]
        public Actor? Vendor => vendor;

        public void Save(string filename)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filename, json);
        }

        public static Castle? Load(string filename)
        {
            var castle = JsonSerializer.Deserialize<Castle>(filename);
            return castle;
        }

        public Room GetRoom(int x, int y, int z)
        {
            return rooms[x, y, z];
        }

        public Room GetRoom(Actor actor)
        {
            return rooms[actor.X, actor.Y, actor.Z];
        }

        public Content GetRoomContents(int x, int y, int z)
        {
            var room = GetRoom(x, y, z);
            if (room.IsEmpty)
                return Content.Empty;

            return room.Items[0].Contents;
        }

        public Actor? GetRoomMonster(int x, int y, int z)
        {
            var room = GetRoom(x, y, z);
            if (!room.Monsters.Any())
                return null;

            return room.Monsters[0];
        }

        public bool GetVisible(int x, int y, int z)
        {
            var room = GetRoom(x, y, z);
            return room.Visited;
        }

        public bool GetTripped(int x, int y, int z)
        {
            var room = GetRoom(x, y, z);
            return room.Tripped;
        }

        private void CreateMonster(Room room, int z)
        {
            if (z == DEPTH - 1)
            {
                var monster = new Actor(ActorType.WanderingWizard);
                monsters.Add(monster);
                room.Monsters.Add(monster);
            }

            foreach(var monsterTemplate in monsterManager.Monsters)
            {
                if (monsterTemplate.Lowest >= z && monsterTemplate.Highest <= z)
                {
                    if (random.Next(100) < 30)
                    {
                        var type = monsterManager.GetActorType(monsterTemplate.Name);
                        if (type == ActorType.Unknown)
                            continue;

                        Actor monster;
                        if (type == ActorType.Vendor)
                        {
                            monster = new Vendor();
                            LoadUpVendor(monster, z);
                        }
                        else
                            monster = new Actor(type);

                        room.Monsters.Add(monster);
                        break;
                    }
                }
            }
        }

        private void LoadUpVendor(Actor monster, int z)
        {
            var shopping = new Dictionary<Item, int>();

            for (int i = 0; i < 3; i++)
            {
                var item = Item.CreateDagger(weaponManager);
                if (item != null)
                {
                    shopping[item] = 10;
                    monster.Items.Add(item);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                var item = Item.CreateSword(weaponManager);
                if (item != null)
                {
                    shopping[item] = 30;
                    monster.Items.Add(item);
                }
            }

            
        }

        private void ExecuteOnAllRooms(int z, Action<Room> execute)
        {
            for(var y = 0; y < HEIGHT; y++)
                for(var x = 0; x < WIDTH; x++)
                {
                    var room = GetRoom(x, y, z);
                    execute(room);
                }
        }

        public void Light(int z)
        {
            ExecuteOnAllRooms(z, room => room.Visited = true);
        }

        public void Dark(int z)
        {
            ExecuteOnAllRooms(z, room => room.Visited = false);
        }

        public void Execute(Actor player)
        {
            var room = GetRoom(player);
            if (room.IsEmpty)
                return;

            var item = room.Items[0];
            var contents = item.Contents;

            switch (contents)
            {
                case Content.StairsUp:
                    player.Z = player.Z - 1;
                    player.Energy = player.Energy - 3;
                    break;

                case Content.StairsDown:
                    player.Z = player.Z + 1;
                    player.Energy = player.Energy - 2;
                    break;
            }
        }

        public (int cx, int cy) Clamp(int x, int y)
        {
            if (x < 0) x = WIDTH - 1;
            if (x >= WIDTH) x = 0;

            if (y < 0) y = HEIGHT - 1;
            if (y >= HEIGHT) y = 0;

            return (x, y);
        }

        public Actor? Check(Actor actor, int dx, int dy)
        {
            var x = actor.X + dx;
            var y = actor.Y + dy;

            (x, y) = Clamp(x, y);

            var monster = GetRoomMonster(x, y, actor.Z);
            return monster;
        }

        public bool MoveOrAttack(Actor player, int dx, int dy)
        {
            var monster = Check(player, dx, dy);
            if (monster == null)
            {
                player.Move(dx, dy);
                return true;
            }

            if (monster.Dead)
            {
                player.Move(dx, dy);
                return true;
            }

            if (monster.ActorType == ActorType.Vendor)
            {
                monster.Awake = true;
                state = CommandState.Vendor;
                vendor = monster;
            }
            else
            {
                monster.Awake = true;
                Arena.Battle(messengerHub, player, monster);
            }

            return false;
        }

        public void Update(Time elapsed, Actor player)
        {
            elapsedTime = elapsed;

            player.Update(elapsedTime);

            foreach(var monster in monsters)
            {
                if (monster.Dead)
                    monster.Energy = 0;
            }

            var statusDur = elapsedTime - statusWhen;
            if (statusDur.AsSeconds() > 3)
                status = "Ready";

            var room = GetRoom(player.X, player.Y, player.Z);
            room.Visited = true;

            if (player.Lighting > 0)
            {
                for (var y = player.Y - 1; y < player.Y + 2; y++)
                    for (var x = player.X - 1; x < player.X + 2; x++)
                        DoAction(x, y, player.Z, r => r.Visited = true);
            }

            if (room.IsEmpty)
                return;

            var list = new List<Item>();

            foreach (var item in room.Items)
            {
                switch (item.Contents)
                {
                    case Content.Food:
                    case Content.Gold:
                    case Content.Torch:
                    case Content.Dagger:
                    case Content.Sword:
                    case Content.Scroll:
                        PickupItem(room, player);
                        list.Add(item);
                        break;

                    case Content.Sink:
                        room.Tripped = true;
                        player.Z = player.Z + 1;
                        player.Energy = player.Energy - 20;
                        messengerHub.Publish<StatusMessage>(new StatusMessage(this, "Player fell down a sink hole!"));
                        break;

                    case Content.Warp:
                        room.Tripped = true;
                        player.Z = player.Z + (int)random.Next(40);
                        if (player.Z > DEPTH)
                            player.Z = DEPTH - 1;
                        player.Energy = player.Energy - 40;
                        messengerHub.Publish<StatusMessage>(new StatusMessage(this, "Player stepped into a warp!"));
                        break;
                }
            }

            foreach (var item in list)
                room.Items.Remove(item);
        }

        private void DoAction(int x, int y, int z, Action<Room> action)
        {
            var cx = x;
            var cy = y;

            if (cx < 0) cx = WIDTH - 1;
            if (cx >= WIDTH) cx = 0;

            if (cy < 0) cy = HEIGHT - 1;
            if (cy >= HEIGHT) cy = 0;

            var room = rooms[cx, cy, z];

            action(room);
        }

        private Room GetEmptyRoom(int z)
        {
            while(true)
            {
                var x = random.Next(WIDTH);
                var y = random.Next(HEIGHT);

                var room = rooms[x, y, z];
                if (room.IsEmpty)
                {
                    return room;
                }
            }
        }

        private void PickupItem(Room room, Actor actor)
        {
            foreach(var item in room.Items)
            {
                switch(item.Contents)
                {
                    case Content.Food:
                        actor.Energy += (int)item.Value;
                        messengerHub.Publish(new StatusMessage(this, "Player picked up some food and ate it"));
                        break;

                    case Content.Gold:
                        actor.Gold = actor.Gold + (int)item.Value;
                        messengerHub.Publish(new StatusMessage(this, $"Player picked up {item.Value} gold pieces"));
                        break;

                    case Content.Torch:
                        actor.Items.Add(item);
                        messengerHub.Publish(new StatusMessage(this, "Player picked up a torch"));
                        break;

                    case Content.Dagger:
                    case Content.Sword:
                        actor.Items.Add(item);
                        messengerHub.Publish(new StatusMessage(this, "Player picked up a weapon"));
                        break;

                    case Content.Scroll:
                        actor.Items.Add(item);
                        messengerHub.Publish(new StatusMessage(this, "Player picked up a scroll"));
                        break;
                }
            }
        }

        private void OnStatusMessage(StatusMessage message)
        {
            status = message.Status;
            statusWhen = elapsedTime;
        }
    }
}
