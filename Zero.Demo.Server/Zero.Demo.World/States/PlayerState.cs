using Zero.Demo.Model.Data;
using Zero.Demo.Model.Types;

namespace Zero.Demo.World.States
{
    public class PlayerState
    {
        public string Name { get; set; }
        public CharacterData CharacterData { get; set; }
        public IconType IconType { get; internal set; }
        public FlagData FlagData { get; set; }
    }
}
