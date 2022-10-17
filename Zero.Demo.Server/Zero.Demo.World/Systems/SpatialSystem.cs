using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zero.Demo.World.Component;
using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public class SpatialSystem : ComponentSystem, IAddEvent<SpatialComponent>, IRemoveEvent<SpatialComponent>
    {
        [StructLayout(LayoutKind.Explicit, Size = 12)]
        public readonly struct SpatialEntry
        {
            [FieldOffset(0)]
            public readonly uint EntityId;
            [FieldOffset(4)]
            public readonly Vec2 Coordinates;

            public SpatialEntry(uint entityId, Vec2 coordinates)
            {
                EntityId = entityId;
                Coordinates = coordinates;
            }

            public static bool operator ==(SpatialEntry left, SpatialEntry right) => left.Equals(right);
            public static bool operator !=(SpatialEntry left, SpatialEntry right) => !(left == right);

            public override bool Equals(object obj)
            {
                if (obj is SpatialEntry other)
                {
                    return Equals(other);
                }
                return base.Equals(obj);
            }

            public bool Equals(SpatialEntry other)
            {
                return EntityId == other.EntityId;
            }

            public override int GetHashCode()
            {
                return (int)EntityId;
            }
        }

        private readonly int _bucketSize;
        private readonly Int2 _bucketCount;
        private readonly List<SpatialEntry>[] _buckets;
        private readonly float _bucketHypoHalf;

        public SpatialSystem(Int2 mapSize, int bucketSize)
        {
            _bucketSize = bucketSize;
            _bucketHypoHalf = bucketSize * MathF.Sqrt(2) * 0.5f;
            _bucketCount = (mapSize + (bucketSize - 1)) / bucketSize;
            _buckets = new List<SpatialEntry>[_bucketCount.X * _bucketCount.Y];
        }

        public void GetEntitiesWithin(Rect rect, List<SpatialEntry> entityResults)
        {
            var minBucket = (rect.Point / _bucketSize).Int2.Clamp(0, _bucketCount - 1);
            var maxBucket = (rect.Tr / _bucketSize).Int2.Clamp(0, _bucketCount - 1);
            for (int y = minBucket.Y; y <= maxBucket.Y; y++)
            {
                for (int x = minBucket.X; x <= maxBucket.X; x++)
                {
                    var bucket = GetBucket(y * _bucketCount.X + x);
                    if (bucket == null)
                    {
                        continue;
                    }

                    if (ContainsWholeBucket(rect, x, y, out var intersectsEdge)) // entire bucket fits in the radius, no need to check each coordinate
                    {
                        entityResults.AddRange(bucket);
                    }
                    else if (intersectsEdge) // check each entry for buckets that the radius edge intersects
                    {
                        var span = CollectionsMarshal.AsSpan(bucket);
                        for (int i = 0; i < span.Length; i++)
                        {
                            var entry = span[i];
                            if (rect.Contains(entry.Coordinates))
                            {
                                entityResults.Add(entry);
                            }
                        }
                    }
                }
            }
        }

        public void OnAdd(uint entityId, ref SpatialComponent component)
        {
            var bucketIndex = GetBucketIndex(component.Coordinates);
            var bucket = GetBucket(bucketIndex);
            bucket.Add(new SpatialEntry(entityId, component.Coordinates));
        }

        public void OnRemove(uint entityId, in SpatialComponent component)
        {
            var bucketIndex = GetBucketIndex(component.Coordinates);
            var bucket = GetBucket(bucketIndex);
            bucket.Remove(new SpatialEntry(entityId, component.Coordinates));
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((uint entityId, ref PositionComponent position, ref SpatialComponent spatial) =>
            {
                if (position.Coordinates == spatial.Coordinates)
                {
                    return;
                }

                var newBucketIndex = GetBucketIndex(position.Coordinates);
                var oldBucketIndex = GetBucketIndex(spatial.Coordinates);
                if (newBucketIndex != oldBucketIndex)
                {
                    var oldBucket = GetBucket(oldBucketIndex);
                    oldBucket.Remove(new SpatialEntry(entityId, spatial.Coordinates));

                    var newBucket = GetBucket(newBucketIndex);
                    newBucket.Add(new SpatialEntry(entityId, position.Coordinates));
                }

                spatial.Coordinates = position.Coordinates;
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ContainsWholeBucket(Rect rect, int bucketX, int bucketY, out bool intersectsEdge)
        {
            var bucketCoordsRect = new Rect(bucketX * _bucketSize + _bucketSize / 2f, bucketY * _bucketSize + _bucketSize / 2f, _bucketSize, _bucketSize);
            var intersects = rect.Intersects(bucketCoordsRect);
            var envelops = rect.Envelops(bucketCoordsRect);
            intersectsEdge = intersects && !envelops;
            return envelops;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<SpatialEntry> GetBucket(int index)
        {
            var bucket = _buckets[index];
            if (bucket == null)
            {
                bucket = new();
                _buckets[index] = bucket;
            }
            return bucket;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetBucketIndex(Vec2 coordinates)
        {
            var bucketCoordinates = (coordinates / _bucketSize).Int2.Clamp(0, _bucketCount - 1);
            return bucketCoordinates.Y * _bucketCount.X + bucketCoordinates.X;
        }
    }
}
