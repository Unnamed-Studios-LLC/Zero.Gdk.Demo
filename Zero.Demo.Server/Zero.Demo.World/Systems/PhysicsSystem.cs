using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using System.Collections.Generic;
using System.Numerics;
using Zero.Demo.Model;
using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Demo.World.Definitions;
using Zero.Demo.World.Physics;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public class PhysicsSystem : ComponentSystem, IAddEvent<PhysicsComponent>, IRemoveEvent<PhysicsComponent>
    {
        private const float BoundsThickness = 5;

        private readonly Box2D.NetStandard.Dynamics.World.World _box2dWorld;
        private readonly Dictionary<Body, uint> _entityMap = new();
        private readonly Dictionary<uint, Body> _bodyMap = new();

        public PhysicsSystem(Int2 mapSize)
        {
            _box2dWorld = new Box2D.NetStandard.Dynamics.World.World(new Vector2(0, 0));
            _box2dWorld.SetContactListener(new DemoContactListener(this));
            CreateBounds(mapSize);
        }

        public void CreateBody(uint entityId, PhysicsDef physicsDef, Vec2 coordinates)
        {
            DestroyBody(entityId);

            var dampening = 1f / (physicsDef.Density * 2);
            var bodyDef = new BodyDef()
            {
                position = new Vector2(coordinates.X, coordinates.Y),
                fixedRotation = true,
                type = physicsDef.Density == 0 ? BodyType.Static : BodyType.Dynamic,
                linearDamping = dampening,
                enabled = true
            };

            var body = _box2dWorld.CreateBody(bodyDef);
            var shape = new PolygonShape();
            shape.SetAsBox(physicsDef.Size.X / 2, physicsDef.Size.Y / 2);
            var fixtureDef = new FixtureDef
            {
                shape = shape,
                density = physicsDef.Density,
                friction = physicsDef.Friction,
                restitution = physicsDef.Restitution,
                userData = World.Entities.GetPersistent<EntityTypeData>(entityId).EntityType
            };

            body.CreateFixture(fixtureDef);

            _bodyMap[entityId] = body;
            _entityMap[body] = entityId;
        }

        public void DestroyBody(uint entityId)
        {
            var body = GetBody(entityId);
            if (body == null)
            {
                return;
            }
            _bodyMap.Remove(entityId);
            _entityMap.Remove(body);
            _box2dWorld.DestroyBody(body);
        }

        public Body GetBody(uint entityId)
        {
            if (_bodyMap.TryGetValue(entityId, out var body))
            {
                return body;
            }
            return null;
        }

        public Body GetBody(Vec2 coordinate)
        {
            Body body = null;
            var aabb = new AABB(coordinate.ToVector2(), coordinate.ToVector2());
            bool queryCallback(Fixture fixture)
            {
                body = fixture.Body;
                return false;
            }
            _box2dWorld.QueryAABB(queryCallback, aabb);
            return body;
        }

        public uint GetEntity(Body body)
        {
            return _entityMap.TryGetValue(body, out var entity) ? entity : default;
        }

        public uint GetEntity(Vec2 coordinate)
        {
            var body = GetBody(coordinate);
            if (body == null)
            {
                return 0;
            }
            return GetEntity(body);
        }

        public void OnAdd(uint entityId, ref PhysicsComponent component)
        {
            ref var position = ref World.Entities.GetComponent<PositionComponent>(entityId);
            CreateBody(entityId, component.Def, position.Coordinates);
        }

        public void OnRemove(uint entityId, in PhysicsComponent component)
        {
            DestroyBody(entityId);
        }

        protected override void OnUpdate()
        {
            // apply heading forces
            Entities.ForEach((uint entityId, ref HeadingComponent heading) =>
            {
                var body = GetBody(entityId);
                if (body == null)
                {
                    return;
                }

                var force = heading.Heading * heading.Speed;
                body.ApplyForceToCenter(force.ToVector2());
            });

            _box2dWorld.Step(Time.DeltaF, 8, 3); // step physics simulation

            // update position of physics components
            Entities.With<PhysicsComponent>().ForEach((uint entityId, ref PositionComponent position, ref PhysicsComponent physics) =>
            {
                var body = GetBody(entityId);
                if (body == null)
                {
                    return;
                }

                position.Coordinates = body.GetPosition().ToVec2();
                position.Trajectory = body.GetLinearVelocity().ToVec2();
            });
        }

        private void CreateBounds(Int2 mapSize)
        {
            CreateBound(new Rect(-BoundsThickness, 0, BoundsThickness, mapSize.Y)); // left
            CreateBound(new Rect(-BoundsThickness, -BoundsThickness, mapSize.X + BoundsThickness * 2, BoundsThickness)); // bottom
            CreateBound(new Rect(mapSize.X, 0, BoundsThickness, mapSize.Y)); // right
            CreateBound(new Rect(-BoundsThickness, mapSize.Y, mapSize.X + BoundsThickness * 2, BoundsThickness)); // top
        }

        private void CreateBound(Rect rect)
        {
            var bodyDef = new BodyDef
            {
                type = BodyType.Static,
                position = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2),
                userData = EntityType.Bounds
            };

            var body = _box2dWorld.CreateBody(bodyDef);
            var box = new PolygonShape(rect.Width / 2, rect.Height / 2);
            var fixtureDef = new FixtureDef
            {
                shape = box,
                userData = EntityType.Bounds
            };
            body.CreateFixture(fixtureDef);
        }
    }
}
