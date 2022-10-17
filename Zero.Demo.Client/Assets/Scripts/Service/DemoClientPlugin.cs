using UnityEngine.SceneManagement;
using Zero.Demo.Model;
using Zero.Game.Client;
using Zero.Game.Shared;

public class DemoClientPlugin : ClientPlugin
{
    private Loading _loading;

    public DemoClientPlugin()
    {
        Options.LogLevel = LogLevel.Trace;
    }

    public override void BuildData(DataBuilder builder)
    {
        builder.BuildDemoData();
    }

    public override void Connected()
    {
        UnityEngine.Debug.Log("Connected to remote host");
        if (_loading != null)
        {
            _loading.Hide();
            _loading = null;
        }
    }

    public override void Connecting()
    {
        UnityEngine.Debug.Log("Connecting...");
        _loading = Loading.Show();
    }

    public override void Disconnected()
    {
        UnityEngine.Debug.Log("Disconnected from remote host");
        SceneManager.LoadScene("MenuScene");
    }
}