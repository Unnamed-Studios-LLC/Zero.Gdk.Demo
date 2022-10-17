using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public readonly struct FlagData
    {
        [FieldOffset(0)]
        public readonly byte Value;

        public FlagData(byte value)
        {
            Value = value;
        }

        public FlagData(int flag, int color)
        {
            Value = (byte)((flag << 3) | color);
        }

        public int Color => Value & 0x7;
        public int Flag => Value >> 3;
    }
}
