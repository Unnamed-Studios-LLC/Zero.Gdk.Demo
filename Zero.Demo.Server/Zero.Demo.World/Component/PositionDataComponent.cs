using Zero.Game.Shared;

namespace Zero.Demo.World.Component
{
    public struct PositionDataComponent
    {
        public Vec2 SentCoordinates;
        public Vec2 SentTrajectory;
        public int Cooldown;

        public PositionDataComponent(Vec2 sentCoordinates, Vec2 sentTrajectory)
        {
            SentCoordinates = sentCoordinates;
            SentTrajectory = sentTrajectory;
            Cooldown = 0;
        }
    }
}
