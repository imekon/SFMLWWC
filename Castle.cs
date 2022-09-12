using SFML.System;
using TinyMessenger;

namespace SFMLWWC
{
    internal class Castle
    {
        public const int WIDTH = 8;
        public const int HEIGHT = 8;
        public const int DEPTH = 50;

        private TinyMessengerHub messengerHub;
        private string status;
        private Random random;
        private Room[,,] rooms;
        private Time elapsedTime;
        private Time statusWhen;

        public Castle(TinyMessengerHub hub)
        {
            messengerHub = hub;

            status = "Ready";

            elapsedTime = new Time();
            statusWhen = new Time();

            random = new Random();

            rooms = new Room[WIDTH, HEIGHT, DEPTH];

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
                var x = random.NextInt64(WIDTH);
                var y = random.NextInt64(HEIGHT);

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
                if (random.NextInt64(100) < 50)
                    room.Items.Add(Item.CreateFood(10));

                // Torch
                room = GetEmptyRoom(z);
                if (random.NextInt64(100) < 50)
                    room.Items.Add(Item.CreateTorch(50));

                // Sink room
                room = GetEmptyRoom(z);
                if (random.NextInt64(100) < 20 + z)
                    room.Items.Add(Item.CreateSinkRoom());

                // Warp room
                room = GetEmptyRoom(z);
                if (random.NextInt64(100) < 10 + z / 2)
                    room.Items.Add(Item.CreateWarpRoom());

                room = GetEmptyRoom(z);
                CreateMonster(room, z);
            }

            hub.Subscribe<StatusMessage>(OnStatusMessage);
        }

        public string Status => status;

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
                room.Monsters.Add(monster);
            }

            if (random.NextInt64(100) < 5)
            {
                var monster = new Actor(ActorType.Mouse);
                room.Monsters.Add(monster);
            }
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

                case Content.Food:
                    player.Energy = player.Energy + 10;
                    room.Items.Remove(item);
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

        public bool Check(Actor actor, int dx, int dy)
        {
            var x = actor.X + dx;
            var y = actor.Y + dy;

            (x, y) = Clamp(x, y);

            var monster = GetRoomMonster(x, y, actor.Z);
            return monster == null;
        }

        public void Update(Time elapsed, Actor player)
        {
            elapsedTime = elapsed;

            player.Update(elapsedTime);

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
                    case Content.Gold:
                        PickupItem(room, player);
                        list.Add(item);
                        break;

                    case Content.Torch:
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
                        player.Z = player.Z + (int)random.NextInt64(40);
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
                var x = random.NextInt64(WIDTH);
                var y = random.NextInt64(HEIGHT);

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
                    case Content.Gold:
                        actor.Gold = actor.Gold + (int)item.Value;
                        messengerHub.Publish(new StatusMessage(this, $"Player picked up {item.Value} gold pieces"));
                        break;

                    case Content.Torch:
                        actor.Items.Add(item);
                        messengerHub.Publish(new StatusMessage(this, "Player picked up a torch"));
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
