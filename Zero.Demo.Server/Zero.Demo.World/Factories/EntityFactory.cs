using System;
using Zero.Demo.Model;
using Zero.Demo.Model.Data;
using Zero.Demo.Model.Types;
using Zero.Demo.World.Component;
using Zero.Demo.World.Definitions;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World
{
    public static class EntityFactory
    {
        private static readonly EntityLayout _playerLayout = new();
        private static readonly EntityLayout _rockLayout = new();
        private static readonly EntityLayout _shipLayout = new();
        private static readonly EntityLayout _droppedWoodLayout = new();
        private static readonly EntityLayout _spawnedWoodLayout = new();

        public static uint CreateCannonball(this Entities entities, Vec2 target, float duration, int damage)
        {
            var entityId = entities.CreateEntity();
            entities.AddComponent(entityId, new CannonballComponent(target, Time.TotalF + duration, damage));
            return entityId;
        }

        public static uint CreateDroppedWood(this Entities entities, byte index, Vec2 coordinates, int wood)
        {
            var entityId = entities.CreateWood(_droppedWoodLayout, index, coordinates, wood);
            entities.ApplyLayout(entityId, _droppedWoodLayout);
            return entityId;
        }

        public static unsafe uint CreatePlayer(this Entities entities, Span<byte> name, IconType iconType, CharacterData characterData, FlagData flagData, Vec2 coordinates)
        {
            var entityId = entities.CreateShip(_playerLayout, ShipType.Dinghy, characterData, flagData, coordinates);
            _playerLayout.Define<PlayerTag>();
            _playerLayout.Define(new IconComponent(iconType));
            entities.ApplyLayout(entityId, _playerLayout);
            entities.PushPersistent(entityId, new NameData(name));
            entities.PushPersistent(entityId, new IconData(iconType));
            return entityId;
        }

        public static uint CreateRock(this Entities entities, byte index, Vec2 coordinates)
        {
            var physicsDef = Library.GetRock(index);
            var entityId = entities.CreatePhysicsBody(_rockLayout, EntityType.Rock, coordinates, physicsDef);
            entities.PushPersistent(entityId, new RockData(index));
            entities.ApplyLayout(entityId, _rockLayout);
            return entityId;
        }

        public static uint CreateShip(this Entities entities, ShipType type, CharacterData characterData, FlagData flagData, Vec2 coordinates)
        {
            var entityId = entities.CreateShip(_shipLayout, type, characterData, flagData, coordinates);
            entities.ApplyLayout(entityId, _shipLayout);
            return entityId;
        }

        public static uint CreateSpawnedWood(this Entities entities, byte index, Vec2 coordinates, int wood)
        {
            var entityId = entities.CreateWood(_spawnedWoodLayout, index, coordinates, wood);
            _spawnedWoodLayout.Define<SpawnedTag>();
            entities.ApplyLayout(entityId, _spawnedWoodLayout);
            return entityId;
        }

        private static uint CreatePhysicsBody(this Entities entities, EntityLayout layout, EntityType type, Vec2 coordinates, PhysicsDef physicsDef)
        {
            coordinates -= new Vec2(0, physicsDef.Size.Y / 2);
            var entityId = entities.CreateTyped(layout, type, coordinates, coordinates - new Vec2(0, physicsDef.Size.Y / 2));
            layout.Define(new PhysicsComponent(physicsDef)); // body id will be set by the physics system
            if (physicsDef.Density != 0)
            {
                layout.Define(new PositionDataComponent(coordinates, Vec2.Zero)); // add position data component, since this position can change
            }
            return entityId;
        }

        private static uint CreateShip(this Entities entities, EntityLayout layout, ShipType type, CharacterData characterData, FlagData flagData, Vec2 coordinates)
        {
            var shipDef = Library.GetShip(type);
            var entityId = entities.CreatePhysicsBody(layout, EntityType.Ship, coordinates, shipDef.PhysicsDef);
            entities.PushPersistent(entityId, new ShipData(type));
            entities.PushPersistent(entityId, characterData);
            entities.PushPersistent(entityId, flagData);
            layout.Define(new ShipComponent(type));
            layout.Define(new HeadingComponent(shipDef.Speed));
            layout.Define(new ResourceComponent(shipDef.MaxWood / 2, shipDef.MaxWood, Time.TotalF));
            layout.Define(new ShootComponent());
            layout.Define(new AimComponent());
            layout.Define(new HittableTag());
            return entityId;
        }

        private static uint CreateTyped(this Entities entities, EntityLayout layout, EntityType type, Vec2 coordinates, Vec2 dataCoordinates)
        {
            var entityId = entities.CreateEntity();
            entities.PushPersistent(entityId, new EntityTypeData(type));
            entities.PushPersistent(entityId, new PositionData(dataCoordinates, Vec2.Zero));
            layout.Define(new PositionComponent(coordinates, default));
            layout.Define(new SpatialComponent(coordinates));
            return entityId;
        }

        private static uint CreateWood(this Entities entities, EntityLayout layout, byte index, Vec2 coordinates, int wood)
        {
            var physicsDef = Library.GetWood(index);
            var entityId = entities.CreatePhysicsBody(layout, EntityType.Wood, coordinates, physicsDef);
            entities.PushPersistent(entityId, new WoodData(index));
            layout.Define(new ResourceComponent(wood, wood, Time.TotalF));
            return entityId;
        }
    }
}
