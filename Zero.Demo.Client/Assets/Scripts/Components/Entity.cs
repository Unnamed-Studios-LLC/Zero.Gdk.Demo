using System;
using System.Collections.Generic;
using UnityEngine;
using Zero.Demo.Model;

public class Entity : MonoBehaviour
{
    private World _world;

    public EntityType EntityType;
    public World World
    {
        get => _world;
        set => SetWorld(value);
    }

    private Component[] _components;
    private Dictionary<Type, object[]> _dataHandlers = new();

    public void ProcessData<T>(ref T data) where T : unmanaged
    {
        var type = typeof(T);
        if (!_dataHandlers.TryGetValue(type, out var handlers))
        {
            handlers = GetComponents<IDataHandler<T>>();
            _dataHandlers.Add(type, handlers);
        }

        foreach (var handler in handlers)
        {
            ((IDataHandler<T>)handler).Process(ref data);
        }
    }

    private void Awake()
    {
        _components = GetComponents<Component>();
    }

    private void SetWorld(World world)
    {
        _world = world;
        foreach (var component in _components)
        {
            component.World = world;
        }
    }
}
