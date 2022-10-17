using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = LeaderboardNamesData.Count * sizeof(int))]
    public unsafe struct LeaderboardValuesData
    {
        [FieldOffset(0)]
        public fixed int Values[LeaderboardNamesData.Count];
    }
}
