using UnityEngine;

public class LocomotionState : BaseZagreusState
{
    public override void OnEnter()
    {
        base.OnEnter();
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
    public LocomotionState(CZagreusMovementController controller, CZagreusAnimation anim) : base(controller, anim)
    {

    }
}
