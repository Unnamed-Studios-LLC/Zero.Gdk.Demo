using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
    public static DiscordController Instance;

    public long AppId = 1027352450765688842;

#if PLATFORM_STANDALONE || UNITY_EDITOR
    private Discord.Discord _discord;

    public void SetMainMenu()
    {
        var activity = new Discord.Activity
        {
            State = "In Main Menu",
            Timestamps = new Discord.ActivityTimestamps
            {
                Start = DateTimeOffset.Now.ToUnixTimeSeconds()
            },
            Assets = new Discord.ActivityAssets
            {
                LargeImage = "logo",
                LargeText = "Scurvy Dogs"
            },
            Instance = true
        };
        _discord.GetActivityManager().UpdateActivity(activity, result => { });
    }
    public void SetWorld(string key)
    {
        var activity = new Discord.Activity
        {
            Details = "Battling on the high seas!",
            State = $"W# {key}",
            Timestamps = new Discord.ActivityTimestamps
            {
                Start = DateTimeOffset.Now.ToUnixTimeSeconds()
            },
            Assets = new Discord.ActivityAssets
            {
                LargeImage = "logo",
                LargeText = "Scurvy Dogs"
            },
            Instance = true
        };
        _discord.GetActivityManager().UpdateActivity(activity, result => { });
    }

    private void Awake()
    {
        var current = FindObjectOfType<DiscordController>();
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        _discord = new Discord.Discord(1027352450765688842, (ulong)Discord.CreateFlags.NoRequireDiscord);
        _discord.GetActivityManager().RegisterSteam(2178170);
        _discord.GetActivityManager().OnActivityJoin += OnJoin;
    }

    private void OnJoin(string secret)
    {
        MainMenu.JoinKey = secret;
        SceneManager.LoadScene("MenuScene");
    }

    private void Update()
    {
        _discord?.RunCallbacks();
    }
#else

    public void SetMainMenu() { }
    public void SetWorld(string key) { }
#endif
}
