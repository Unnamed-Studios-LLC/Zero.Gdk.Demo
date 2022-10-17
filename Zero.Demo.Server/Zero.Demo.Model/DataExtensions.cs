using Zero.Demo.Model.Data;
using Zero.Game.Shared;

namespace Zero.Demo.Model
{
    public static class DataExtensions
    {
        public static T BuildDemoData<T>(this T builder)
            where T : DataBuilder
        {
            builder
                
                .Define<AimInputData>()
                .Define<CharacterData>()
                .Define<ChangeShipInputData>()
                .Define<ControlData>()
                .Define<EntityTypeData>()
                .Define<EventData>()
                .Define<FlagData>()
                .Define<HeadingInputData>()
                .Define<HitData>()
                .Define<IconData>()
                .Define<LeaderboardNamesData>()
                .Define<LeaderboardValuesData>()
                .Define<NameData>()
                .Define<PositionData>()
                .Define<ResourceData>()
                .Define<RockData>()
                .Define<ShipData>()
                .Define<ShootData>()
                .Define<WoodData>()
                .Define<WorldKeyData>()

                ;

            return builder;
        }
    }
}
