public interface IState
{
    void OnEnter();
    void Update();
    void FixedUpdate();
    void LateUpdate();
    void OnExit();
    void SetPreviousState(IState prevState);
    IState PreviousState { get; }
    StateMachine StateMachine { get; }
}
