using System;
using Zero.Demo.Model.Data;
using Zero.Demo.Model.Types;

namespace Zero.Demo.World.Component
{
    public unsafe struct PlayerComponent
    {
        public fixed byte Name[NameData.NameLength];
        public IconType IconType;
        public uint EntityId;
        public CharacterData CharacterData;
        public FlagData FlagData;
        public float RespawnCooldown;

        public PlayerComponent(ReadOnlySpan<char> name, IconType iconType, uint entityId, CharacterData characterData, FlagData flagData)
        {
            fixed (char* ptr = name)
            {
                for (int i = 0; i < NameData.NameLength; i++)
                {
                    Name[i] = (byte)(i < name.Length ? ptr[i] : 0);
                }
            }

            IconType = iconType;
            EntityId = entityId;
            CharacterData = characterData;
            FlagData = flagData;
            RespawnCooldown = 0;
        }
    }
}
