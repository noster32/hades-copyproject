using UnityEngine;

[RequireComponent(typeof(CZagreusSpriteLoader))]
[RequireComponent(typeof(CSpriteAnimation))]
public class CZagreusAnimation : MonoBehaviour
{
    private CZagreusSpriteLoader _spriteLoader;
    private CSpriteAnimation _spriteAnimation;
    [SerializeField] CSpriteAnimation _vfxSpriteAnimation;

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _rotationSpeed = 800f;

    private Vector2 _characterTargetDiection;
    private Vector2 _characterCurrentDirection;

    private void Awake()
    {
        _spriteLoader = GetComponent<CZagreusSpriteLoader>();
        _spriteAnimation = GetComponent<CSpriteAnimation>();
    }

    private async void Start()
    {
        await _spriteLoader.InitializationTask;

        RegisterAnimation();
        _spriteAnimation.SetDefaultAnimation("idle");

        RegisterTransition();
        RegisterEvent();
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // 마우스 스크린 좌표를 월드 좌표로 변환
        //    Vector3 mouseScreenPos = Input.mousePosition;
        //    mouseScreenPos.z = -Camera.main.transform.position.z; // 카메라 Z 위치 보정
        //    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        //    mouseWorldPos.z = 0; // 2D 평면 설정
        //
        //    _spriteAnimation.ChangeDirection(mouseWorldPos);
        //
        //    Debug.DrawLine(transform.position, mouseWorldPos, Color.red, 10f);
        //}

        UpdateDirection();
    }

    #region 애니메이션 설정
    private void RegisterAnimation()
    {
        _spriteAnimation.AddSpriteAnimation("idle", true, 30f, _spriteLoader.ZagreusIdleSprites);
        _spriteAnimation.AddSpriteAnimation("run", true, 30f, _spriteLoader.ZagreusRunSprites);
        _spriteAnimation.AddSpriteAnimation("start", false, 30f, _spriteLoader.ZagreusStartSprites);
        _spriteAnimation.AddSpriteAnimation("stop", false, 30f, _spriteLoader.ZagreusStopSprites);
        _spriteAnimation.AddSpriteAnimation("dash", false, 30f, _spriteLoader.ZagreusDashSprites);

        _vfxSpriteAnimation.AddSpriteAnimation("dashVFX", false, 30f, _spriteLoader.ZagreusDashVFXSprites);
    }

    private void RegisterTransition()
    {
        _spriteAnimation.AddTransition("start", "run", true);
        _spriteAnimation.AddTransition("stop", "idle", true);
        _spriteAnimation.AddTransition("dash", "idle", true);
    } 

    private void RegisterEvent()
    {
    }

    #endregion

    #region 애니메이션 업데이트

    public void PlayAnimation(string name) => _spriteAnimation.PlayAnimation(name);
    public void PlayVFXAnimation(string name)
    {
        _vfxSpriteAnimation.SetDirection(_characterCurrentDirection);
        _vfxSpriteAnimation.PlayAnimation(name);
    } 
    public void SetTargetDirection(Vector2 dir) => _characterTargetDiection = dir;
    public void ResumeAnimation() => _spriteAnimation.ResumeAnimation();

    #endregion

    //애니메이션 방향 설정
    private void UpdateDirection()
    {
        float targetAngle = Mathf.Atan2(_characterTargetDiection.y, _characterTargetDiection.x) * Mathf.Rad2Deg;
        float currentAngle = Mathf.Atan2(_characterCurrentDirection.y, _characterCurrentDirection.x) * Mathf.Rad2Deg;

        float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
        float step = _rotationSpeed * Time.deltaTime;
        currentAngle += Mathf.Clamp(angleDiff, -step, step);

        Vector2 rawDirection = new Vector2(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad),
            Mathf.Sin(currentAngle * Mathf.Deg2Rad)
        );

        _characterCurrentDirection = rawDirection.sqrMagnitude > 0.01f ? rawDirection.normalized : Vector2.zero;
        _spriteAnimation.SetDirection(_characterCurrentDirection);
        
    }
}
