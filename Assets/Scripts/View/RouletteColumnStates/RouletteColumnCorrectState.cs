using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace View.RouletteColumnStates
{
    public class RouletteColumnCorrectState : ColumnState
    {
        private Transform _nearestToMiddle;

        private Transform _cachedColumnsParent;

        private const float Duration = 1f;

        public RouletteColumnCorrectState(Transform column) : base(column)
        {
            
        }

        public void SetNearestToMiddleItem(Transform item, Action completed)
        {
            _nearestToMiddle = item;

            _nearestToMiddle.parent = null;

            _cachedColumnsParent = Column.parent;

            Column.parent = _nearestToMiddle;

            TweenerCore<Vector3, Vector3, VectorOptions> tween = _nearestToMiddle.transform.DOMoveY(0, Duration)
                .SetSpeedBased().SetEase(Ease.InQuad);
            
            tween.onComplete += delegate
            {
                Column.parent = null;
                
                Column.parent = _cachedColumnsParent;

                _nearestToMiddle.parent = Column;
                
                Debug.Log("Completed");
                
                completed?.Invoke();
            };
        }
    }
}