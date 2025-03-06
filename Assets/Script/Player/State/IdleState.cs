using UnityEngine;

public class IdleState : BaseZagreusState
{
    public override void OnEnter()
    {
        base.OnEnter();

#if UNITY_EDITOR
        Debug.Log("Idle State");
#endif

        switch (PreviousState)
        {
            case MoveState moveState:
                _zagreusAnimation.PlayAnimation("stop");
                break;
            default:
                break;
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }


    public IdleState(CZagreusMovementController controller, CZagreusAnimation anim) : base(controller, anim)
    {
    }

}
