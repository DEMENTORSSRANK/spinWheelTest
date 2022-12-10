using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static Transform GetMostNearToPositionY(this IEnumerable<Transform> items, float positionY)
        {
            return items.OrderBy(x => Mathf.Abs(x.position.y - positionY)).First();
        }

        public static Transform GetMostBiggerPositionY(this IEnumerable<Transform> items)
        {
            return items.OrderBy(x => x.position.y).Last();
        }

        public static IEnumerable<Transform> GetMostNearToPositionY(this IEnumerable<Transform> items, float positionY, int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return items.OrderBy(x => Mathf.Abs(x.position.y - positionY)).Take(count);
        }
    }
}