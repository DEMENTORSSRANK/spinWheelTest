using System;
using UnityEngine;

namespace View.RouletteColumnStates
{
    public class RouletteColumnStartingMove : ColumnState
    {
        private float _currentSpeed;

        private const float SpeedChange = 15;

        private const float ToMoveSpeed = 10;

        public event Action<float> StartedMoveWithSpeed;

        public RouletteColumnStartingMove(Transform column) : base(column)
        {
        }

        public override void OnEnter()
        {
            _currentSpeed = 0;
        }

        public override void OnTick()
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed + SpeedChange * Time.deltaTime, _currentSpeed, ToMoveSpeed);
            
            Column.transform.position += Vector3.down * (_currentSpeed * Time.deltaTime);
            
            if (_currentSpeed >= ToMoveSpeed)
                StartedMoveWithSpeed?.Invoke(ToMoveSpeed);
        }
    }
}