using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public readonly struct EntityTypeData
    {
        [FieldOffset(0)]
        private readonly byte _entityType;

        public EntityTypeData(EntityType entityType)
        {
            _entityType = (byte)entityType;
        }

        public EntityType EntityType => (EntityType)_entityType;
    }
}
