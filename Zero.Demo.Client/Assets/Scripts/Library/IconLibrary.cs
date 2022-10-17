using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zero.Demo.Model.Types;

public class IconLibrary : MonoBehaviour
{
    [Serializable]
    public struct Icon
    {
        public IconType Type;
        public Sprite Sprite;
    }

    public static Dictionary<IconType, Sprite> s_icons = new();

    public Icon[] Icons;

    public static bool TryGet(IconType type, out Sprite sprite)
    {
        return s_icons.TryGetValue(type, out sprite);
    }

    private void Awake()
    {
        s_icons = Icons.ToDictionary(x => x.Type, x => x.Sprite);
    }
}