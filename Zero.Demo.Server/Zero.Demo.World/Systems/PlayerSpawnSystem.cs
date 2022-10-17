using System;
using System.Collections.Generic;
using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public unsafe class PlayerSpawnSystem : ComponentSystem
    {
        private readonly Int2 _mapSize;
        private readonly HashSet<Int2> _rocks = new();

        public PlayerSpawnSystem(Int2 mapSize, HashSet<Int2> rocks)
        {
            _mapSize = mapSize;
            _rocks = rocks;
        }

        public Int2 GetSpawnCoordinates()
        {
            Int2 coordinates;
            do
            {
                coordinates = new Int2(Game.Server.Random.IntRange(0, _mapSize.X), Game.Server.Random.IntRange(0, _mapSize.Y));
            }
            while (_rocks.Contains(coordinates));
            return coordinates;
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((uint entityId, ref PlayerComponent player) =>
            {
                if (player.EntityId != 0)
                {
                    return;
                }

                player.RespawnCooldown -= Time.DeltaF;
                if (player.RespawnCooldown > 0)
                {
                    return;
                }

                // spawn
                Commands.Add(() =>
                {
                    ref var innerPlayer = ref Entities.GetComponent<PlayerComponent>(entityId);
                    fixed (byte* name = innerPlayer.Name)
                    {
                        innerPlayer.EntityId = Entities.CreatePlayer(new Span<byte>(name, 10), innerPlayer.IconType, innerPlayer.CharacterData, innerPlayer.FlagData, GetSpawnCoordinates());
                        Entities.PushPersistent(entityId, new ControlData(innerPlayer.EntityId));
                    }
                });
            });
        }
    }
}
