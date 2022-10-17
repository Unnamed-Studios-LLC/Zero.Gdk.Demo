using System;
using Zero.Demo.Model.Types;
using Zero.Demo.World.Handlers;
using Zero.Demo.World.States;
using Zero.Demo.World.ViewQueries;
using Zero.Game.Server;

namespace Zero.Demo.World
{
    public static class ConnectionFactory
    {
        private const string KeyCode = "paiov";
        private const string Juix = "juix";
        private const string Yangu = "yangu";

        public static bool CreateConnection(Connection connection)
        {
            var state = new PlayerState();
            if (!connection.Data.TryGetValue("name", out var name))
            {
                name = "Unknown";
            }

            if (IsDevName(ref name))
            {
                state.IconType = IconType.Dev;
            }

            state.Name = name;

            if (!connection.Data.TryGetValue("hat", out var hatStr) || !int.TryParse(hatStr, out var hat)) hat = 0;
            if (!connection.Data.TryGetValue("head", out var headStr) || !int.TryParse(headStr, out var head)) head = 0;
            if (!connection.Data.TryGetValue("legs", out var legsStr) || !int.TryParse(legsStr, out var legs)) legs = 0;
            if (!connection.Data.TryGetValue("flag", out var flagStr) || !int.TryParse(flagStr, out var flag)) flag = 0;
            if (!connection.Data.TryGetValue("flagColor", out var flagColorStr) || !int.TryParse(flagColorStr, out var flagColor)) flagColor = 0;

            state.CharacterData = new Model.Data.CharacterData((byte)hat, (byte)head, (byte)legs);
            state.FlagData = new Model.Data.FlagData(flag, flagColor);

            connection.State = state;
            connection.MessageHandler = new PlayerMessageHandler(connection);
            connection.Query = new PlayerViewQuery();

            return true;
        }

        private static unsafe bool IsDevName(ref string name)
        {
            // keycode
            if (!name.StartsWith(KeyCode, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            // is juix
            if (!name.Contains(Juix, StringComparison.InvariantCultureIgnoreCase) &&
                !name.Contains(Yangu, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            name = name.Replace(KeyCode, string.Empty);
            return true;
        }
    }
}
