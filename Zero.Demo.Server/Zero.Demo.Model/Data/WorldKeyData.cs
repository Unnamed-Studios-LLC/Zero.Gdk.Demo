using System;
using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public unsafe struct WorldKeyData
    {
        [FieldOffset(0)]
        public fixed byte Key[4];

        public WorldKeyData(ReadOnlySpan<char> key)
        {
            for (int i = 0; i < key.Length && i < 4; i++)
            {
                Key[i] = (byte)key[i];
            }
        }
    }
}
