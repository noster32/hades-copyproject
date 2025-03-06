public class SpriteAnimTransition : ISpriteAnimTransition
{
    public string To { get; }
    public bool HasExitTime { get; }
    public IStatePredicate Condition { get; }

    public SpriteAnimTransition(string to, bool hasExitTime)
    {
        this.To = to;
        this.HasExitTime = hasExitTime;
        this.Condition = null;
    }

    public SpriteAnimTransition(string to, bool hasExitTime, IStatePredicate condition)
    {
        this.To = to;
        this.HasExitTime = hasExitTime;
        this.Condition = condition;
    }
}
