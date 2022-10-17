using Zero.Demo.Model.Data;
using Zero.Demo.World.Component;
using Zero.Demo.World.States;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Handlers
{
    public class PlayerMessageHandler : IMessageHandler,
        IDataHandler<AimInputData>,
        IDataHandler<ChangeShipInputData>,
        IDataHandler<HeadingInputData>
    {
        private readonly Connection _connection;
        private readonly PlayerState _playerState;
        private uint _entityId;

        public PlayerMessageHandler(Connection connection)
        {
            _connection = connection;
            _playerState = (PlayerState)connection.State;
        }

        public void HandleData(ref AimInputData data)
        {
            if (_entityId == 0)
            {
                return;
            }

            ref var aim = ref _connection.World.Entities.GetComponent<AimComponent>(_entityId);
            aim.Aiming = data.Aiming;
            aim.AimCoordinates = data.Coordinates;
        }

        public void HandleData(ref ChangeShipInputData data)
        {
            if (_entityId == 0)
            {
                return;
            }

#if DEBUG
            _connection.World.UpgradeShip(_entityId);
#endif
        }

        public void HandleData(ref HeadingInputData data)
        {
            if (_entityId == 0)
            {
                return;
            }

            var heading = data.Heading;
            if (heading.SqrMagnitude > 1)
            {
                heading = heading.SetMagnitude(1);
            }

            ref var headingComponent = ref _connection.World.Entities.GetComponent<HeadingComponent>(_entityId);
            headingComponent.Heading = heading;
        }

        public void HandleEntity(uint entityId)
        {

        }

        public void HandleRemove(uint entityId)
        {

        }

        public void HandleWorld(uint worldId)
        {

        }

        public void PostHandle()
        {

        }

        public void PreHandle(uint time)
        {
            ref var player = ref _connection.World.Entities.GetComponent<PlayerComponent>(_connection.EntityId);
            if (!_connection.World.Entities.EntityExists(player.EntityId))
            {
                _entityId = 0;
            }
            else
            {
                _entityId = player.EntityId;
            }
        }
    }
}
