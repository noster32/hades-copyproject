public class BaseZagreusState : IState
{
    protected readonly CZagreusMovementController _zagreusController;
    protected readonly CZagreusAnimation _zagreusAnimation;

    public IState PreviousState => _previousState;
    private IState _previousState;

    public StateMachine StateMachine => _stateMachine;
    private StateMachine _stateMachine;
    private CZagreusMovementController controller;
    private CZagreusAnimation anim;

    protected BaseZagreusState(CZagreusMovementController controller, CZagreusAnimation anim)
    {
        this._zagreusController = controller;
        this._zagreusAnimation = anim;
    }

    public virtual void Update() { /*Do Nothing;*/ }
    public virtual void FixedUpdate() { /*Do Nothing;*/ }
    public virtual void LateUpdate() { /*Do Nothing;*/ }
    public virtual void OnEnter() { /*Do Nothing;*/ }
    public virtual void OnExit() { /*Do Nothing;*/ }

    public void SetPreviousState(IState prevState)
    {
        _previousState = prevState;
    }

}
