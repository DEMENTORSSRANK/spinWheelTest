using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private TmpTextValueView _winValueView;

        [SerializeField] private Button _pressScreen;

        public event Action OnPressed;
        
        public void UpdateWinValue(int winValue)
        {
            _winValueView.UpdateView(winValue);
        }

        private void Start()
        {
            _pressScreen.onClick.AddListener(delegate
            {
                OnPressed?.Invoke();
            });
        }
    }
}