public class TriggerPredicate : IStatePredicate
{
    private bool _triggered;
    public void Trigger() => _triggered = true;
    public bool Evaluate() => _triggered ? !(_triggered = false) : false;
}
