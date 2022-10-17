using System.Runtime.InteropServices;
namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct ResourceData
    {
        [FieldOffset(0)]
        public readonly int Amount;

        public ResourceData(int amount)
        {
            Amount = amount;
        }
    }
}
