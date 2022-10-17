using UnnamedStudios.Common.Model.Configuration;
using Zero.Game.Shared;

namespace Zero.Demo.Core
{
    public class DemoSettings : ConfigurationBase
    {
        [ConfigKey("Api.Url")]
        public string ApiUrl => GetValue();

        [ConfigKey("Api.Key")]
        public string ApiKey => GetValue();

        [ConfigKey("Api.DeploymentTtl")]
        public int ApiDeploymentTtl => GetIntValue();

        [ConfigKey("Game.LogLevel")]
        public LogLevel LogLevel => GetEnumValue(LogLevel.Information);

        [ConfigKey("Https.CertificatePassword")]
        public string HttpsCertificatePassword => GetValue();

        [ConfigKey("Local")]
        public bool Local => GetBoolValue();

        [ConfigKey("Game.WorldAutoFillMaxConnections")]
        public int WorldAutoFillMaxConnections => GetIntValue(30);

        [ConfigKey("Game.WorldMaxConnections")]
        public int WorldMaxConnections => GetIntValue(40);

        [ConfigKey("Game.MaxWorlds")]
        public int MaxWorlds => GetIntValue(100);

        [ConfigKey("Zero.ServiceToken")]
        public string ZeroServiceToken => GetValue();

        [ConfigKey("Zero.ServiceUrl")]
        public string ZeroServiceUrl => GetValue();

        [ConfigKey("Zero.WorldServiceId")]
        public long ZeroWorldServiceId => GetLongValue();
    }
}
