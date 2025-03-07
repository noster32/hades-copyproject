using UnityEngine;

public class MoveState : BaseZagreusState
{
    public override void OnEnter()
    {
        base.OnEnter();

        switch (PreviousState)
        {
            case DashState dashState:
                _zagreusAnimation.StopVFXAnimation();
                break;
            default:
                break;
        }

        _zagreusAnimation.PlayAnimation("start");

#if UNITY_EDITOR
        Debug.Log("Move State");
#endif
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _zagreusController.PlayerMoveUpdate();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public MoveState(CZagreusMovementController controller, CZagreusAnimation anim) : base(controller, anim)
    {
    }
}
