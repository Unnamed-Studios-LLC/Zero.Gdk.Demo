using System.Runtime.InteropServices;
using Zero.Game.Shared;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 9)]
    public readonly struct AimInputData
    {
        [FieldOffset(0)]
        public readonly Vec2 Coordinates;
        [FieldOffset(8)]
        public readonly bool Aiming;

        public AimInputData(Vec2 coordinates, bool active)
        {
            Coordinates = coordinates;
            Aiming = active;
        }
    }
}
