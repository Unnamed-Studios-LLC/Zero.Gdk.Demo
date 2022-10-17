using System.Threading.Tasks;
using UnnamedStudios.Common.Model;

namespace Zero.Demo.Model.Api
{
    public class DemoApiClient : ServiceClientBase
    {
        public DemoApiClient(ServiceClientOptions options) : base(options)
        {
        }

        public Task<ServiceResponse<CreateConnectionResponse>> ConnectionCreateAsync(CreateConnectionRequest request, ServiceRequestOptions options = null)
        {
            return Post<CreateConnectionRequest, CreateConnectionResponse>("/api/v1/connection", request, GetRequestOptions(options));
        }

        private static ServiceRequestOptions GetRequestOptions(ServiceRequestOptions inputOptions, bool? logResponseBody = null, bool? logRequestBody = null)
        {
            if (inputOptions == null)
            {
                inputOptions = new ServiceRequestOptions();
            }

            if (logResponseBody.HasValue)
            {
                inputOptions.LogResponseBody = logResponseBody.Value;
            }

            if (logRequestBody.HasValue)
            {
                inputOptions.LogRequestBody = logRequestBody.Value;
            }

            return inputOptions;
        }
    }
}
