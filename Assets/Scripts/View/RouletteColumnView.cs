using System;
using System.Collections.Generic;
using StateGeneratorStates.Scripts.States;
using UnityEngine;
using View.RouletteColumnStates;

namespace View
{
    [Serializable]
    public class RouletteColumnView : MonoBehaviour
    {
        [SerializeField] private RouletteItemsUpdater _rouletteItemsUpdater;

        [SerializeField] private int _middleCount = 3;

        private StateMachine _stateMachine = new StateMachine();

        private RouletteColumnIdle _idle;

        private RouletteColumnStartingMove _startingMove;

        private RouletteColumnMovingState _moving;

        private RouletteColumnStoppingMove _stoppingMove;

        private RouletteColumnCorrectState _correct;

        public bool IsMoving => _stateMachine.CurrentState != _idle;

        public event Action<SpriteRenderer> UpdateRandomSprite;

        public IEnumerable<SpriteRenderer> GetMiddleItems()
        {
            return _rouletteItemsUpdater.GetMiddleItems(_middleCount);
        }
        
        public void StartMoveBottom()
        {
            if (IsMoving)
                throw new InvalidOperationException("Column is moving now");

            Transform column = transform;
            
            _idle = new RouletteColumnIdle(column);
            
            _startingMove = new RouletteColumnStartingMove(column);
            
            _moving = new RouletteColumnMovingState(column);
            
            _stoppingMove = new RouletteColumnStoppingMove(column);
            
            _correct = new RouletteColumnCorrectState(column);

            _startingMove.StartedMoveWithSpeed += delegate(float speed)
            {
                _moving.SetSpeed(speed);
                
                _stoppingMove.Speed = speed;
                
                _stateMachine.SetState(_moving);
            };
            
            _stateMachine.SetState(_startingMove);

            _rouletteItemsUpdater.IsUpdating = true;
        }

        public void StopMove()
        {
            if (!IsMoving)
                throw new InvalidOperationException("Column is not moving now");

            if (_stateMachine.CurrentState == _stoppingMove)
                return;

            _stateMachine.SetState(_stoppingMove);
            
            _stoppingMove.Stopped += delegate
            {
                _stateMachine.SetState(_correct);
                
                _correct.SetNearestToMiddleItem(_rouletteItemsUpdater.GetMostNearestToMiddle(), delegate
                {
                    _stateMachine.SetState(_idle);

                    _rouletteItemsUpdater.IsUpdating = false;
                });
            };
        }

        private void OnEnable()
        {
            _rouletteItemsUpdater.UpdateRandomSprite += delegate(SpriteRenderer spriteRenderer)
            {
                UpdateRandomSprite?.Invoke(spriteRenderer);
            };
        }

        private void Start()
        {
            _idle = new RouletteColumnIdle(transform);
            
            _rouletteItemsUpdater.Initialize();
            
            _stateMachine.SetState(_idle);
        }

        private void Update()
        {
            _stateMachine.CurrentState.OnTick();
            
            _rouletteItemsUpdater.Tick();
        }
    }
}