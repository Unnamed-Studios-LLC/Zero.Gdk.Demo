using Zero.Game.Shared;

namespace Zero.Demo.World.Definitions
{
    public struct PhysicsDef
    {
        public PhysicsDef(Vec2 size, float density, float friction, float restitution)
        {
            Size = size;
            Density = density;
            Friction = friction;
            Restitution = restitution;
        }

        public Vec2 Size { get; }
        public float Density { get; }
        public float Friction { get; }
        public float Restitution { get; }
    }
}
