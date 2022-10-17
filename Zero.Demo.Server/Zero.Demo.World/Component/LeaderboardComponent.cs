using Zero.Demo.Model.Data;

namespace Zero.Demo.World.Component
{
    public unsafe struct LeaderboardComponent
    {
        public const int Count = LeaderboardNamesData.Count;

        public fixed uint EntityIds[Count];
        public fixed int Values[Count];
    }
}
