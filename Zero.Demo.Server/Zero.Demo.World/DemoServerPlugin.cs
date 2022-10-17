using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zero.Demo.Model;
using Zero.Demo.World.Component;
using Zero.Demo.World.Global;
using Zero.Demo.World.States;
using Zero.Game.Model;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World
{
    public class DemoServerPlugin : ServerPlugin
    {
        private static readonly char[] s_worldKeyCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private DateTime _lastWorldsFetch;
        private readonly object _worldsLock = new();
        private readonly object _startWorldLock = new();
        private readonly List<WorldInfo> _worlds = new();
        private Task<bool> _startWorldTask;
        private readonly ConcurrentDictionary<string, uint> _worldKeyToIdMap = new();
        private readonly ConcurrentDictionary<uint, string> _worldIdToKeyMap = new();
        private int _worldCount;

        public DemoServerPlugin()
        {
#if DEBUG
            Options.LogLevel = LogLevel.Trace;
#else
            Options.LogLevel = App.Settings.LogLevel;
#endif
            Options.UpdateIntervalMs = 20;
            Options.UpdatesPerViewUpdate = 15;
        }

        public override void BuildData(DataBuilder builder) => builder.BuildDemoData();

        public override void AddToWorld(Connection connection)
        {
            var playerState = (PlayerState)connection.State;
            connection.World.Entities.AddComponent(connection.EntityId, new PlayerComponent(playerState.Name.AsSpan(), playerState.IconType, 0, playerState.CharacterData, playerState.FlagData));
        }

        public override Task<bool> LoadConnectionAsync(Connection connection)
        {
            Debug.LogDebug("Loading connection {0}", connection.RemoteEndPoint);
            ConnectionFactory.CreateConnection(connection);
            return base.LoadConnectionAsync(connection);
        }

        public override Task<bool> LoadWorldAsync(Game.Server.World world)
        {
            WorldFactory.CreateWorld(world);
            return base.LoadWorldAsync(world);
        }

        public override async Task OnStartConnectionAsync(StartConnectionRequest request)
        {
            if (request.Data != null &&
                request.Data.TryGetValue("worldKey", out var worldKey) &&
                !string.IsNullOrWhiteSpace(worldKey)) // if a world key is provided, assign connection directly
            {
                if (_worldKeyToIdMap.TryGetValue(worldKey, out var worldId) && // world key exists
                    Deployment.TryGetWorld(worldId, out var worldInfo) && // world still exists
                    worldInfo.ConnectionCount < App.Settings.WorldAutoFillMaxConnections) // space available
                {
                    request.WorldId = worldId;
                }
                return;
            }

            while (request.WorldId == 0)
            {
                // put into world
                var now = DateTime.UtcNow;
                lock (_worldsLock)
                {
                    if ((now - _lastWorldsFetch).TotalSeconds > 5)
                    {
                        _lastWorldsFetch = now;
                        _worlds.Clear();
                        Deployment.GetAllWorlds(_worlds);
                        _worldCount = _worlds.Count;
                        _worlds.Sort((a, b) => a.ConnectionCount - b.ConnectionCount);
                        int i;
                        for (i = _worlds.Count - 1; i >= 0; i--) // remove all worlds that are full
                        {
                            if (_worlds[i].ConnectionCount < App.Settings.WorldAutoFillMaxConnections) // traverse downwards to the first worlds with space
                            {
                                break;
                            }
                        }

                        if (i != _worlds.Count - 1)
                        {
                            _worlds.RemoveRange(i + 1, _worlds.Count - 1 - i);
                        }
                    }

                    if (_worlds.Count > 0)
                    {
                        var last = _worlds[^1];
                        request.WorldId = last.Id;
                        if (last.ConnectionCount + 1 >= App.Settings.WorldAutoFillMaxConnections)
                        {
                            _worlds.RemoveAt(_worlds.Count - 1);
                        }
                        else
                        {
                            _worlds[^1] = new WorldInfo(last.Id, last.ConnectionCount + 1);
                        }
                    }
                }

                if (request.WorldId != 0 || // world is assigned
                    _worldCount >= App.Settings.MaxWorlds) // we're at max worlds, cannot create another
                {
                    return;
                }

                async Task<bool> startWorld()
                {
                    var response = await Deployment.StartWorldAsync(new StartWorldRequest());
                    if (response.State != WorldStartState.Started)
                    {
                        return false;
                    }

                    lock (_worldsLock)
                    {
                        _worldCount++;
                        _worlds.Insert(0, new WorldInfo(response.WorldId, 0));
                    }
                    return true;
                }

                Task<bool> task = null;
                lock (_startWorldLock)
                {
                    if (_startWorldTask != null &&
                        _startWorldTask.IsCompleted)
                    {
                        _startWorldTask = null;
                    }

                    task = _startWorldTask ??= startWorld();
                }

                if (!await task)
                {
                    return;
                }
            }
        }

        public override Task OnStartWorldAsync(StartWorldRequest request)
        {
            request.Data ??= new();
            string worldKey;
            do
            {
                worldKey = Game.Server.Random.String(4, s_worldKeyCharacters);
            }
            while (!_worldKeyToIdMap.TryAdd(worldKey, request.WorldId));
            _worldIdToKeyMap[request.WorldId] = worldKey;
            request.Data["worldKey"] = worldKey;
            Debug.Log("Starting world: {0} {1}/{2}", worldKey, _worldCount + 1, App.Settings.MaxWorlds);
            return Task.CompletedTask;
        }

        public override Task OnStopWorldAsync(uint worldId)
        {
            if (_worldIdToKeyMap.TryRemove(worldId, out var key))
            {
                _worldKeyToIdMap.TryRemove(key, out _);
                Debug.Log("Removed world: {0} {1}/{2}", key, _worldCount - 1, App.Settings.MaxWorlds);
            }
            return Task.CompletedTask;
        }

        public override void RemoveFromWorld(Connection connection)
        {
            ref var player = ref connection.World.Entities.GetComponent<PlayerComponent>(connection.EntityId);
            if (player.EntityId == 0)
            {
                return;
            }

            connection.World.Entities.DestroyEntity(player.EntityId);
            player.EntityId = 0;
        }

        public override async Task StartDeploymentAsync()
        {
            // start the first world
            await Deployment.StartWorldAsync(new StartWorldRequest
            {
                Data = new Dictionary<string, string> { ["permanent"] = "true" }
            });
        }

        public override Task UnloadConnectionAsync(Connection connection)
        {
            Debug.LogDebug("Unloading connection {0}", connection.RemoteEndPoint);
            return base.UnloadConnectionAsync(connection);
        }
    }
}
