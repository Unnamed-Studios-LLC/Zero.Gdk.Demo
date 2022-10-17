using System.Collections.Generic;
using System.Threading.Tasks;
using UnnamedStudios.Common.Repositories.Abstract;
using Zero.Demo.Api.Providers.Interfaces;
using Zero.Demo.Core;
using Zero.Game.Model;
using Zero.ServiceApi.Model.Deployment;

namespace Zero.Demo.Api.Providers
{
    public class CachedDeploymentProvider : ICachedDeploymentProvider
    {
        private readonly IDeploymentProvider _deploymentProvider;
        private readonly ICacheRepository<Task<IEnumerable<GetDeploymentResponse>>> _deploymentCacheRepository;
        private readonly DemoSettings _settings;

        public CachedDeploymentProvider(IDeploymentProvider deploymentProvider,
            ICacheRepository<Task<IEnumerable<GetDeploymentResponse>>> deploymentCacheRepository,
            DemoSettings settings)
        {
            _deploymentProvider = deploymentProvider;
            _deploymentCacheRepository = deploymentCacheRepository;
            _settings = settings;
        }

        public async ValueTask<IEnumerable<GetDeploymentResponse>> GetDeploymentsAsync()
        {
            return await _deploymentCacheRepository.Get("deployments", _settings.ApiDeploymentTtl, async () =>
            {
                return (await _deploymentProvider.GetDeploymentsAsync()) ?? new List<GetDeploymentResponse>();
            });
        }

        public Task<StartConnectionResponse> StartConnectionAsync(GetDeploymentResponse deployment, StartConnectionRequest request)
        {
            return _deploymentProvider.StartConnectionAsync(deployment, request);
        }
    }
}
