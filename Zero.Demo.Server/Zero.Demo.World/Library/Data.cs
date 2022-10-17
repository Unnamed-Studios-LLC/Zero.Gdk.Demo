using Zero.Demo.Model;
using Zero.Demo.World.Definitions;

namespace Zero.Demo.World
{
    public static class Data
    {
        public readonly static PhysicsDef[] RockDefs = new PhysicsDef[]
        {
            //     width|     height|     density|   friction|  restitution|
            new(new(5.0f,       1.70f),          0,          1,            0),
            new(new(3.2f,       1.10f),          0,          1,            0),
            new(new(2.3f,       1.10f),          0,          1,            0),
            new(new(3.2f,       1.10f),          0,          1,            0),
            new(new(6.6f,       8.50f),          0,          1,            0),
            new(new(6.0f,       3.25f),          0,          1,            0),
        };

        public readonly static ShipDef[] ShipDefs = new ShipDef[]
        {
            //  shiptype        speed|        width|     height|     density|   friction|  restitution|   cannons|     spread|   fire rate|   max wood| death wood|  damage|  max range|
            new(ShipType.Dinghy,   11, new(new(1.8f,       0.8f),       1.0f,       1.0f,         0.8f),        1,       1.0f,       1.20f,         15,          6,       2,         18),
            new(ShipType.Cutter,   28, new(new(3.0f,       1.3f),       1.5f,       1.0f,         0.8f),        3,       2.0f,       0.80f,         30,         14,       2,         22),
            new(ShipType.Schooner, 42, new(new(4.0f,       1.3f),       2.0f,       1.0f,         0.8f),        6,       3.0f,       0.60f,         60,         24,       3,         26),
            new(ShipType.Frigate,  70, new(new(7.0f,       1.6f),       2.5f,       1.0f,         0.8f),       10,       4.0f,       0.50f,        120,         50,       3,         32),
            new(ShipType.Galleon,  90, new(new(9.0f,       1.8f),       3.0f,       1.0f,         0.8f),       23,       5.5f,       0.46f,        240,        100,       4,         40),
            new(ShipType.ManOWar,  90, new(new(9.0f,       1.8f),       3.0f,       1.0f,         0.8f),       37,       7.0f,       0.43f,        300,        150,       4,         42),
        };

        public readonly static PhysicsDef[] WoodDefs = new PhysicsDef[]
        {
            //     width|     height|     density|   friction|  restitution|
            new(new(1.2f,       1.2f),       0.5f,          0,            0),
            new(new(1.2f,       1.2f),       0.5f,          0,            0),
            new(new(1.2f,       1.2f),       0.5f,          0,            0),
            new(new(1.2f,       1.2f),       0.5f,          0,            0),
            new(new(1.2f,       1.2f),       0.5f,          0,            0),
            new(new(1.2f,       1.2f),       0.5f,          0,            0),
            new(new(1.2f,       1.2f),       0.5f,          0,            0),
        };
    }
}
