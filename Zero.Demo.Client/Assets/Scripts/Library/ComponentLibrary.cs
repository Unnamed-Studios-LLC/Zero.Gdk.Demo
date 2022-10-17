using System;
using System.Collections.Generic;
using UnityEngine;

public class ComponentLibrary : MonoBehaviour
{
    private readonly static Dictionary<Type, GameObjectCache> s_caches = new();
    private static Transform _cacheParent;

    public MonoBehaviour[] ComponentPrefabs;

    public static T Get<T>() where T : MonoBehaviour
    {
        return Get(typeof(T)).GetComponent<T>();
    }

    public static GameObject Get(Type type)
    {
        if (!s_caches.TryGetValue(type, out var cache))
        {
            return null;
        }

        var obj = cache.Get(false);
        obj.SetActive(false);
        obj.transform.SetParent(null);
        obj.SetActive(true);
        return obj;
    }

    public static void Return(MonoBehaviour component)
    {
        if (!s_caches.TryGetValue(component.GetType(), out var cache))
        {
            return;
        }

        component.gameObject.transform.SetParent(_cacheParent);
        cache.Return(component.gameObject);
    }

    private void Awake()
    {
        foreach (var cache in s_caches.Values)
        {
            cache.Clear();
        }
        s_caches.Clear();

        foreach (var componentPrefab in ComponentPrefabs)
        {
            s_caches[componentPrefab.GetType()] = new GameObjectCache(componentPrefab.gameObject);
        }
        _cacheParent = transform;
    }
}
