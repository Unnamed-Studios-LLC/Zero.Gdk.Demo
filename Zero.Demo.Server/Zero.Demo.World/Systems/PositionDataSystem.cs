using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public class PositionDataSystem : ComponentSystem
    {
        private const int PositionPushCooldown = 200;

        protected override void OnUpdate()
        {
            Entities.ForEach((uint entityId, ref PositionComponent position, ref PositionDataComponent positionData, ref PhysicsComponent physics) =>
            {
                positionData.Cooldown += Time.Delta;
                if (positionData.Cooldown < PositionPushCooldown)
                {
                    return;
                }

                positionData.Cooldown = 0;

                if (position.Coordinates == positionData.SentCoordinates &&
                    position.Trajectory == positionData.SentTrajectory)
                {
                    return;
                }

                positionData.SentCoordinates = position.Coordinates;
                positionData.SentTrajectory = position.Trajectory;

                Entities.PushPersistent(entityId, new PositionData(position.Coordinates - new Vec2(0, physics.Def.Size.Y / 2), position.Trajectory));
            });
        }
    }
}
