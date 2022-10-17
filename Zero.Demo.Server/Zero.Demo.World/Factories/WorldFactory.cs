using System;
using System.Collections.Generic;
using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Demo.World.Global;
using Zero.Demo.World.Systems;
using Zero.Game.Shared;

namespace Zero.Demo.World
{
    public static class WorldFactory
    {
        public static bool CreateWorld(Game.Server.World world)
        {
            world.Parallel = true; // mark this world to run updates in parallel to other worlds marked parallel
            world.MaxConnections = App.Settings.WorldMaxConnections;

            if (world.Data.TryGetValue("permanent", out var perm) &&
                !string.IsNullOrWhiteSpace(perm) &&
                perm.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                world.AddSystem(new CapacityLogSystem());
            }

            var mapSize = new Int2(150, 150);

            world.AddSystem(new PhysicsSystem(mapSize));
            world.AddSystem(new SpatialSystem(mapSize, 20));
            world.AddSystem(new PositionDataSystem());
            world.AddSystem(new CannonballSystem());
            world.AddSystem(new ShootSystem());
            world.AddSystem(new ResourceSystem());
            world.AddSystem(new WoodSpawnSystem(mapSize));
            world.AddSystem(new PlayerSpawnSystem(mapSize, AddRocks(world, mapSize, 115, 5)));
            world.AddSystem(new LeaderboardSystem());

            world.Entities.AddComponent(world.EntityId, new LeaderboardComponent());

            if (world.Data.TryGetValue("worldKey", out var worldKey) &&
                !string.IsNullOrWhiteSpace(worldKey))
            {
                world.Entities.PushPersistent(world.EntityId, new WorldKeyData(worldKey.AsSpan()));
            }

            return true;
        }

        private static HashSet<Int2> AddRocks(Game.Server.World world, Int2 mapSize, int rockCount, int islandCount)
        {
            var taken = new HashSet<Int2>();
            // add rocks
            for (int i = 0; i < rockCount; i++)
            {
                Int2 coordinates;
                do
                {
                    coordinates = new Int2(Game.Server.Random.IntRange(0, mapSize.X), Game.Server.Random.IntRange(0, mapSize.Y));
                }
                while (!taken.Add(coordinates));

                for (int y = -2; y <= 2; y++)
                {
                    for (int x = -2; x <= 2; x++)
                    {
                        taken.Add(coordinates + new Int2(x, y));
                    }
                }

                var rockIndex = (byte)Game.Server.Random.IntRange(0, 4);
                world.Entities.CreateRock(rockIndex, (Vec2)coordinates + 0.5f);
            }

            for (int i = 0; i < islandCount; i++)
            {
                Int2 coordinates;
                do
                {
                    coordinates = new Int2(Game.Server.Random.IntRange(6, mapSize.X - 12), Game.Server.Random.IntRange(6, mapSize.Y - 12));
                    var invalid = false;
                    for (int y = -8; y <= 8 && !invalid; y++)
                    {
                        for (int x = -8; x <= 8 && !invalid; x++)
                        {
                            if (taken.Contains(coordinates + new Int2(x, y)))
                            {
                                invalid = true;
                            }
                        }
                    }

                    if (invalid)
                    {
                        continue;
                    }
                }
                while (!taken.Add(coordinates));

                for (int y = -8; y <= 8; y++)
                {
                    for (int x = -8; x <= 8; x++)
                    {
                        taken.Add(coordinates + new Int2(x, y));
                    }
                }

                var islandIndex = (byte)Game.Server.Random.IntRange(4, 6);
                world.Entities.CreateRock(islandIndex, (Vec2)coordinates + 0.5f);
            }
            return taken;
        }
    }
}