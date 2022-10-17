using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zero.Core.Model.Enums;
using Zero.Demo.Api.Providers.Interfaces;
using Zero.Demo.Core;
using Zero.Game.Model;
using Zero.ServiceApi.Model.Deployment;

namespace Zero.Demo.Api.Providers.Local
{
    public class LocalDeploymentProvider : IDeploymentProvider
    {
        private readonly DemoSettings _settings;
        private readonly ILogger<LocalDeploymentProvider> _logger;
        private readonly GameClientProvider _gameClientProvider;

        public LocalDeploymentProvider(DemoSettings settings,
            ILogger<LocalDeploymentProvider> logger,
            GameClientProvider gameClientProvider)
        {
            _settings = settings;
            _logger = logger;
            _gameClientProvider = gameClientProvider;
        }

        public ValueTask<IEnumerable<GetDeploymentResponse>> GetDeploymentsAsync()
        {
            var localIp = "localhost:4001";
            var deployment = new GetDeploymentResponse
            {
                Id = 1,
                Name = "Local",
                InstanceId = 1,
                StaticIp = localIp,
                Instances = new List<GetInstanceResponse>()
                {
                    new GetInstanceResponse
                    {
                        Id = 1,
                        Type = InstanceType.Game,
                        Host = localIp,
                        ApiToken = string.Empty
                    },
                    new GetInstanceResponse
                    {
                        Id = 2,
                        Type = InstanceType.Worker,
                        Host = localIp,
                        ApiToken = string.Empty
                    }
                },
                Tags = new Dictionary<string, string>
                {
                    { "Group", "Development" }
                }
            };

            var response = new List<GetDeploymentResponse>
            {
                deployment
            };
            return ValueTask.FromResult((IEnumerable<GetDeploymentResponse>)response);
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
    }
}
