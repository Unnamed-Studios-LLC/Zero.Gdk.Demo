using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero.Demo.Api.Providers;
using Zero.Demo.Api.Providers.Interfaces;
using Zero.Demo.Model;
using Zero.Demo.Model.Api;
using Zero.Game.Model;

namespace Zero.Demo.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly ILogger<ConnectionController> _logger;
        private readonly GameClientProvider _gameClientProvider;
        private readonly ICachedDeploymentProvider _cachedDeploymentProvider;
        private readonly ProfanityFilter.ProfanityFilter _profanityFilter;

        public ConnectionController(ILogger<ConnectionController> logger,
            GameClientProvider gameClientProvider,
            ICachedDeploymentProvider cachedDeploymentProvider,
            ProfanityFilter.ProfanityFilter profanityFilter)
        {
            _logger = logger;
            _gameClientProvider = gameClientProvider;
            _cachedDeploymentProvider = cachedDeploymentProvider;
            _profanityFilter = profanityFilter;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateConnectionRequest request)
        {
            var address = Request.HttpContext.Connection.RemoteIpAddress;
            string requesterIp = null;
            if (address != null)
            {
                requesterIp = (address.IsIPv4MappedToIPv6 ? address.MapToIPv4() : address).ToString();
            }

            for (int i = 0; i < request.Name.Length; i++)
            {
                var c = request.Name[i];
                if (c < 65 || (c > 90 && c < 97) || c > 122) // only ascii letters are allowed
                {
                    return BadRequest();
                }
            }

            if (_profanityFilter.ContainsProfanity(request.Name))
            {
                return BadRequest();
            }

            var gameRequest = new StartConnectionRequest
            {
                ClientIp = requesterIp,
                Data = new Dictionary<string, string>
                {
                    ["name"] = request.Name,
                    ["hat"] = request.Hat.ToString(),
                    ["head"] = request.Head.ToString(),
                    ["legs"] = request.Legs.ToString(),
                    ["flag"] = request.Flag.ToString(),
                    ["flagColor"] = request.FlagColor.ToString(),
                    ["worldKey"] = request.WorldKey
                }
            };

            var task = _cachedDeploymentProvider.GetDeploymentsAsync();
            var deployments = task.IsCompleted ? task.Result : await task;
            var deployment = deployments.FirstOrDefault();
            if (deployment == null)
            {
                return NotFound();
            }

            var client = _gameClientProvider.GetClient(deployment);
            var gameResponse = await client.ConnectionStartAsync(gameRequest);
            if (!gameResponse.Successful)
            {
                _logger.LogError(gameResponse.Error);
                return StatusCode(500);
            }

            if (!gameResponse.Object.Started)
            {
                return gameResponse.Object.FailReason switch
                {
                    ConnectionFailReason.WorldNotFound => NotFound(),
                    _ => StatusCode(500),
                };
            }

            var response = new CreateConnectionResponse
            {
                Ip = gameResponse.Object.WorkerIp,
                Key = gameResponse.Object.Key,
                Port = gameResponse.Object.Port
            };
            return Ok(response);
        }
    }
}
