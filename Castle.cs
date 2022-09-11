using SFML.System;

namespace SFMLWWC
{
    internal class Castle
    {
        public const int WIDTH = 8;
        public const int HEIGHT = 8;
        public const int DEPTH = 50;

        private string status;
        private Random random;
        private Room[,,] rooms;
        private Time elapsedTime;
        private Time statusWhen;

        public Castle()
        {
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
                room.Items.Add(new Item(Content.StairsDown, 0));
            }

            // Set stairs up
            for (var z = 1; z < DEPTH - 1; z++)
            {
                var room = GetEmptyRoom(z);
                room.Items.Add(new Item(Content.StairsUp, 0));
            }

            for(var z = 0; z < DEPTH; z++)
            {
                var room = GetEmptyRoom(z);

                // Gold!
                room.Items.Add(new Item(Content.Gold, 10 + z * 10));

                // Food
                if (random.NextInt64(100) < 50)
                    room.Items.Add(new Item(Content.Food, 10));
            }
        }

        public string Status => status;

        public Room GetRoom(int x, int y, int z)
        {
            return rooms[x, y, z];
        }

        public Content GetRoomContents(int x, int y, int z)
        {
            var room = GetRoom(x, y, z);
            if (room.IsEmpty)
                return Content.Empty;

            return room.Items[0].Contents;
        }

        public Content GetRoomContents(Actor player)
        {
            return GetRoomContents(player.X, player.Y, player.Z);
        }

        public bool GetVisible(int x, int y, int z)
        {
            var room = GetRoom(x, y, z);
            return room.Visited;
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
                }
            }

            foreach (var item in list)
                room.Items.Remove(item);
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
                        SetStatus($"Player picked up {item.Value} gold pieces");
                        break;
                }
            }
        }

        private void SetStatus(string text)
        {
            status = text;
            statusWhen = elapsedTime;
        }
    }
}
