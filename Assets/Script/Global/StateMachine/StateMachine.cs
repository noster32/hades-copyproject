using System;
using System.Collections.Generic;

public class StateMachine
{
    private StateNode current;
    private Dictionary<Type, StateNode> nodes = new Dictionary<Type, StateNode>();
    private HashSet<IStateTransition> anyTransition = new HashSet<IStateTransition>();

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
            ChangeState(transition.to);

        current.state?.Update();
    }

    public void FixedUpdate()
    {
        current.state?.FixedUpdate();
    }

    public void LateUpdate()
    {
        current.state?.LateUpdate();
    }

    public void SetState(IState state)
    {
        var stateType = state.GetType();

        if (!nodes.ContainsKey(stateType))
        {
            StateNode node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        current = nodes[state.GetType()];
        current.state?.OnEnter();
    }

    private void ChangeState(IState state)
    {
        if (state == current.state || state == current.state) 
            return;

        var previousState = current.state;
        var nextState = nodes[state.GetType()].state;

        previousState?.OnExit();
        nextState.SetPreviousState(previousState);
        nextState?.OnEnter();

        current = nodes[state.GetType()];
    }

    private IStateTransition GetTransition()
    {
        foreach (var transition in anyTransition)
        {
            if (transition.condition.Evaluate())
                return transition;
        }

        foreach (var transition in current.transitions)
        {
            if (transition.condition.Evaluate())
                return transition;
        }

        return null;
    }

    public void AddTransition(IState from, IState to, IStatePredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).state, condition);
    }

    public void AddAnyTransition(IState to, IStatePredicate condition)
    {
        anyTransition.Add(new StateTransition(GetOrAddNode(to).state, condition));
    }

    private StateNode GetOrAddNode(IState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }

    public IState GetCurrentState() => current.state;

    private class StateNode
    {
        public IState state { get; }
        public HashSet<IStateTransition> transitions { get; }

        public StateNode(IState state)
        {
            this.state = state;
            transitions = new HashSet<IStateTransition>();
        }

        public void AddTransition(IState to, IStatePredicate condition)
        {
            transitions.Add(new StateTransition(to, condition));
        }
    }
}
