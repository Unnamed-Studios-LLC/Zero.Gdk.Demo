using Zero.Demo.Model.Types;

namespace Zero.Demo.World.Component
{
    public struct IconComponent
    {
        public IconType IconType;

        public IconComponent(IconType iconType)
        {
            IconType = iconType;
        }
    }
}
