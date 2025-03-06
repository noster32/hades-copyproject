using System;
using Unity.VisualScripting;
using UnityEngine;

public class CZagreusStateMachine : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }

    private CZagreusMovementController _zagreusController;
    private CZagreusAnimation _zagreusAnimation;

    private IdleState _idleState;
    private MoveState _moveState;
    private DashState _dashState;
   

    private void Start()
    {
        StateMachine = new StateMachine();
        _zagreusController = GetComponent<CZagreusMovementController>();
        _zagreusAnimation = GetComponentInChildren<CZagreusAnimation>();

        _idleState = new IdleState(_zagreusController, _zagreusAnimation);
        _moveState = new MoveState(_zagreusController, _zagreusAnimation);
        _dashState = new DashState(_zagreusController, _zagreusAnimation);

        RegisterState();
        StateMachine.SetState(_idleState);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        StateMachine.LateUpdate();
    }
    private void RegisterState()
    {
        Any(_idleState, new FuncStatePredicate(ReturnToIdleState));
        
        //Move
        At(_idleState, _moveState, new FuncStatePredicate(() => _zagreusController.IsMoving));
        At(_dashState, _moveState, new FuncStatePredicate(() => _zagreusController.IsMoving && !_zagreusController.IsDashing));

        //Dash
        At(_idleState, _dashState, new FuncStatePredicate(() => _zagreusController.IsDashing));
        At(_moveState, _dashState, new FuncStatePredicate(() => _zagreusController.IsDashing));
    }

    bool ReturnToIdleState()
    {
        return !_zagreusController.IsMoving
            && !_zagreusController.IsDashing;
    }

    private void At(IState from, IState to, IStatePredicate condition) => StateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IStatePredicate condition) => StateMachine.AddAnyTransition(to, condition);
}
