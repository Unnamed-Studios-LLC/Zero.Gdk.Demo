using Zero.Demo.Model;
using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Demo.World.Systems;

namespace Zero.Demo.World
{
    public static class EntitiesExtensions
    {
        public static void UpgradeShip(this Game.Server.World world, uint entityId)
        {
            ref var ship = ref world.Entities.GetComponent<ShipComponent>(entityId);
            if (ship.ShipType >= ShipType.Count - 1)
            {
                return;
            }

            var physicsSystem = world.GetSystem<PhysicsSystem>();
            if (physicsSystem == null)
            {
                return;
            }

            ref var position = ref world.Entities.GetComponent<PositionComponent>(entityId);
            ref var shoot = ref world.Entities.GetComponent<ShootComponent>(entityId);
            ref var heading = ref world.Entities.GetComponent<HeadingComponent>(entityId);
            ref var resource = ref world.Entities.GetComponent<ResourceComponent>(entityId);

            var shipDef = Library.GetShip(++ship.ShipType);
            var velocity = physicsSystem.GetBody(entityId).GetLinearVelocity();
            physicsSystem.CreateBody(entityId, shipDef.PhysicsDef, position.Coordinates);
            physicsSystem.GetBody(entityId).SetLinearVelocity(in velocity);
            resource.MaxWood = shipDef.MaxWood;
            heading.Speed = shipDef.Speed;
            world.Entities.PushPersistent(entityId, new ShipData(ship.ShipType));
            world.Entities.PushEvent(entityId, new EventData(EventType.Upgrade));
        }
    }
}
