using System.Collections.Generic;
using UnityEngine;
using Zero.Demo.Model;

public class EntityLibrary : MonoBehaviour
{
    private static readonly Dictionary<EntityType, GameObjectCache> s_caches = new();

    public Entity[] EntityPrefabs;

    public static Entity Get(EntityType entityType)
    {
        if (!s_caches.TryGetValue(entityType, out var cache))
        {
            return null;
        }

        return cache.Get().GetComponent<Entity>();
    }

    public static void Return(Entity entity)
    {
        if (!s_caches.TryGetValue(entity.EntityType, out var cache))
        {
            return;
        }

        cache.Return(entity.gameObject);
    }

    private void Awake()
    {
        foreach (var cache in s_caches.Values)
        {
            cache.Clear();
        }
        s_caches.Clear();

        foreach (var entityPrefab in EntityPrefabs)
        {
            s_caches[entityPrefab.EntityType] = new GameObjectCache(entityPrefab.gameObject);
        }
    }
}
