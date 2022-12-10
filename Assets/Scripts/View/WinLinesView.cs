using System;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace View
{
    public class WinLinesView : MonoBehaviour
    {
        [SerializeField] private GameObject _trailPrefab;

        [SerializeField] private float _durationLineView = 2;

        [SerializeField] private int _delayLineViewMilliSeconds = 200;

        [SerializeField] private int _prevDelayWinView = 200;

        public event Action ViewCompleted;

        public event Action ViewStarted;
        
        public bool IsView { get; private set; }
        
        public async void ShowWinAsync(SpriteRenderer[][] items, Action completed = null)
        {
            IsView = true;
            
            ViewStarted?.Invoke();
            
            await Task.Delay(_prevDelayWinView);

            foreach (SpriteRenderer[] item in items)
            {
                await ShowWinAsync(item);

                await Task.Delay(_delayLineViewMilliSeconds);
            }

            IsView = false;
            
            completed?.Invoke();
            
            ViewCompleted?.Invoke();
        }

        private async Task ShowWinAsync(SpriteRenderer[] items)
        {
            GameObject created = Instantiate(_trailPrefab, items.First().transform.position, Quaternion.identity);

            items.ToList().ForEach(x =>
            {
                x.transform.DOShakeScale(_durationLineView, 1, 5);
            });
            
            await created.transform.DOMove(items.Last().transform.position, _durationLineView).AsyncWaitForCompletion();
            
            await created.transform.DOMove(items.First().transform.position, _durationLineView).AsyncWaitForCompletion();
        }
    }
}