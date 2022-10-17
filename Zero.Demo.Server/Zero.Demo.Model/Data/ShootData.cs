using System.Runtime.InteropServices;
using Zero.Game.Shared;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public readonly struct ShootData
    {
        [FieldOffset(0)]
        public readonly float Duration;
        [FieldOffset(4)]
        public readonly Vec2 Target;

        public ShootData(float duration, Vec2 target)
        {
            Duration = duration;
            Target = target;
        }
    }
}
