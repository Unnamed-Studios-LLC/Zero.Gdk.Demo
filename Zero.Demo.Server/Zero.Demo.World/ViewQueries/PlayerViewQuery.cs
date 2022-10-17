using System.Collections.Generic;
using System.Runtime.InteropServices;
using Zero.Demo.World.Component;
using Zero.Demo.World.Systems;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.ViewQueries
{
    public class PlayerViewQuery : ViewQuery
    {
        private const float SightWidth = 140;
        private const float SightHeight = 100;

        private SpatialSystem _spatialSystem;
        private readonly List<SpatialSystem.SpatialEntry> _spatialResults = new();
        private Vec2 _coordinates;

        protected override void OnAddEntities(Connection connection, List<uint> entityList)
        {
            ref var player = ref connection.World.Entities.GetComponent<PlayerComponent>(connection.EntityId);

            entityList.Add(connection.EntityId);
            entityList.Add(connection.World.EntityId);

            if (player.EntityId != 0 &&
                !connection.World.Entities.EntityExists(player.EntityId))
            {
                player.EntityId = 0;
                player.RespawnCooldown = 3;
            }

            if (_spatialSystem != null)
            {
                if (player.EntityId != 0)
                {
                    ref var position = ref connection.World.Entities.GetComponent<PositionComponent>(player.EntityId);
                    _coordinates = position.Coordinates;
                }

                _spatialSystem.GetEntitiesWithin(new Rect(_coordinates.X - SightWidth / 2, _coordinates.Y - SightHeight / 2, SightWidth, SightHeight), _spatialResults);

                var span = CollectionsMarshal.AsSpan(_spatialResults);
                for (int i = 0; i < span.Length; i++)
                {
                    ref var entry = ref span[i];
                    entityList.Add(entry.EntityId);
                }
                _spatialResults.Clear();
            }
        }

        protected override void OnStartWorld(Connection connection)
        {
            _spatialSystem = connection.World.GetSystem<SpatialSystem>();
        }
    }
}
