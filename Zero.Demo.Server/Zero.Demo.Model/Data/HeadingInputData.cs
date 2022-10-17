using System.Runtime.InteropServices;
using Zero.Game.Shared;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public readonly struct HeadingInputData
    {
        [FieldOffset(0)]
        public readonly Vec2 Heading;

        public HeadingInputData(Vec2 heading)
        {
            Heading = heading;
        }
    }
}
