using UnityEngine;

[RequireComponent(typeof(CZagreusSpriteLoader))]
[RequireComponent(typeof(CSpriteAnimation))]
public class CZagreusAnimation : MonoBehaviour
{
    private CZagreusSpriteLoader _spriteLoader;
    private CSpriteAnimation _spriteAnimation;
    [SerializeField] CSpriteAnimation _vfxSpriteAnimation;

    private Vector2 _targetDirection;

    private void Awake()
    {
        _spriteLoader = GetComponent<CZagreusSpriteLoader>();
        _spriteAnimation = GetComponent<CSpriteAnimation>();
    }

    private async void Start()
    {
        await _spriteLoader.InitializationTask;

        RegisterAnimations();
        _spriteAnimation.SetDefaultAnimation("idle");

        RegisterTransitions();
        RegisterEvents();
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
    }

    #region Register
    private void RegisterAnimations()
    {
        _spriteAnimation.AddSpriteAnimation("idle", true, 30f, _spriteLoader.ZagreusIdleSprites);
        _spriteAnimation.AddSpriteAnimation("run", true, 30f, _spriteLoader.ZagreusRunSprites, true);
        _spriteAnimation.AddSpriteAnimation("start", false, 30f, _spriteLoader.ZagreusStartSprites, true);
        _spriteAnimation.AddSpriteAnimation("stop", false, 30f, _spriteLoader.ZagreusStopSprites);
        _spriteAnimation.AddSpriteAnimation("dash", false, 30f, _spriteLoader.ZagreusDashSprites);

        _vfxSpriteAnimation.AddSpriteAnimation("dashVFX", false, 30f, _spriteLoader.ZagreusDashVFXSprites);
    }

    private void RegisterTransitions()
    {
        _spriteAnimation.AddTransition("start", "run", true);
        _spriteAnimation.AddTransition("stop", "idle", true);
        _spriteAnimation.AddTransition("dash", "idle", true);
    } 

    private void RegisterEvents()
    {
    }

    #endregion

    #region Animation Update

    public void PlayAnimation(string name) => _spriteAnimation.PlayAnimation(name);
    public void StopVFXAnimation() => _vfxSpriteAnimation.StopAnimation();
    public void PlayVFXAnimation(string name)
    {
        _vfxSpriteAnimation.PlayOneShotAnimation(name, _targetDirection);
    }

    public void SetTargetDirection(Vector2 dir)
    {
        _targetDirection = dir;
        _spriteAnimation.SetDirection(_targetDirection);
    }

    public void ResumeAnimation() => _spriteAnimation.ResumeAnimation();

    #endregion
}
