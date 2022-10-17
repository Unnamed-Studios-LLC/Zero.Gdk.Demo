using Zero.Game.Shared;

namespace Zero.Demo.World.Component
{
    public struct HeadingComponent
    {
        public Vec2 Heading;
        public float Speed;

        public HeadingComponent(float speed) : this()
        {
            Speed = speed;
            Heading = Vec2.Zero;
        }
    }
}
