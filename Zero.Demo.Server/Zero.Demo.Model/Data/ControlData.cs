using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public readonly struct ControlData
    {
        [FieldOffset(0)]
        public readonly uint EntityId;

        public ControlData(uint entityId)
        {
            EntityId = entityId;
        }
    }
}
