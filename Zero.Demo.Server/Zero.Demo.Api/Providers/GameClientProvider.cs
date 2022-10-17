using System;
using System.Collections.Concurrent;
using System.Linq;
using Zero.Game.Model;
using Zero.ServiceApi.Model.Deployment;

namespace Zero.Demo.Api.Providers
{
    public class GameClientProvider
    {
        private readonly ConcurrentDictionary<long, (string Host, ZeroGameClient Client)> _clients = new();

        public ZeroGameClient GetClient(GetDeploymentResponse deployment)
        {
            var primaryInstance = deployment.Instances.FirstOrDefault(x => x.Id == deployment.InstanceId);
            if (!_clients.TryGetValue(deployment.Id, out var clientPair) ||
                !string.Equals(clientPair.Host, primaryInstance.Host, StringComparison.Ordinal))
            {
                clientPair.Host = primaryInstance.Host;
                clientPair.Client = new ZeroGameClient(primaryInstance.Host, primaryInstance.ApiToken);
                _clients[deployment.Id] = clientPair;
            }
            return clientPair.Client;
        }
    }
}
