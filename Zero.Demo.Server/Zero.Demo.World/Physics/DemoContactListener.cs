using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using System;
using Zero.Demo.Model;
using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Demo.World.Systems;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Physics
{
    public static class EntityTypeExtensions
    {
        public static EntityType GetEntityType(this Fixture fixture) => (EntityType)fixture.UserData;
        public static EntityType GetEntityType(this Body body) => (EntityType)body.UserData;
    }

    public class DemoContactListener : ContactListener
    {
        private readonly PhysicsSystem _physicsSystem;

        public DemoContactListener(PhysicsSystem physicsSystem)
        {
            _physicsSystem = physicsSystem;
        }

        public override void BeginContact(in Contact contact)
        {
            var entityA = contact.FixtureA.GetEntityType();
            var entityB = contact.FixtureB.GetEntityType();
            if (CheckForWood(entityA, entityB, contact))
            {
                return;
            }

            /*
            float relSpeed;
            Vec2 midCoordinates;
            if (entityA == EntityType.Ship)
            {
                relSpeed = GetRelativeSpeed(contact.FixtureA.Body, contact.FixtureB.Body, out midCoordinates);
                CollisionDamage(contact.FixtureA.Body, relSpeed, midCoordinates);
            }

            if (entityB == EntityType.Ship)
            {
                relSpeed = GetRelativeSpeed(contact.FixtureA.Body, contact.FixtureB.Body, out midCoordinates);
                CollisionDamage(contact.FixtureB.Body, relSpeed, midCoordinates);
            }
            */
        }

        private bool CheckForWood(EntityType entityA, EntityType entityB, Contact contact)
        {
            if (entityA != EntityType.Ship && entityA != EntityType.Wood ||
                entityB != EntityType.Ship && entityB != EntityType.Wood ||
                entityA == entityB)
            {
                return false;
            }

            // ship to wood collision
            var (shipBody, woodBody) = entityA == EntityType.Ship ? (contact.FixtureA.Body, contact.FixtureB.Body) : (contact.FixtureB.Body, contact.FixtureA.Body);
            var (shipEntityId, woodEntityId) = (_physicsSystem.GetEntity(shipBody), _physicsSystem.GetEntity(woodBody));

            if (shipEntityId == 0 ||
                woodEntityId == 0)
            {
                return true;
            }

            ref var woodResource = ref _physicsSystem.Entities.TryGetComponent<ResourceComponent>(woodEntityId, out var foundComponent);
            if (!foundComponent ||
                woodResource.Wood == 0 ||
                (Time.TotalF - woodResource.SpawnTime) < 0.5f)
            {
                return true;
            }

            ref var shipResource = ref _physicsSystem.Entities.TryGetComponent<ResourceComponent>(shipEntityId, out foundComponent);
            if (!foundComponent)
            {
                return true;
            }

            shipResource.Give(woodResource.TakeAll());
            _physicsSystem.Entities.PushEvent(shipEntityId, new EventData(EventType.Pickup));
            contact.Enabled = false;
            return true;
        }

        public override void EndContact(in Contact contact)
        {

        }

        public override void PostSolve(in Contact contact, in ContactImpulse impulse)
        {
            var entityA = contact.FixtureA.GetEntityType();
            var entityB = contact.FixtureB.GetEntityType();

            if (entityA == EntityType.Ship)
            {
                CollisionDamage(contact.FixtureA.Body, impulse.normalImpulses[0]);
            }

            if (entityB == EntityType.Ship)
            {
                CollisionDamage(contact.FixtureB.Body, impulse.normalImpulses[0]);
            }
            
        }

        public override void PreSolve(in Contact contact, in Manifold oldManifold)
        {
            var entityA = contact.FixtureA.GetEntityType();
            var entityB = contact.FixtureB.GetEntityType();
            CheckForWood(entityA, entityB, contact);
        }

        private void CollisionDamage(Body body, float impulse)
        {
            var entityId = _physicsSystem.GetEntity(body);
            if (entityId == 0)
            {
                return;
            }

            var toTake = (int)(impulse * 0.1f);
            if (toTake < 1)
            {
                return;
            }

            ref var resource = ref _physicsSystem.Entities.TryGetComponent<ResourceComponent>(entityId, out var foundComponent);
            if (!foundComponent)
            {
                return;
            }

            _physicsSystem.Entities.PushEvent(entityId, new HitData());
            resource.Take(toTake);

            var coordinates = body.GetPosition().ToVec2();
            for (int i = 0; i < toTake; i++)
            {
                if (Game.Server.Random.IntRange(0, 4) == 0)
                {
                    var spawnCoords = coordinates + new Vec2(Game.Server.Random.Float01(), Game.Server.Random.Float01()) - 0.5f;
                    _physicsSystem.Commands.Add(() => _physicsSystem.Entities.CreateDroppedWood((byte)Game.Server.Random.IntRange(0, 4), spawnCoords, 1));
                }
            }
        }
    }
}
