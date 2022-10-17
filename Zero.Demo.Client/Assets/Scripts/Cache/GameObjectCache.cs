using System.Collections.Generic;
using UnityEngine;

public class GameObjectCache
{
    private struct StoredObject
    {
        public StoredObject(GameObject gameObject, float time)
        {
            GameObject = gameObject;
            StoredTime = time;
        }

        public GameObject GameObject { get; private set; }
        public float StoredTime { get; private set; }
    }

    private readonly List<StoredObject> _objects = new();
    private readonly GameObject _prefab;

    public GameObjectCache(GameObject prefab)
    {
        _prefab = prefab;
    }

    public void Clear()
    {
        foreach (var storedObject in _objects)
        {
            if (storedObject.GameObject)
            {
                Object.Destroy(storedObject.GameObject);
            }
        }
        _objects.Clear();
    }

    public GameObject Get(bool setActive = true)
    {
        GameObject gameObject;
        if (_objects.Count > 0)
        {
            gameObject = _objects[^1].GameObject;
            _objects.RemoveAt(_objects.Count - 1);
        }
        else
        {
            gameObject = Object.Instantiate(_prefab);
        }

        if (setActive)
        {
            gameObject.SetActive(true);
        }
        return gameObject;
    }

    public void Return(GameObject gameObject)
    {
        gameObject.SetActive(false);
        _objects.Add(new StoredObject(gameObject, Time.realtimeSinceStartup));
    }

    public void Trim(float maxTimeDif)
    {
        var minTime = Time.time - maxTimeDif;
        int removeCount = -1;
        for (int i = 0; i < _objects.Count; i++)
        {
            if (i >= removeCount)
            {
                _objects[i - removeCount] = _objects[i];
            }
            else if (_objects[i].StoredTime >= minTime)
            {
                removeCount = i + 1;
                _objects[i - removeCount] = _objects[i];
            }
            else
            {
                var storedObject = _objects[i];
                if (storedObject.GameObject)
                {
                    Object.Destroy(storedObject.GameObject);
                }
            }
        }

        if (removeCount > 0)
        {
            _objects.RemoveRange(_objects.Count - removeCount, removeCount);
        }
    }
}

