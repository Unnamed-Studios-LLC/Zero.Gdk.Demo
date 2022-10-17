using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public readonly struct RockData
    {
        [FieldOffset(0)]
        public readonly byte Index;

        public RockData(byte index)
        {
            Index = index;
        }
    }
}
