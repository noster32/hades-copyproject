using UnityEngine;

public class DashState : BaseZagreusState
{

    public override void OnEnter()
    {
        base.OnEnter();

#if UNITY_EDITOR
        Debug.Log("Dash State");
#endif

        _zagreusController.TriggerDash();
        _zagreusAnimation.PlayAnimation("dash");
        _zagreusAnimation.PlayVFXAnimation("dashVFX");
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

        _zagreusAnimation.ResumeAnimation();
    }

    public DashState(CZagreusMovementController controller, CZagreusAnimation anim) : base(controller, anim)
    {

    }

}
