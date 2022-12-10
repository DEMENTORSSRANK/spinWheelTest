using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateGeneratorStates.Scripts.States
{
    public class StatesControl
    {
        public List<IState> MyStates { get; }

        public StatesControl()
        {
            MyStates = new List<IState>();
        }

        public IState GetState<T>() where T : class
        {
            var type = typeof(T);

            if (MyStates.All(x => x.GetType() != type))
                throw new Exception($"Cant get state of {type.Name}");

            return MyStates.Find(x => x.GetType() == type);
        }

        public void AddNewState(IState state)
        {
            if (MyStates.Count > 0)
            {
                Debug.Log(MyStates[0]);
            }

            if (MyStates.Any(x => x.GetType() == state.GetType()))
            {
                Debug.LogWarning($"Cant add {state.GetType()} in states control, type of this already exists");

                return;
            }
            
            MyStates.Add(state);
        }

        public void AddNewStates(IEnumerable<IState> states)
        {
            states.ToList().ForEach(AddNewState);
        }
    }
}