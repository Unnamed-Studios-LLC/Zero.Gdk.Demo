using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public struct WoodData
    {
        [FieldOffset(0)]
        public readonly byte Index;

        public WoodData(byte index)
        {
            Index = index;
        }
    }
}
