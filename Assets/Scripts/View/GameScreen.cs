using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private Button _spin;

        [SerializeField] private TmpTextValueView _betView;

        [SerializeField] private TmpTextValueView _balanceView;

        [SerializeField] private Button _stepUp;

        [SerializeField] private Button _stepDown;

        public event Action OnSpinClicked;

        public event Action OnStepUpClicked;

        public event Action OnStepDownClicked;

        public void UpdateBet(int bet)
        {
            _betView.UpdateView(bet);
        }

        public void DeActiveSpinInteractable()
        {
            _spin.interactable = false;
        }

        public void ActivateSpinInteractable()
        {
            _spin.interactable = true;
        }
        
        public void UpdateBalance(int balance)
        {
            _balanceView.UpdateView(balance);
        }

        private void Start()
        {
            _spin.onClick.AddListener(delegate
            {
                OnSpinClicked?.Invoke();
            });
            
            _stepUp.onClick.AddListener(delegate
            {
                OnStepUpClicked?.Invoke();
            });
            
            _stepDown.onClick.AddListener(delegate
            {
                OnStepDownClicked?.Invoke();
            });
        }
    }
}