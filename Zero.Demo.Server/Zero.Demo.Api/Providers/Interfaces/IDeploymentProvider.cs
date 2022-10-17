using System.Collections.Generic;
using System.Threading.Tasks;
using Zero.Game.Model;
using Zero.ServiceApi.Model.Deployment;

namespace Zero.Demo.Api.Providers.Interfaces
{
    public interface IDeploymentProvider
    {
        ValueTask<IEnumerable<GetDeploymentResponse>> GetDeploymentsAsync();

        Task<StartConnectionResponse> StartConnectionAsync(GetDeploymentResponse deployment, StartConnectionRequest request);
    }
}
