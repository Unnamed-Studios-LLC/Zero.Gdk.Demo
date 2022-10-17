using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zero.Demo.Model.Api;
using Zero.Game.Client;
using Zero.Game.Shared;
using Zero.Unity;

public class DemoGameService : MonoBehaviour
{
    public static CreateConnectionResponse Response;
    public World World;

    public static readonly ZeroUnityLogger _logger = new();
    private ZeroClient _client;

    public bool Connected => _client?.Connected ?? false;

    public void Disconnect()
    {
        _client?.Disconnect();
        SceneManager.LoadScene("MenuScene");
    }

    public void Push<T>(ref T data) where T : unmanaged
    {
        if (!Connected)
        {
            return;
        }

        _client?.Push(data);
    }

    private void Awake()
    {
        Connect();
    }

    private void Connect()
    {
        if (Response == null)
        {
            UnityEngine.Debug.LogError("Response is null");
            SceneManager.LoadScene("MenuScene");
            return;
        }

        _client = ZeroClient.Create(IPAddress.Parse(Response.Ip), Response.Port, Response.Key, World, _logger, new DemoClientPlugin());
    }

    private void FixedUpdate()
    {
        _client?.Update((uint)Mathf.RoundToInt(Time.fixedTime * 1000));
    }

    private void OnApplicationQuit()
    {
        _logger.Update();
        _client?.Disconnect();
    }

    private void OnDestroy()
    {
        _logger.Update();
        _client?.Disconnect();
    }

    private void Update()
    {
        _logger.Update();
    }
}
