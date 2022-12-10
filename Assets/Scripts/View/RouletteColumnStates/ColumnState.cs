using System;
using StateGeneratorStates.Scripts.States;
using UnityEngine;

namespace View.RouletteColumnStates
{
    public abstract class ColumnState : IState
    {
        protected readonly Transform Column;

        protected ColumnState(Transform column)
        {
            Column = column ? column : throw new ArgumentNullException(nameof(column));
        }

        public virtual void OnTick()
        {
            
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }
    }
}