using Zero.Demo.Model;
using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public class ResourceSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            // upgrade, filter on components that we might be
            Entities.With<ShipComponent, PositionComponent, ShootComponent, HeadingComponent>().ForEach((uint entityId, ref ResourceComponent wood, ref ShipComponent ship) =>
            {
                if (wood.Wood != wood.MaxWood ||
                    ship.ShipType >= ShipType.Count - 1)
                {
                    return;
                }

                World.UpgradeShip(entityId);
            });

            // remove 0 resource entities
            Entities.ForEach((uint entityId, ref ResourceComponent wood) =>
            {
                if (wood.Wood == 0)
                {
                    if (Entities.HasComponent<ShipComponent>(entityId))
                    {
                        ref var ship = ref Entities.GetComponent<ShipComponent>(entityId);
                        if (!ship.Dead)
                        {
                            Entities.PushEvent(entityId, new EventData(EventType.Death));
                            ship.Dead = true;
                        }
                        else
                        {
                            var shipDef = Library.GetShip(ship.ShipType);
                            var coordinates = Entities.GetComponent<PositionComponent>(entityId).Coordinates;
                            var stackCount = shipDef.DeathWood / 5;
                            for (int i = 0; i < stackCount; i++)
                            {
                                Commands.Add(() => Entities.CreateDroppedWood(4, coordinates + new Vec2(Random.Float01(), Random.Float01()), 5));
                            }

                            var remainder = shipDef.DeathWood % 5;
                            for (int i = 0; i < remainder; i++)
                            {
                                Commands.Add(() => Entities.CreateDroppedWood((byte)Random.IntRange(0, 4), coordinates + new Vec2(Random.Float01(), Random.Float01()), 1));
                            }
                            Commands.Add(() => Entities.DestroyEntity(entityId));
                        }
                    }
                    else
                    {
                        Commands.Add(() => Entities.DestroyEntity(entityId));
                    }
                    return;
                }

                if (wood.SentWood == wood.Wood)
                {
                    return;
                }

                wood.SentWood = wood.Wood;
                Entities.PushPersistent(entityId, new ResourceData(wood.Wood));
            });
        }
    }
}
