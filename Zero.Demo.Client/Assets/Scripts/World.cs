using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Zero.Demo.Model.Data;
using Zero.Game.Shared;

public class World : MonoBehaviour, IMessageHandler,
    Zero.Game.Shared.IDataHandler<ControlData>,
    Zero.Game.Shared.IDataHandler<CharacterData>,
    Zero.Game.Shared.IDataHandler<EntityTypeData>,
    Zero.Game.Shared.IDataHandler<EventData>,
    Zero.Game.Shared.IDataHandler<FlagData>,
    Zero.Game.Shared.IDataHandler<HitData>,
    Zero.Game.Shared.IDataHandler<IconData>,
    Zero.Game.Shared.IDataHandler<LeaderboardNamesData>,
    Zero.Game.Shared.IDataHandler<LeaderboardValuesData>,
    Zero.Game.Shared.IDataHandler<NameData>,
    Zero.Game.Shared.IDataHandler<PositionData>,
    Zero.Game.Shared.IDataHandler<ResourceData>,
    Zero.Game.Shared.IDataHandler<RockData>,
    Zero.Game.Shared.IDataHandler<ShootData>,
    Zero.Game.Shared.IDataHandler<ShipData>,
    Zero.Game.Shared.IDataHandler<WoodData>,
    Zero.Game.Shared.IDataHandler<WorldKeyData>
{
    public Aim Aim;
    public AudioSource AudioSource;
    public Transform Controlled;
    public Follow CameraFollow;
    public ObliqueCamera ObliqueCamera;
    //public PixelPerfectCamera PPCamera;
    public Camera MainCamera;
    public AudioSource AmbientSource;
    public Leaderboard Leaderboard;
    public float CurrentRange = 18;
    public SpriteRenderer Direction;
    public Follow DirectionFollow;
    public TextMeshProUGUI WorldKey;

    private float _targetOrthoSize = 20;
    private readonly Dictionary<uint, Entity> _entities = new();
    private uint _handlingEntityId;
    private Entity _handlingEntity;
    private Vector2? _controlledPosition;

    private uint _controlledEntityId;
    private float _speedScale;

    public float GetVolumeFromPlayer(Vector2 coordinates)
    {
        if (Controlled == null)
        {
            return 0;
        }

        var controlledCoords = (Vector2)Controlled.transform.position;
        var magnitude = (controlledCoords - coordinates).magnitude;
        if (magnitude <= 10)
        {
            return 1;
        }

        var inverse = 1 / (magnitude / 10);
        return inverse * inverse;
        
        //return Mathf.Clamp01(1 - Mathf.Sqrt((Mathf.Max(magnitude - 10f, 0)) / 30f));
    }

    public void HandleData(ref ControlData data)
    {
        _controlledEntityId = data.EntityId;
        if (_entities.TryGetValue(data.EntityId, out var controlled))
        {
            SetControlled(controlled);
        }
    }

    public void HandleData(ref EntityTypeData data)
    {
        _handlingEntity = EntityLibrary.Get(data.EntityType);
        _entities.Add(_handlingEntityId, _handlingEntity);
        _handlingEntity.World = this;

        if (_handlingEntityId == _controlledEntityId)
        {
            SetControlled(_handlingEntity);
        }
    }

    public void HandleData(ref LeaderboardNamesData data)
    {
        Leaderboard.SetNames(ref data);
    }

    public void HandleData(ref LeaderboardValuesData data)
    {
        Leaderboard.SetValues(ref data);
    }

    public unsafe void HandleData(ref WorldKeyData data)
    {
        fixed (byte* buffer = data.Key)
        {
            var key = Encoding.ASCII.GetString(buffer, 4);
            WorldKey.text = "W# " + key;
            DiscordController.Instance.SetWorld(key);
        }
    }

    public void HandleData(ref CharacterData data) => HandleEntityData(ref data);
    public void HandleData(ref EventData data) => HandleEntityData(ref data);
    public void HandleData(ref FlagData data) => HandleEntityData(ref data);
    public void HandleData(ref HitData data) => HandleEntityData(ref data);
    public void HandleData(ref IconData data) => HandleEntityData(ref data);
    public void HandleData(ref NameData data) => HandleEntityData(ref data);
    public void HandleData(ref PositionData data) => HandleEntityData(ref data);
    public void HandleData(ref ResourceData data) => HandleEntityData(ref data);
    public void HandleData(ref RockData data) => HandleEntityData(ref data);
    public void HandleData(ref ShootData data) => HandleEntityData(ref data);
    public void HandleData(ref ShipData data) => HandleEntityData(ref data);
    public void HandleData(ref WoodData data) => HandleEntityData(ref data);

    public void HandleEntity(uint entityId)
    {
        _handlingEntityId = entityId;
        _entities.TryGetValue(entityId, out _handlingEntity);
    }

    public void HandleRemove(uint entityId)
    {
        if (!_entities.TryGetValue(entityId, out var entity))
        {
            return;
        }

        _entities.Remove(entityId);
        EntityLibrary.Return(entity);

        if (entityId == _controlledEntityId)
        {
            _controlledPosition = null;
            SetControlled(null);
            _controlledEntityId = 0;
        }
    }

    public void HandleWorld(uint worldId)
    {
    }

    public void PostHandle()
    {

    }

    public void PreHandle(uint time)
    {

    }

    public void SetMaxRange(float maxRange)
    {
        _targetOrthoSize = 8 + maxRange * 0.4f;
        CurrentRange = maxRange;
    }

    private void HandleEntityData<T>(ref T data) where T : unmanaged
    {
        if (_handlingEntity == null)
        {
            return;
        }
        _handlingEntity.ProcessData(ref data);
    }

    private void LateUpdate()
    {
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, _targetOrthoSize, Time.deltaTime * 5);

        float speedScale = 0;
        if (Controlled != null)
        {
            var position = (Vector2)Controlled.position;
            var speed = ((_controlledPosition ?? position) - position).magnitude / Time.deltaTime;
            _controlledPosition = position;
            speedScale = Mathf.Clamp01(speed / 10f);
        }

        _speedScale = Mathf.Lerp(_speedScale, speedScale, Time.deltaTime * 3);
        AmbientSource.volume = 0.1f + _speedScale * 0.8f;
        AmbientSource.pitch = 1 + _speedScale * 0.8f;
    }

    private void SetControlled(Entity controlled)
    {
        if (controlled != null)
        {
            Controlled = controlled.transform;
            var control = controlled.GetComponent<Control>();
            if (control != null)
            {
                control.SetControlled();
            }
        }
        else
        {
            Controlled = null;
        }
        DirectionFollow.Target = Controlled;
        Aim.Aiming = false;
    }
}