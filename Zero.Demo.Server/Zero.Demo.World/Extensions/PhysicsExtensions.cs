using System.Numerics;
using Zero.Game.Shared;

namespace Zero.Demo.World
{
    public static class PhysicsExtensions
    {
        public static Vec2 ToVec2(this Vector2 vector2) => new(vector2.X, vector2.Y);
        public static Vector2 ToVector2(this Vec2 vec2) => new(vec2.X, vec2.Y);
    }
}
