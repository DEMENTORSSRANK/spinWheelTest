using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    public class RouletteView : MonoBehaviour
    {
        [SerializeField] private RouletteColumnView[] _rouletteColumnViews;

        [Min(0)] [SerializeField] private int _delayColumnStartMilliseconds = 300;
        
        [Min(0)] [SerializeField] private int _delayColumnStopMilliseconds = 300;

        [SerializeField] private int _timeSpinInMilliseconds = 5000;

        [SerializeField] private Vector2Int _visibleSize = new Vector2Int(3, 3);
        
        public bool IsSpin { get; private set; }

        public event Action<SpriteRenderer> UpdateRandomSprite;

        public event Action SpinStarted;
        
        public event Action SpinStopped;

        public async void SpinAsync()
        {
            if (IsSpin)
                throw new InvalidOperationException("Roulette is spining now");
            
            SpinStarted?.Invoke();
            
            IsSpin = true;
            
            foreach (var rouletteColumnView in _rouletteColumnViews)
            {
                rouletteColumnView.StartMoveBottom();

                await Task.Delay(_delayColumnStartMilliseconds);
            }

            await Task.Delay(_timeSpinInMilliseconds);
            
            foreach (var rouletteColumnView in _rouletteColumnViews)
            {
                rouletteColumnView.StopMove();

                await Task.Delay(_delayColumnStopMilliseconds);
            }

            StartCoroutine(WaitSpinStopping());
        }

        public SpriteRenderer[,] GetVisibleItems()
        {
            SpriteRenderer[,] result = new SpriteRenderer[_visibleSize.x, _visibleSize.y];

            for (int columnIndex = 0; columnIndex < result.GetLength(1); columnIndex++)
            {
                SpriteRenderer[] columnItems = _rouletteColumnViews[columnIndex].GetMiddleItems().ToArray();

                for (int rowIndex = 0; rowIndex < result.GetLength(0); rowIndex++)
                    result[rowIndex, columnIndex] = columnItems[rowIndex];
            }

            return result;
        }

        private void OnEnable()
        {
            foreach (var itemView in _rouletteColumnViews)
            {
                itemView.UpdateRandomSprite += delegate(SpriteRenderer spriteRenderer)
                {
                    UpdateRandomSprite?.Invoke(spriteRenderer);
                };
            }
        }

        private IEnumerator WaitSpinStopping()
        {
            yield return new WaitUntil(() => _rouletteColumnViews.All(x => !x.IsMoving));

            IsSpin = false;
            
            SpinStopped?.Invoke();
            
            print("Spin stopped");
        }
    }
}