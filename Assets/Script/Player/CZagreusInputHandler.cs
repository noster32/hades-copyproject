using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CZagreusInputHandler : MonoBehaviour
{
    private CZagreusMovementController _zagreusController;
    private CZagreusAnimation _zagreusAnimation;

    private Vector2 _movementInput;
    private Vector2 _targetDirectionInput;

    private void Start()
    {
        _zagreusController = GetComponent<CZagreusMovementController>();
        _zagreusAnimation = GetComponentInChildren<CZagreusAnimation>();
    }

    private void Update()
    {
       
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = Vector2.ClampMagnitude(context.ReadValue<Vector2>(), 1);
        _zagreusController.SetMoveDirection(_movementInput);

        if (_movementInput != Vector2.zero)
        {
            _targetDirectionInput = _movementInput;
        }
        _zagreusAnimation.SetTargetDirection(_targetDirectionInput);

        switch (context)
        {
            case { phase: InputActionPhase.Started }:
#if UNITY_EDITOR
                Debug.Log("Move Started");
#endif
                _zagreusAnimation.PlayAnimation("start");
                break;
            case { phase: InputActionPhase.Canceled }:
                _zagreusAnimation.PlayAnimation("stop");
                break;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case { phase: InputActionPhase.Started }:
#if UNITY_EDITOR
                Debug.Log("Dash Started");
#endif
                Vector2 dashDir = _movementInput;

                if (dashDir == Vector2.zero && _targetDirectionInput == Vector2.zero)
                    dashDir = Vector2.right;
                else if (dashDir == Vector2.zero && _targetDirectionInput != Vector2.zero)
                    dashDir = _targetDirectionInput;
#if UNITY_EDITOR
                Debug.Log($"Dash Direction: {dashDir}");
#endif
                _zagreusController.SetDashDirection(dashDir);
                _zagreusController.TriggerDash();
                break;
        }
    }
}
