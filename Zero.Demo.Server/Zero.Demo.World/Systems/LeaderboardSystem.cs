using System.Collections.Generic;
using Zero.Demo.Model.Data;
using Zero.Demo.Model.Types;
using Zero.Demo.World.Component;
using Zero.Game.Server;

namespace Zero.Demo.World.Systems
{
    public unsafe class LeaderboardSystem : ComponentSystem
    {
        private readonly List<(int Value, uint EntityId)> _orderedPlayers = new();

        protected override void OnUpdate()
        {
            ref var leaderboardComponent = ref Entities.TryGetComponent<LeaderboardComponent>(World.EntityId, out var found);
            if (!found)
            {
                return;
            }
            _orderedPlayers.Clear();
            var min = 0;

            // collect top three
            Entities.With<PlayerTag>().ForEach((uint entityId, ref ResourceComponent resource) =>
            {
                if (resource.WoodRunningTotal <= min &&
                    _orderedPlayers.Count >= LeaderboardComponent.Count)
                {
                    return;
                }

                int i;
                for (i = 0; i < _orderedPlayers.Count; i++)
                {
                    var value = _orderedPlayers[i].Value;
                    if (resource.WoodRunningTotal > value)
                    {
                        _orderedPlayers.Insert(i, (resource.WoodRunningTotal, entityId));
                        break;
                    }
                }

                if (i == _orderedPlayers.Count &&
                    _orderedPlayers.Count < LeaderboardComponent.Count)
                {
                    _orderedPlayers.Add((resource.WoodRunningTotal, entityId));
                }

                while (_orderedPlayers.Count > LeaderboardComponent.Count)
                {
                    _orderedPlayers.RemoveAt(_orderedPlayers.Count - 1);
                }
                min = _orderedPlayers[^1].Value;
            });

            var namesChanged = false;
            var valuesChanged = false;
            for (int i = 0; i < LeaderboardComponent.Count; i++)
            {
                var currentEntityId = leaderboardComponent.EntityIds[i];
                if (i < _orderedPlayers.Count)
                {
                    var (resource, entityId) = _orderedPlayers[i];
                    if (currentEntityId != entityId)
                    {
                        if (i == 0)
                        {
                            if (currentEntityId != 0 && Entities.EntityExists(currentEntityId))
                            {
                                ResetIcon(currentEntityId);
                            }
                            SetIcon(entityId, IconType.Crown);
                        }

                        leaderboardComponent.EntityIds[i] = entityId;
                        leaderboardComponent.Values[i] = resource;
                        namesChanged = true;
                    }
                    else if (resource != leaderboardComponent.Values[i])
                    {
                        leaderboardComponent.Values[i] = resource;
                        valuesChanged = true;
                    }
                }
                else if (currentEntityId != 0)
                {
                    if (i == 0 && Entities.EntityExists(currentEntityId))
                    {
                        ResetIcon(currentEntityId);
                    }

                    leaderboardComponent.EntityIds[i] = 0;
                    namesChanged = true;
                }
            }

            if (namesChanged)
            {
                var leaderboardNames = new LeaderboardNamesData();
                for (int i = 0; i < LeaderboardNamesData.Count; i++)
                {
                    var entityId = leaderboardComponent.EntityIds[i];
                    if (entityId != 0)
                    {
                        var currentNameData = Entities.GetPersistent<NameData>(entityId);
                        var currentFlag = Entities.GetPersistent<FlagData>(entityId);
                        leaderboardNames.Flags[i] = currentFlag.Value;
                        leaderboardNames.SetName(i, currentNameData.Name);
                    }
                }
                Entities.PushPersistent(World.EntityId, leaderboardNames);
            }

            if (namesChanged || valuesChanged)
            {
                var leaderboardValues = new LeaderboardValuesData();
                for (int i = 0; i < LeaderboardNamesData.Count; i++)
                {
                    leaderboardValues.Values[i] = leaderboardComponent.Values[i];
                }
                Entities.PushPersistent(World.EntityId, leaderboardValues);
            }
        }

        private void ResetIcon(uint entityId)
        {
            ref var icon = ref Entities.GetComponent<IconComponent>(entityId);
            Entities.PushPersistent(entityId, new IconData(icon.IconType));
        }

        private void SetIcon(uint entityId, IconType iconType)
        {
            Entities.PushPersistent(entityId, new IconData(iconType));
        }
    }
}
