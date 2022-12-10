using System;
using UnityEngine;

namespace View.RouletteColumnStates
{
    public class RouletteColumnStoppingMove : ColumnState
    {
        private const float Change = 4f;

        private const float StopSpeed = 1f;
        
        public float Speed { get; set; }

        public event Action Stopped;
        
        public RouletteColumnStoppingMove(Transform column) : base(column)
        {
        }

        public override void OnTick()
        {
            Speed = Mathf.Clamp(Speed - Change * Time.deltaTime, 0, Speed);
            
            Column.transform.position += Vector3.down * (Speed * Time.deltaTime);
            
            if (Speed <= StopSpeed)
                Stopped?.Invoke();
        }
    }
}