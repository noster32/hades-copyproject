public interface ISpriteAnimTransition
{
    string To { get; }
    bool HasExitTime { get; }
    IStatePredicate Condition { get; }
}
