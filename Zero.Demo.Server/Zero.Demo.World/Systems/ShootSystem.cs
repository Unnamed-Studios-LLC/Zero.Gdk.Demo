using System.Runtime.CompilerServices;
using Zero.Demo.Model;
using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Demo.World.Definitions;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public class ShootSystem : ComponentSystem
    {
        private const float CannonSpeed = 10;

        protected override void OnUpdate()
        {
            var delta = Time.DeltaF;
            Entities.ForEach((uint entityId, ref PositionComponent position, ref AimComponent aim, ref ShootComponent shoot, ref ShipComponent ship) =>
            {
                if (shoot.Cooldown > 0)
                {
                    shoot.Cooldown -= delta;
                }

                if (aim.Aiming &&
                    shoot.Cooldown <= 0)
                {
                    shoot.Cooldown = Shoot(entityId, ship.ShipType, position.Coordinates, aim.AimCoordinates);
                }
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vec2 GetClippedAimCoordinates(ShipDef shipDef, Vec2 coordinates, Vec2 aimCoordinates)
        {
            var vector = aimCoordinates - coordinates;
            if (vector.SqrMagnitude <= shipDef.MaxRangeSqr)
            {
                return aimCoordinates;
            }
            return coordinates + vector.SetMagnitude(shipDef.MaxRange);
        }

        private float Shoot(uint entityId, ShipType shipType, Vec2 coordinates, Vec2 aimCoordinates)
        {
            var shipDefinition = Library.GetShip(shipType);
            aimCoordinates = GetClippedAimCoordinates(shipDefinition, coordinates, aimCoordinates);
            for (int i = 0; i < shipDefinition.Cannons; i++)
            {
                var target = aimCoordinates + new Vec2(Random.Float01(), Random.Float01()) * shipDefinition.CannonSpread - shipDefinition.CannonSpread / 2;
                var duration = (coordinates - target).Magnitude / CannonSpeed;
                Commands.Add(() => Entities.CreateCannonball(target, duration, shipDefinition.Damage));
                Entities.PushEvent(entityId, new ShootData(duration, target));
            }
            return shipDefinition.FireRateCooldown;
        }
    }
}
