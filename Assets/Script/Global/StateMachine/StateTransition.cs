public class StateTransition : IStateTransition
{
    public IState to { get; }

    public IStatePredicate condition { get; }

    public StateTransition(IState to, IStatePredicate condition)
    {
        this.to = to;
        this.condition = condition;
    }
}