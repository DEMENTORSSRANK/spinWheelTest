using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace View
{
    [Serializable]
    public class RouletteItemsUpdater
    {
        [SerializeField] private SpriteRenderer[] _items;

        private float _itemOffset;

        private float _positionYToUpdate;

        private Queue<Transform> _movingItems;

        public event Action<SpriteRenderer> UpdateRandomSprite;

        public bool IsUpdating { get; set; }

        public Transform GetMostNearestToMiddle()
        {
            return _movingItems.Where(x => x.position.y >= 0).GetMostNearToPositionY(0);
        }

        public IEnumerable<SpriteRenderer> GetMiddleItems(int count)
        {
            return _movingItems.GetMostNearToPositionY(0, count).Select(x => _items.First(y => y.transform == x))
                .OrderBy(x => x.transform.position.y).Reverse();
        }

        public void Initialize()
        {
            _itemOffset = Vector2.Distance(_items[1].transform.position, _items[0].transform.position);

            _movingItems = new Queue<Transform>(_items.Select(x => x.transform).Reverse());

            _positionYToUpdate = _movingItems.Peek().position.y - _itemOffset;

            foreach (var item in _items)
                UpdateRandomSprite?.Invoke(item);
        }

        public void Tick()
        {
            if (!IsUpdating)
                return;

            if (_movingItems.Peek().position.y <= _positionYToUpdate)
                UpdateItems();
        }

        private void UpdateItems()
        {
            Transform taken = _movingItems.Dequeue();

            taken.position = _movingItems.GetMostBiggerPositionY().position + Vector3.up * _itemOffset;

            _movingItems.Enqueue(taken);

            UpdateRandomSprite?.Invoke(_items.First(x => x.transform == taken));
        }
    }
}