using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero.Core.Model.Enums;
using Zero.Demo.Api.Providers.Interfaces;
using Zero.Demo.Core;
using Zero.Game.Model;
using Zero.Service.Model;
using Zero.ServiceApi.Model;
using Zero.ServiceApi.Model.Deployment;

namespace Zero.Demo.Api.Providers.Zero
{
    public class ZeroDeploymentProvider : IDeploymentProvider
    {
        private readonly DemoSettings _settings;
        private readonly ILogger<ZeroDeploymentProvider> _logger;
        private readonly GameClientProvider _gameClientProvider;
        private readonly ZeroServiceApiClient _zeroServiceApiClient;

        public ZeroDeploymentProvider(DemoSettings settings,
            ILogger<ZeroDeploymentProvider> logger,
            GameClientProvider gameClientProvider,
            ZeroServiceApiClient zeroServiceApiClient)
        {
            _settings = settings;
            _logger = logger;
            _gameClientProvider = gameClientProvider;
            _zeroServiceApiClient = zeroServiceApiClient;
        }

        public async ValueTask<IEnumerable<GetDeploymentResponse>> GetDeploymentsAsync()
        {
            var response = await _zeroServiceApiClient.DeploymentGetAllAsync(ZeroMetaData.ProjectId);
            if (!response.Successful)
            {
                // error
                _logger.LogError("Failed to retrieve deployments, status code: {0} error: {1}", response.StatusCode, response.Error);
                return null;
            }

            var deployments = response.Object.Deployments
                .Where(x => x.ServiceId == _settings.ZeroWorldServiceId &&
                    x.EnvironmentId == ZeroMetaData.EnvironmentId &&
                    (x.Instances?.Count(x => x.Status == InstanceStatus.Running) ?? 0) > 1);

            return deployments;
        }

        public async Task<StartConnectionResponse> StartConnectionAsync(GetDeploymentResponse deployment, StartConnectionRequest request)
        {
            var response = await _gameClientProvider.GetClient(deployment)
                .ConnectionStartAsync(request);

            if (!response.Successful)
            {
                _logger.LogError("Failed to create connection in deployment.\n\tStatus code: {0}\n\tError: {1}\n\tRequest: {2}", response.StatusCode, response.Error, JsonConvert.SerializeObject(request));
                return null;
            }
            return response.Object;
        }

        private static string GetPingIp(Dictionary<long, GetDeploymentResponse> pings, long regionId)
        {
            if (!pings.TryGetValue(regionId, out var x))
            {
                return null;
            }
            return x.Instances.FirstOrDefault()?.Host;
        }
    }
}
