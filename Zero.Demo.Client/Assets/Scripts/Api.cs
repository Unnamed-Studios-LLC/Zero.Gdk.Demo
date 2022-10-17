using UnityEngine;
using UnnamedStudios.Common.Model;
using Zero.Demo.Model.Api;

public static class Api
{
    private const string Domain =
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        "https://localhost:6001";
#else
        "https://demo.zeroservices.co";
#endif

    private static DemoApiClient _client;

    public static DemoApiClient Client => _client ??= new DemoApiClient(GetOptions());

    private static ServiceClientOptions GetOptions()
    {
        return new ServiceClientOptions(Domain)
        {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            LogFunc = Debug.Log,
#endif
#if PLATFORM_ANDROID
            IgnoreSslErrors = true
#endif
        };
    }
}
