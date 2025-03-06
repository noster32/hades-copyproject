using System;

public class FuncStatePredicate : IStatePredicate
{
    private readonly Func<bool> func;

    public FuncStatePredicate(Func<bool> func)
    {
        this.func = func;
    }

    public bool Evaluate() => func.Invoke();
}
