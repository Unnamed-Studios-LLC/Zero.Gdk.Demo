using Zero.Demo.Model;

namespace Zero.Demo.World.Definitions
{
    public struct ShipDef
    {
        public ShipDef(ShipType type, float speed, PhysicsDef physicsDef, int cannons, float cannonSpread, float fireRate, int maxWood, int deathWood, int damage, float maxRange)
        {
            Type = type;
            Speed = speed;
            PhysicsDef = physicsDef;
            Cannons = cannons;
            CannonSpread = cannonSpread;
            FireRate = fireRate;
            MaxWood = maxWood;
            DeathWood = deathWood;
            Damage = damage;
            MaxRange = maxRange;
        }

        public ShipType Type { get; }
        public float Speed { get; }
        public PhysicsDef PhysicsDef { get; }
        public int Cannons { get; }
        public float CannonSpread { get; }
        public float FireRate { get; }
        public float FireRateCooldown => 1f / FireRate;
        public int MaxWood { get; }
        public int DeathWood { get; }
        public int Damage { get; }
        public float MaxRange { get; }
        public float MaxRangeSqr => MaxRange * MaxRange;
    }
}
