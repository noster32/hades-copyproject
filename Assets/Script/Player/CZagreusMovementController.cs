using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

//TODO: 움직이는 애니메이션 재생, FSM 설정

//TODO: 대쉬 -> 움직임(애니메이션은 프레임 고정) -> 움직임 끝나면 대쉬 애니메이션 재생
//그 사이에 쿨다운 해제하면 될듯?

public class CZagreusMovementController : MonoBehaviour
{
    private Rigidbody2D rbody2D;

    [SerializeField] private float moveSpeed = 1f;

    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCooldown = 1f;

    private Vector2 _moveDirection;
    private Vector2 _dashDirection;

    private bool _canDash = true;

    public bool IsMoving { get; private set; } = false;
    public bool IsDashing { get; private set; } = false;

    private void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(_moveDirection != Vector2.zero)
            IsMoving = true;
        else
            IsMoving = false;
    }

    public void PlayerMoveUpdate()
    {
        Vector2 currentPos = rbody2D.position;
        Vector2 movement = _moveDirection * moveSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        rbody2D.MovePosition(newPos);
    }

    private async UniTaskVoid DashAsync()
    {
        _canDash = false;
        IsDashing = true;

        Vector2 dashMovement = _dashDirection.normalized * _dashSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < _dashDuration)
        {
            rbody2D.MovePosition(rbody2D.position + dashMovement * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            await UniTask.WaitForFixedUpdate();
        }

        IsDashing = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_dashCooldown));
        _canDash = true;
    }

    public void TriggerDash()
    {
        if(_canDash)
        {
            DashAsync().Forget();
        }
    }

    public void SetMoveDirection(Vector2 dir) => _moveDirection = dir;
    public void SetDashDirection(Vector2 dir) => _dashDirection = dir;
}
