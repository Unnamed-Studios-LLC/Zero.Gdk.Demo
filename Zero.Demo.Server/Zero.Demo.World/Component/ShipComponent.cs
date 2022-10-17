using Zero.Demo.Model;

namespace Zero.Demo.World.Component
{
    public struct ShipComponent
    {
        public ShipType ShipType;
        public bool Dead;

        public ShipComponent(ShipType shipType) : this()
        {
            ShipType = shipType;
        }
    }
}
