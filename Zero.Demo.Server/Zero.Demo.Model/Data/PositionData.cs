using System.Runtime.InteropServices;
using Zero.Game.Shared;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct PositionData
    {
        [FieldOffset(0)]
        public readonly Vec2 Coordinates;
        [FieldOffset(8)]
        public readonly Vec2 Trajectory;

        public PositionData(Vec2 coordinates, Vec2 trajectory)
        {
            Coordinates = coordinates;
            Trajectory = trajectory;
        }
    }
}
