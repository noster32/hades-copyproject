using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CSpriteAnimation : MonoBehaviour
{
    #region Component References
    [SerializeField] private SpriteRenderer _spriteRenderer;
    #endregion

    #region Animation Data
    private Dictionary<string, SpriteAnimationNode> _spriteAnimNodes = new Dictionary<string, SpriteAnimationNode>();
    private string _defaultAnimationName;
    private string _currentAnimationName;
    private SpriteAnimationNode _currentAnimationNode;
    #endregion

    #region Direction Variables
    private Vector3 _targetVecDirection;
    private Vector3 _currentVecDirection;
    private float _currentAngle;
    private int _currentIndexDirection;
    private int _targetIndexDirection;
    #endregion

    #region Frame and Playback Control
    private int _currentFrameIndex;
    private bool _isAnimationPlaying;
    private bool _isPause;
    private bool _isOneShot;
    private uint _animationGeneration = 0;
    #endregion

    #region Cancellation Tokens
    private CancellationTokenSource _animationCts;
    private CancellationTokenSource _directionCts;
    #endregion

    private void OnDestroy()
    {
        _animationCts?.Cancel();
        _animationCts?.Dispose();
        _animationCts = null;

        _directionCts?.Cancel();
        _directionCts?.Dispose();
        _animationCts = null;
    }

    #region Animation Setup
    //스프라이트 애니메이션  추가
    public void AddSpriteAnimation(string name, bool loop, float rate, Sprite[][] sprites, 
                                   bool smoothDir = false, float rotSpeed = 800f)
    {
        if (_spriteAnimNodes.TryGetValue(name, out var node))
        {
#if UNITY_EDITOR
            Debug.LogError($"Sprite Animation '{node}' is Already assigned");
#endif
            return;
        }

        node = new SpriteAnimationNode(sprites, loop, rate, smoothDir, rotSpeed);
        _spriteAnimNodes.Add(name, node);
    }

    //기본 재생 애니메이션 설정
    public void SetDefaultAnimation(string name)
    {
        if (!_spriteAnimNodes.ContainsKey(name))
        {
#if UNITY_EDITOR
            Debug.LogError($"Sprite Animation '{name}' is not assigned");
#endif
            return;
        }
        _defaultAnimationName = name;
        PlayAnimation(_defaultAnimationName);
    }

    //Current Animation 설정
    //OneShot만 플레이할 경우 처음에 Current Node가 비어있을 수 있는 오류 방지
    /// <summary>
    /// Set to Current Animation but not playing
    /// </summary>
    public void SetCurrentAnimation(string name)
    {
        if (!_spriteAnimNodes.TryGetValue(name, out var node))
        {
#if UNITY_EDITOR
            Debug.LogError($"Sprite Animation '{node}' is not assigned");
#endif
            return;
        }

        _currentAnimationName = name;
        _currentAnimationNode = node;
    }

    //트랜지션 추가
    public void AddTransition(string from, string to, bool hasExitTime, IStatePredicate condition = null)
    {
        if (!_spriteAnimNodes.TryGetValue(from, out var nodeFrom))
        {
#if UNITY_EDITOR
            Debug.LogError($"Sprite Animation '{from}' is not assigned");
#endif
            return;
        }

        if (!_spriteAnimNodes.TryGetValue(to, out var nodeTo))
        {
#if UNITY_EDITOR
            Debug.LogError($"Sprite Animation '{to}' is not assigned");
#endif
            return;
        }

        nodeFrom.AddTransition(to, hasExitTime, condition);
    }

    //이벤트 추가
    public void AddEvent(string to, int frame, Action action)
    {
        if (!_spriteAnimNodes.TryGetValue(to, out var node))
        {
#if UNITY_EDITOR
            Debug.LogError($"Sprite Animation '{to}' is not assigned");
#endif
            return;
        }

        node.AddAnimationEvents(to, frame, action);
    }


    #endregion

    #region Transition

    //트랜지션 체크
    //조건이 맞을 경우 트랜지션 반환 그렇지 않으면 null 반환
    private ISpriteAnimTransition GetTransition()
    {
        if(_currentAnimationNode == null || _currentAnimationNode.Transitions.Count == 0)
            return null;

        foreach (var transition in _currentAnimationNode.Transitions)
        {
            bool conditionMet = transition.Condition?.Evaluate() ?? false;
            if (conditionMet && !transition.HasExitTime)
                return transition;
        }

        return null;
    }

    //ExitTime 트랜지션 체크, 위와 동일
    private void CheckExitTimeTransitions()
    {
        foreach (var transition in _currentAnimationNode.Transitions)
        {
            if (transition.HasExitTime && (transition.Condition?.Evaluate() ?? true))
            {
                PlayAnimation(transition.To);
                break;
            }
        }
    }

    #endregion

    #region Direction Handling

    //방향 설정
    public void SetDirection(Vector3 dir)
    {
        if (_currentAnimationNode == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Current animation node is null. Cannot change direction.");
#endif
            return;
        }

        _targetVecDirection = dir;

        if (!_currentAnimationNode.UseSmoothDirection)
            ConvertVectorToIndexDirection(_targetVecDirection);
    }

    //Index -> Index 방향 전환
    private void ConvertIndexToIndexDirection(string newAnimationName)
    {
        if (!_spriteAnimNodes.TryGetValue(newAnimationName, out SpriteAnimationNode newNode))
        {
#if UNITY_EDITOR
            Debug.LogError($"Animation {newAnimationName} not found in ConvertDirection.");
#endif
            return;
        }

        if (_currentAnimationNode == null)
        {
            _currentIndexDirection = 0;
            _targetIndexDirection = 0;
            return;
        }

        int newDirectionCount = newNode.Sprites.Length;

        if (newDirectionCount == 0)
        {
#if UNITY_EDITOR
            Debug.LogError("Direction count is zero in ConvertDirection.");
#endif
            return;
        }

        float newSegmentSize = 360f / newDirectionCount;
        int newDirection = Mathf.FloorToInt(_currentAngle / newSegmentSize) % newDirectionCount;

        _currentIndexDirection = newDirection;
        _targetIndexDirection = newDirection;
    }

    //Vector -> Index 방향 전환
    private void ConvertVectorToIndexDirection(Vector3 dir)
    {
        if (_currentAnimationNode == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Current animation node is null. Cannot change direction.");
#endif
            return;
        }

        if (dir == Vector3.zero)
            return;

        Vector3 direction = dir.normalized;
        _currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (_currentAngle < 0) _currentAngle += 360;

        float segmentSize = 360f / _currentAnimationNode.Sprites.Length;
        _targetIndexDirection = Mathf.FloorToInt(_currentAngle / segmentSize);

        if (_targetIndexDirection >= _currentAnimationNode.Sprites.Length)
        {
            int overDirection = _targetIndexDirection - _currentAnimationNode.Sprites.Length;
            _targetIndexDirection = overDirection;
        }

        _currentIndexDirection = _targetIndexDirection;
    }

    //애니메이션 방향을 부드럽게 회전 UniTask
    private async UniTaskVoid SmoothDirection()
    {
        _directionCts?.Cancel();
        _directionCts = new CancellationTokenSource();
        CancellationToken token = _directionCts.Token;

        float rotSpeed = _currentAnimationNode.RotateSpeed;

        while (!token.IsCancellationRequested)
        {
            float targetAngle = Mathf.Atan2(_targetVecDirection.y, _targetVecDirection.x) * Mathf.Rad2Deg;
            float currentAngle = Mathf.Atan2(_currentVecDirection.y, _currentVecDirection.x) * Mathf.Rad2Deg;

            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
            float step = rotSpeed * Time.deltaTime;
            currentAngle += Mathf.Clamp(angleDiff, -step, step);

            Vector2 rawDirection = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            );

            _currentVecDirection = rawDirection.sqrMagnitude > 0.01f ? rawDirection.normalized : Vector2.zero;
            ConvertVectorToIndexDirection(_currentVecDirection);

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    //부드럽게 회전 취소, 회전 중 바뀔수 있으니 target과 current각도 동일하게 설정
    private void CancelSmoothDirection()
    {
        _directionCts?.Cancel();
        _directionCts?.Dispose();
        _directionCts = null;

        _currentVecDirection = _targetVecDirection;
        ConvertVectorToIndexDirection(_currentVecDirection);
    }

    #endregion

    #region Animation Playback

    //애니메이션 재생 UniTask
    private async UniTaskVoid PlayAnimationAsync(string name, bool resetFrame = true)
    {
        if (!_spriteAnimNodes.TryGetValue(name, out var newNode))
        {
#if UNITY_EDITOR
            Debug.LogError($"Animation {name} not found!");
#endif
            return;
        }

        uint currentGeneration = ++_animationGeneration;

        _animationCts?.Cancel();
        _animationCts = new CancellationTokenSource();
        CancellationToken token = _animationCts.Token;

        _currentAnimationName = name;
        _currentAnimationNode = newNode;

        if(resetFrame)
            _currentFrameIndex = 0;

        ConvertIndexToIndexDirection(name);

        if (_currentAnimationNode.UseSmoothDirection)
            SmoothDirection().Forget();
        else
            CancelSmoothDirection();


        _isAnimationPlaying = true;

        try
        {
            while(!token.IsCancellationRequested)
            {
                //트랜지션 체크
                ISpriteAnimTransition transition = GetTransition();
                if (transition != null)
                {
                    PlayAnimation(transition.To);
                    break;
                }

                UpdateSpriteFrame();

                float delayPerFrame = 1f / _currentAnimationNode.FrameRate;
                int delayMs = (int)(delayPerFrame * 1000);

                await UniTask.Delay(delayMs, cancellationToken: token);

                _currentFrameIndex++;

                try
                {
                    var sprite = _currentAnimationNode.Sprites[_currentIndexDirection];
                    if (sprite == null)
                    {
                        Debug.Log("Sprite is null.");
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.LogError($"IndexOutOfRangeException 발생: {_currentIndexDirection}");
                }

                if (_currentFrameIndex >= _currentAnimationNode.Sprites[_currentIndexDirection].Length)
                {
                    CheckExitTimeTransitions();

                    if (!_currentAnimationNode.IsLoop || _isOneShot)
                        break;

                    _currentFrameIndex = 0;
                }
            }
        }
        catch (OperationCanceledException)
        {
            //Do nothing
        }
        finally
        {
            if (currentGeneration == _animationGeneration)
            {
                if (_isOneShot)
                {
                    _spriteRenderer.sprite = null;
                    _isOneShot = false;
                }
                _isAnimationPlaying = false;
            }
        }
    }

    private async UniTaskVoid PlayOneShotAnimationAsync(string name, Vector3? dir = null)
    {
        if (!_spriteAnimNodes.TryGetValue(name, out var newNode))
        {
#if UNITY_EDITOR
            Debug.LogError($"Animation {name} not found!");
#endif
            return;
        }

        uint currentGeneration = ++_animationGeneration;

        _animationCts?.Cancel();
        _animationCts = new CancellationTokenSource();
        CancellationToken token = _animationCts.Token;

        _currentAnimationName = name;
        _currentAnimationNode = newNode;

        _currentFrameIndex = 0;

        Vector3 direction = dir ?? Vector3.zero;

        if (direction == Vector3.zero)
            ConvertIndexToIndexDirection(_currentAnimationName);
        else
            ConvertVectorToIndexDirection(direction);

        _isOneShot = true;
        _isAnimationPlaying = true;

        try
        {
            while (!token.IsCancellationRequested)
            {
                //트랜지션 체크
                ISpriteAnimTransition transition = GetTransition();
                if (transition != null)
                {
                    PlayAnimation(transition.To);
                    break;
                }

                UpdateSpriteFrame();

                float delayPerFrame = 1f / _currentAnimationNode.FrameRate;
                int delayMs = (int)(delayPerFrame * 1000);

                await UniTask.Delay(delayMs, cancellationToken: token);

                _currentFrameIndex++;

                try
                {
                    var sprite = _currentAnimationNode.Sprites[_currentIndexDirection];
                    if (sprite == null)
                    {
                        Debug.Log("Sprite is null.");
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.LogError($"IndexOutOfRangeException 발생: {_currentIndexDirection}");
                }

                if (_currentFrameIndex >= _currentAnimationNode.Sprites[_currentIndexDirection].Length)
                {
                    CheckExitTimeTransitions();

                    if (!_currentAnimationNode.IsLoop || _isOneShot)
                        break;

                    _currentFrameIndex = 0;
                }
            }
        }
        catch (OperationCanceledException)
        {
            //Do nothing
        }
        finally
        {
            if (currentGeneration == _animationGeneration)
            {
                _spriteRenderer.sprite = null;
                _isOneShot = false;
                _isAnimationPlaying = false;
            }
        }
    }

    //애니메이션 재생
    public void PlayAnimation(string name, bool resetFrame = true)
    {
        PlayAnimationAsync(name, resetFrame).Forget();
    }

    //일회성 애니메이션 재생
    public void PlayOneShotAnimation(string name, Vector3? dir = null)
    {
        PlayOneShotAnimationAsync(name, dir).Forget();
    }

    //애니메이션 정지
    public void StopAnimation()
    {
        if (!_isAnimationPlaying)
            return;

        _isAnimationPlaying = false;
        _animationCts?.Cancel();
        _spriteRenderer.sprite = null;
    }

    //퍼즈
    public void PauseAnimation()
    {
        if(!_isAnimationPlaying || _isPause) 
            return;

        _isPause = true;
        _animationCts?.Cancel();
    }

    //재개
    public void ResumeAnimation()
    {
        if (!_isPause)
            return;

        _isPause = false;
        PlayAnimationAsync(_currentAnimationName, false).Forget();
    }

    //Sprite 프레임 업데이트
    private void UpdateSpriteFrame()
    {
       
        if (_currentAnimationNode == null || !_isAnimationPlaying) return;
        int currentDir = _currentIndexDirection;
        var sprites = _currentAnimationNode.Sprites[currentDir];

        if(_currentFrameIndex < 0 || _currentFrameIndex >= sprites.Length)
        {
#if UNITY_EDITOR
            Debug.LogError($"Invalid frame index: {_currentFrameIndex}");
#endif
            return;
        }

        _spriteRenderer.sprite = sprites[_currentFrameIndex];

        if(_currentAnimationNode.Events.TryGetValue(_currentFrameIndex, out var action))
        {
            action.Invoke();
        }
    }

    #endregion

    #region Helper Classes

    //애니메이션 기본 노드
    private class SpriteAnimationNode
    {
        public Sprite[][] Sprites { get; }
        public bool IsLoop { get; }
        public float FrameRate { get; }                 //초당 프레임
        public bool UseSmoothDirection { get; }
        public float RotateSpeed { get; }               //SmoothDIrection시 초당 회전각도

        private readonly HashSet<SpriteAnimTransition> _transitions;
        private readonly Dictionary<int, Action> _events;

        public IReadOnlyCollection<SpriteAnimTransition> Transitions => _transitions;
        public IReadOnlyDictionary<int, Action> Events => _events;

        public SpriteAnimationNode(Sprite[][] Sprites, bool loop, float frameRate, 
                                   bool smoothDir = false, float rotSpeed = 800f)
        {
            this.Sprites = Sprites;
            this.IsLoop = loop;
            this._transitions = new HashSet<SpriteAnimTransition>();
            this._events = new Dictionary<int, Action>();
            this.FrameRate = frameRate;
            this.UseSmoothDirection = smoothDir;
            this.RotateSpeed = rotSpeed;
        }

        public void AddTransition(string to, bool hasExitTime, IStatePredicate condition = null)
        {
            _transitions.Add(new SpriteAnimTransition(to, hasExitTime, condition));
        }

        public void AddAnimationEvents(string to, int frame, Action action)
        {
            _events.Add(frame, action);
        }
    }
    #endregion

}
