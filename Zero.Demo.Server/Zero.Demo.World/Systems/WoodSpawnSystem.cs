using Zero.Demo.World.Component;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public class WoodSpawnSystem : ComponentSystem,
        IAddEvent<SpawnedTag>,
        IRemoveEvent<SpawnedTag>,
        IAddEvent<ShipComponent>,
        IRemoveEvent<ShipComponent>
    {
        private const float SpawnRate = 6;
        private const float SpawnRateCooldown = 1f / SpawnRate;

        private const int MinSpawnCount = 80;
        private const int SpawnCountAddPerShip = 20;

        private readonly Int2 _mapSize;
        private float _spawnCooldown;
        private int _spawnedCount;
        private int _shipCount;

        public WoodSpawnSystem(Int2 mapSize)
        {
            _mapSize = mapSize;
        }

        private int MaxSpawnCount => MinSpawnCount + _shipCount * SpawnCountAddPerShip;

        public void OnAdd(uint entityId, ref ShipComponent component)
        {
            _shipCount++;
        }

        public void OnAdd(uint entityId, ref SpawnedTag component)
        {
            _spawnedCount++;
        }

        public void OnRemove(uint entityId, in ShipComponent component)
        {
            _shipCount--;
        }

        public void OnRemove(uint entityId, in SpawnedTag component)
        {
            _spawnedCount--;
        }

        protected override void OnUpdate()
        {
            _spawnCooldown -= Time.DeltaF;
            while (_spawnCooldown <= 0)
            {
                _spawnCooldown += SpawnRateCooldown;

                if (_spawnedCount < MaxSpawnCount)
                {
                    var woodIndex = (byte)Random.IntRange(0, Library.WoodCount);
                    int amount = woodIndex switch
                    {
                        4 => 5,
                        > 4 => 3,
                        _ => 1
                    };
                    var coordinates = new Int2(Random.IntRange(0, _mapSize.X), Random.IntRange(0, _mapSize.Y));
                    Entities.CreateSpawnedWood(woodIndex, (Vec2)coordinates + 0.5f, amount);
                }
            }
        }
    }
}
