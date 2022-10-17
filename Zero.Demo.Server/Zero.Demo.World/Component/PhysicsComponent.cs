using Zero.Demo.World.Definitions;

namespace Zero.Demo.World.Component
{
    public struct PhysicsComponent
    {
        public PhysicsDef Def;

        public PhysicsComponent(PhysicsDef def)
        {
            Def = def;
        }
    }
}
