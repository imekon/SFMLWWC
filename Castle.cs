namespace SFMLWWC
{
    internal class Castle
    {
        public const int WIDTH = 8;
        public const int HEIGHT = 8;
        public const int DEPTH = 50;
        private Random random;
        private Room[,,] rooms;

        public Castle()
        {
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
                room.Contents = Content.StairsDown;
            }

            // Set stairs up
            for (var z = 1; z < DEPTH - 1; z++)
            {
                var room = GetEmptyRoom(z);
                room.Contents = Content.StairsUp;
            }

            // Gold!
            for(var z = 0; z < DEPTH; z++)
            {
                var room = GetEmptyRoom(z);
                room.Contents = Content.Gold;
                room.Item = new Item(ItemType.Gold, 100);
            }
        }

        public Room GetRoom(int x, int y, int z)
        {
            return rooms[x, y, z];
        }

        public Content GetRoomContents(int x, int y, int z)
        {
            var room = GetRoom(x, y, z);
            return room.Contents;
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

        public void Update(Actor player)
        {
            var room = GetRoom(player.X, player.Y, player.Z);
            room.Visited = true;

            switch(room.Contents)
            {
                case Content.Gold:
                    PickupItem(room, player);
                    break;
            }
        }

        private Room GetEmptyRoom(int z)
        {
            while(true)
            {
                var x = random.NextInt64(WIDTH);
                var y = random.NextInt64(HEIGHT);

                var room = rooms[x, y, z];
                if (room.Contents == Content.Empty)
                {
                    return room;
                }
            }
        }

        private void PickupItem(Room room, Actor actor)
        {
            var item = room.Item;
            if (item == null)
                return;

            room.Contents = Content.Empty;
            room.Item = null;

            switch(item.ItemType)
            {
                case ItemType.Gold:
                    actor.Gold = actor.Gold + (int)item.Value;
                    break;
            }
        }
    }
}
