using System;
using System.Collections.Generic;
using System.Linq;
using Zero.Demo.Model;
using Zero.Demo.World.Definitions;

namespace Zero.Demo.World
{
    public static class Library
    {
        private readonly static Dictionary<ShipType, ShipDef> _shipMap = Data.ShipDefs.ToDictionary(x => x.Type);

        public static int RockCount => Data.RockDefs.Length;
        public static int ShipCount => Data.ShipDefs.Length;
        public static int WoodCount => Data.WoodDefs.Length;

        public static PhysicsDef GetRock(uint index)
        {
            return Data.RockDefs[Math.Min(index, RockCount)];
        }

        public static ShipDef GetShip(ShipType shipType)
        {
            return _shipMap.TryGetValue(shipType, out var shipDef) ? shipDef : default;
        }

        public static PhysicsDef GetWood(uint index)
        {
            return Data.WoodDefs[Math.Min(index, WoodCount)];
        }
    }
}
