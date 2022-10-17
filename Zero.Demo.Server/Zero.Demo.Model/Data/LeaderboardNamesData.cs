using System;
using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = Count + NameData.NameLength * Count)]
    public unsafe struct LeaderboardNamesData
    {
        public const int Count = 3;

        [FieldOffset(0)]
        public fixed byte Flags[Count];

        [FieldOffset(3)]
        public fixed byte Names[NameData.NameLength * Count];

        public FlagData GetFlag(int index)
        {
            return new FlagData(Flags[index]);
        }

        public void SetName(int index, void* src)
        {
            fixed (byte* dst = &Names[index * NameData.NameLength])
            {
                Buffer.MemoryCopy(src, dst, NameData.NameLength, NameData.NameLength);
            }
        }
    }
}
