using Zero.Game.Shared;

namespace Zero.Demo.World.Component
{
    public struct SpatialComponent
    {
        public Vec2 Coordinates;

        public SpatialComponent(Vec2 coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
