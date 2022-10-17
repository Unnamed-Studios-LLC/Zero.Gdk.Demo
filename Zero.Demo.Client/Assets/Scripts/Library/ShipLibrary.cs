using System.Collections.Generic;
using UnityEngine;
using Zero.Demo.Model;

public class ShipLibrary : MonoBehaviour
{
    private readonly static Dictionary<ShipType, ShipDefinition> s_definitions = new();

    public ShipDefinition[] ShipDefinitions;

    public static ShipDefinition Get(ShipType shipType)
    {
        if (!s_definitions.TryGetValue(shipType, out var definition))
        {
            return default;
        }
        return definition;
    }

    private void Awake()
    {
        s_definitions.Clear();

        foreach (var definition in ShipDefinitions)
        {
            s_definitions[definition.ShipType] = definition;
        }
    }
}
