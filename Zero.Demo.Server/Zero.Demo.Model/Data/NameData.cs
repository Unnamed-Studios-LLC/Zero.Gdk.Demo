using System;
using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 10)]
    public unsafe struct NameData
    {
        public const int NameLength = 10;

        [FieldOffset(0)]
        public fixed byte Name[NameLength];

        public NameData(Span<byte> name)
        {
            fixed (byte* ptr = name)
            {
                for (int i = 0; i < NameLength; i++)
                {
                    Name[i] = (byte)(i < name.Length ? ptr[i] : 0);
                }
            }
        }
    }
}
