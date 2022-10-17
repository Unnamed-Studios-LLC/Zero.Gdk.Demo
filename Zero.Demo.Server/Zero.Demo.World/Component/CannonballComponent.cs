using Zero.Game.Shared;

namespace Zero.Demo.World.Component
{
    public struct CannonballComponent
    {
        public Vec2 Target;
        public float HitTime;
        public int Damage;

        public CannonballComponent(Vec2 target, float hitTime, int damage)
        {
            Target = target;
            HitTime = hitTime;
            Damage = damage;
        }
    }
}
