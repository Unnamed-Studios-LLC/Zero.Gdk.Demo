using Zero.Game.Shared;

namespace Zero.Demo.World.Component
{
    public struct PositionComponent
    {
        public Vec2 Coordinates;
        public Vec2 Trajectory;

        public PositionComponent(Vec2 coordinates, Vec2 trajectory)
        {
            Coordinates = coordinates;
            Trajectory = trajectory;
        }
    }
}
