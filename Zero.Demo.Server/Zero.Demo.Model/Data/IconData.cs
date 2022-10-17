using System.Runtime.InteropServices;
using Zero.Demo.Model.Types;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public readonly struct IconData
    {
        [FieldOffset(0)]
        public readonly byte _iconType;

        public IconData(IconType iconType)
        {
            _iconType = (byte)iconType;
        }

        public IconType IconType => (IconType)_iconType;
    }
}
