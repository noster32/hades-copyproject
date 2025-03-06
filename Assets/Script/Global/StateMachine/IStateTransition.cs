public interface IStateTransition
{
    IState to { get; }
    IStatePredicate condition { get; }
}
