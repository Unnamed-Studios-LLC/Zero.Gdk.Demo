using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public readonly struct ShipData
    {
        [FieldOffset(0)]
        private readonly byte _shipType;

        public ShipData(ShipType shipType)
        {
            _shipType = (byte)shipType;
        }

        public ShipType ShipType => (ShipType)_shipType;
    }
}
