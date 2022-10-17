using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public class CannonballSystem : ComponentSystem
    {
        private PhysicsSystem _physicsSystem;

        protected override void OnStart()
        {
            _physicsSystem = World.GetSystem<PhysicsSystem>();
        }

        protected override void OnUpdate()
        {
            var time = Time.TotalF;
            Entities.ForEach((uint entityId, ref CannonballComponent cannon) =>
            {
                if (time < cannon.HitTime)
                {
                    return;
                }

                HitCheck(cannon.Target, cannon.Damage); // hitcheck
                Commands.Add(() => Entities.DestroyEntity(entityId)); // remove cannonball
            });
        }

        private void Hit(uint entityId, Vec2 coordinates, int damage)
        {
            if (!Entities.HasComponent<HittableTag>(entityId))
            {
                return;
            }

            ref var woodComponent = ref Entities.TryGetComponent<ResourceComponent>(entityId, out var found);
            if (!found)
            {
                return;
            }

            Entities.PushEvent(entityId, new HitData());
            var amount = woodComponent.Take(damage);
            if (amount <= 0 ||
                Random.IntRange(0, 4) == 0)
            {
                return;
            }

            Commands.Add(() => Entities.CreateDroppedWood((byte)Random.IntRange(0, 4), coordinates, amount));
        }

        private void HitCheck(Vec2 coordinates, int damage)
        {
            if (_physicsSystem == null)
            {
                return;
            }

            var entityId = _physicsSystem.GetEntity(coordinates);
            if (entityId == 0)
            {
                return;
            }

            Hit(entityId, coordinates, damage);
        }
    }
}
