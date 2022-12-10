using UnityEngine;

namespace View.RouletteColumnStates
{
    public class RouletteColumnMovingState : ColumnState
    {
        private float _speed;
        
        public RouletteColumnMovingState(Transform column) : base(column)
        {
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public override void OnTick()
        {
            Column.transform.position += Vector3.down * (_speed * Time.deltaTime);
        }
    }
}