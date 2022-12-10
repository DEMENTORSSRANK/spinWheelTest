namespace StateGeneratorStates.Scripts.States
{
    public interface IState
    {
        void OnTick();

        void OnEnter();

        void OnExit();
    }
}
