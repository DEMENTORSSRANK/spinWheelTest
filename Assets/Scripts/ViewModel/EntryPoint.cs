using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Model;
using Sources.Extensions;
using View;
using Random = UnityEngine.Random;

namespace ViewModel
{
    public class EntryPoint : MonoBehaviour
    {
        [Min(0)] [SerializeField] private int _startBalance = 1000;

        [Min(0)] [SerializeField] private int _startBet = 1;

        [Min(0)] [SerializeField] private int _stepBet = 10;

        [SerializeField] private FruitItem[] _fruits;

        [SerializeField] private RouletteView _rouletteView;

        [SerializeField] private WinLinesView _winLinesView;

        [SerializeField] private GameScreen _gameScreen;

        [SerializeField] private WinScreen _winScreen;

        private Roulette _roulette;

        private Balance _balance;

        private Bet _bet;

        private BalanceSaver _balanceSaver;

        private SpriteRenderer[][] _winElements;

        private List<FruitItem> _fruitsWithChances;

        private Sprite GetRandomSprite(Sprite exception)
        {
            FruitItem[] arrayWithoutExceptions = _fruitsWithChances.Where(x => x.Avatar != exception).ToArray(); 
            
            return arrayWithoutExceptions[Random.Range(0, arrayWithoutExceptions.Length)].Avatar;
        }

        private void CheckBetMaximumBalance()
        {
            if (_bet.Value > _balance.Value)
                _bet.Set(_balance.Value);
            
            if (_bet.Value == 0)
                _gameScreen.DeActiveSpinInteractable();
        }
        
        private void TryAddStepToBet()
        {
            if (_rouletteView.IsSpin)
                return;
            
            _bet.Add(_stepBet);

            CheckBetMaximumBalance();
        }

        public void TryRemoveStepFromBet()
        {
            if (_rouletteView.IsSpin)
                return;
            
            if (!_bet.TryTake(_stepBet))
                _bet.Reset();
        }

        private void UpdateRandomSprite(SpriteRenderer spriteRenderer) => spriteRenderer.sprite = GetRandomSprite(spriteRenderer.sprite);

        private void ShowResult()
        {
            SpriteRenderer[,] visibleItems = _rouletteView.GetVisibleItems();

            WinLine[] winLines = _roulette.CheckWins(visibleItems).ToArray();

            if (winLines.Length == 0)
            {
                print("No wins");

                return;
            }

            print($"Win {winLines.Length} line (s)");

            _winElements = new SpriteRenderer[winLines.Length][];

            for (int i = 0; i < winLines.Length; i++)
            {
                Vector2Int[] itemPositions = winLines[i].ItemPositions.ToArray();

                _winElements[i] = new SpriteRenderer[itemPositions.Length];

                for (int j = 0; j < itemPositions.Length; j++)
                    _winElements[i][j] = visibleItems[itemPositions[j].y, itemPositions[j].x];
            }

            _winLinesView.ShowWinAsync(_winElements, delegate
            {
                int win = _bet.CalculateWin(_winElements.SelectMany(x => x).Distinct()
                    .Select(x => _fruits.First(y => y.Avatar == x.sprite).Coefficient));

                _balance.Add(win);

                ShowWinScreen(win);
            });
        }

        private void ShowWinScreen(int winValue)
        {
            _winScreen.gameObject.SetActive(true);

            _winScreen.UpdateWinValue(winValue);
        }

        private void CloseWinScreen()
        {
            _winScreen.gameObject.SetActive(false);
        }

        private void TakeBetFromBalance()
        {
            _balance.TryTake(_bet.Value);
        }

        private void InitializeFruitsWithChances()
        {
            _fruitsWithChances = new List<FruitItem>(_fruits.Select(x => x.PercentsShow).Sum());

            foreach (var fruit in _fruits)
            {
                for (int i = 0; i < fruit.PercentsShow; i++)
                    _fruitsWithChances.Add(fruit);
            }

            ListExtensions.MixList(_fruitsWithChances);
        }

        private void WaitReturnInput()
        {
            StartCoroutine(WaitingInputReturn());
        }
        
        private void Awake()
        {
            _balanceSaver = new BalanceSaver();
            _balance = new Balance(_balanceSaver.TryGetSaved(out var savedBalance) ? savedBalance : _startBalance);
            _bet = new Bet(_startBet);
            _roulette = new Roulette();
            InitializeFruitsWithChances();
        }

        private void Start()
        {
            _gameScreen.UpdateBalance(_balance.Value);

            _gameScreen.UpdateBet(_bet.Value);

            CloseWinScreen();
            
            CheckBetMaximumBalance();
        }

        private void OnEnable()
        {
            _balance.ValueChanged += _balanceSaver.Save;
            _balance.ValueChanged += _gameScreen.UpdateBalance;
            _bet.ValueChanged += _gameScreen.UpdateBet;
            _gameScreen.OnSpinClicked += _rouletteView.SpinAsync;
            _rouletteView.UpdateRandomSprite += UpdateRandomSprite;
            _rouletteView.SpinStopped += ShowResult;
            _gameScreen.OnStepDownClicked += TryRemoveStepFromBet;
            _gameScreen.OnStepUpClicked += TryAddStepToBet;
            _winScreen.OnPressed += CloseWinScreen;
            _rouletteView.SpinStarted += TakeBetFromBalance;
            _rouletteView.SpinStarted += _gameScreen.DeActiveSpinInteractable;
            _rouletteView.SpinStopped += WaitReturnInput;
        }

        private void OnDisable()
        {
            _balance.ValueChanged -= _balanceSaver.Save;
            _balance.ValueChanged -= _gameScreen.UpdateBalance;
            _bet.ValueChanged -= _gameScreen.UpdateBet;
            _gameScreen.OnSpinClicked -= _rouletteView.SpinAsync;
            _rouletteView.UpdateRandomSprite -= UpdateRandomSprite;
            _rouletteView.SpinStopped -= ShowResult;
            _gameScreen.OnStepDownClicked -= TryRemoveStepFromBet;
            _gameScreen.OnStepUpClicked -= TryAddStepToBet;
            _winScreen.OnPressed -= CloseWinScreen;
            _rouletteView.SpinStarted -= TakeBetFromBalance;
            _rouletteView.SpinStarted -= _gameScreen.DeActiveSpinInteractable;
            _rouletteView.SpinStopped -= WaitReturnInput;
        }

        private IEnumerator WaitingInputReturn()
        {
            yield return new WaitUntil(() => !_winLinesView.IsView);
            
            _gameScreen.ActivateSpinInteractable();
            
            CheckBetMaximumBalance();
        }
    }
}