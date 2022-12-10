using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    public readonly struct WinLine
    {
        private readonly Vector2Int[] _itemPositions;

        public IEnumerable<Vector2Int> ItemPositions => _itemPositions;
        
        public WinLine(IEnumerable<Vector2Int> itemPositions)
        {
            _itemPositions = itemPositions.ToArray();
        }
    }
}