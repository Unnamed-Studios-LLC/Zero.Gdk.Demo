using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public readonly struct CharacterData
    {
        [FieldOffset(0)]
        public readonly byte HatIndex;
        [FieldOffset(1)]
        public readonly byte HeadIndex;
        [FieldOffset(2)]
        public readonly byte LegsIndex;

        public CharacterData(byte hatIndex, byte headIndex, byte legsIndex)
        {
            HatIndex = hatIndex;
            HeadIndex = headIndex;
            LegsIndex = legsIndex;
        }
    }
}
