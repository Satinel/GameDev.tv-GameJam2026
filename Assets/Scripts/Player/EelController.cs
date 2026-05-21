using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelController : MonoBehaviour, IElectrifiable
{
    public static event Action<AudioClip, float> OnEelHurtSFX;
    public static event Action<AudioClip, float> OnEelElectrifiedSFX;
    public static event Action<int> OnEelHealthChange;
    public static event Action OnEelDefeat;

    [SerializeField] int _health = 3;
    [SerializeField] float _moveSpeed = 5f, _retractSpeed = 7.5f;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] EelSegment _segmentPrefab, _tailPrefab;
    [SerializeField] float _segmentSize = 1;
    [SerializeField] float _bufferSpace = 0.25f;
    [SerializeField] Animator _animator;
    [SerializeField] float _elecDelay = 0.1f;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _hurtSFX, _elecSFX, _retractSFX;
    [SerializeField] float _hurtVol = 1f, _elecVol = 1f, _retractVol = 1f;
    [SerializeField] float _retractStartPitch = 0.5f, _relocateMaxPitch = 1.5f, _retractPitchIncrease = 0.1f;

    [SerializeField] IntReferenceSO _totalFishEaten;

    WaitForSeconds _elecDelayWait = new(0.01f);

    Vector2 _currentDirection = Vector2.zero;
    bool _isRetracting, _isElectrified, _fullRetract, _isRelocating;
    bool _isPlayingRetractSFX, _isPlayingRelocateSFX;
    bool _isLevelStarted, _isGamePaused, _isDefeated;

    Vector2 _lastSegmentPosition;
    EelSegment _tail;
    List<EelSegment> _segments = new();
    Coroutine _currentCoroutine;
    EelHome _currentHome;

    static readonly int ELEC_HASH = Animator.StringToHash("Electrified");
    static readonly int MOVE_HASH = Animator.StringToHash("Moving");

    void Awake()
    {
        InputManager.OnMoveAction += GetMoveValue;
        InputManager.OnActAction += SetRetract;

        VolumeControl.OnPauseStateChanged += TogglePausedState;

        EelSegment.OnEelSegmentAttacked += HandleAttack;
        Pemming.OnPemmingDefeat += HandleDefeat;

        LevelManager.OnLevelStarted += SetLevelStarted;
        LevelManager.OnLevelFinished += SetLevelFinished;
    }

    void OnDestroy()
    {
        InputManager.OnMoveAction -= GetMoveValue;
        InputManager.OnActAction -= SetRetract;

        VolumeControl.OnPauseStateChanged -= TogglePausedState;

        EelSegment.OnEelSegmentAttacked -= HandleAttack;
        Pemming.OnPemmingDefeat -= HandleDefeat;

        LevelManager.OnLevelStarted -= SetLevelStarted;
        LevelManager.OnLevelFinished -= SetLevelFinished;
    }

    void Start()
    {
        _tail = Instantiate(_tailPrefab, transform.position, transform.rotation);
        _lastSegmentPosition = _tail.transform.position;
        _elecDelayWait = new(_elecDelay);
    }

    void Update()
    {
        if(!_isLevelStarted || _isGamePaused) { return; }

        Move();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(_fullRetract) { return; }

        if(collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.Bite();
            _totalFishEaten.AddToValue(1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(_fullRetract) { return; }

        if(collision.CompareTag("Hazard"))
        {
            if(!_isElectrified)
            {
                HandleAttack();
                return;
            }
        }

        if(_isRetracting) { return; }

        if(collision.TryGetComponent(out EelHome eelHome))
        {
            if(eelHome != _currentHome)
            {
                _isRelocating = true;
                _currentHome = eelHome;
                transform.position = _currentHome.transform.position;

                StartRelocateSFX();
            }
        }
    }

    void TogglePausedState(bool isPaused)
    {
        _isGamePaused = isPaused;

        if(_isGamePaused)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.UnPause();
        }
    }

    void StartRelocateSFX()
    {
        if(!_isLevelStarted) { return; }
        if(_isPlayingRelocateSFX) { return; }

        _isPlayingRelocateSFX = true;
        _audioSource.clip = _retractSFX;
        _audioSource.volume = _retractVol;
        _audioSource.pitch = _relocateMaxPitch;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    void StopRelocateSFX()
    {
        _audioSource.Stop();
        _audioSource.pitch = 1f;
        _isPlayingRelocateSFX = false;
    }

    void GetMoveValue(Vector2 input)
    {
        if(!_isLevelStarted || _isGamePaused) { return; }

        _currentDirection = input;

        if(input == Vector2.zero)
        {
            _animator.SetBool(MOVE_HASH, false);
        }
        else
        {
            _animator.SetBool(MOVE_HASH, true);
            Rotate();
        }
    }

    void Rotate()
    {
        Vector3 rotationDirection = _currentDirection;
        rotationDirection.z = 0f;
        if(rotationDirection.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void SetRetract(bool value)
    {
        if(!_isLevelStarted || _isGamePaused || _isDefeated) { return; }

        _isRetracting = value;

        if(_isRetracting && !_isPlayingRetractSFX && _segments.Count > 0)
        {
            _isPlayingRetractSFX = true;
            _audioSource.clip = _retractSFX;
            _audioSource.volume = _retractVol;
            _audioSource.pitch = _retractStartPitch;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        if((!_isRetracting || _segments.Count == 0) && _isPlayingRetractSFX)
        {
            _audioSource.Stop();
            _audioSource.pitch = 1f;
            _isPlayingRetractSFX = false;
        }
    }

    void HandleAttack()
    {
        if(!_isLevelStarted || _isRelocating || _fullRetract || _isDefeated) { return; }

        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = null;


        if(_hurtSFX)
        {
            OnEelHurtSFX?.Invoke(_hurtSFX, _hurtVol);
        }

        _health--;
        OnEelHealthChange?.Invoke(_health);

        if(_health <= 0)
        {
            HandleDefeat();
            return;
        }

        _fullRetract = true;
    }

    void HandleDefeat()
    {
        if(_isDefeated) { return; }

        _isDefeated = true;
        StopAllCoroutines();
        _rigidBody.linearVelocity = Vector2.zero;
        _audioSource.Stop();
        OnEelDefeat?.Invoke();
        enabled = false;
    }

    void SetLevelStarted()
    {
        _isLevelStarted = true;
    }

    void SetLevelFinished()
    {
        _isLevelStarted = false;
        _currentDirection = Vector2.zero;
        _rigidBody.linearVelocity = Vector2.zero;
        _audioSource.Stop();
    }

    void Move()
    {
        if(_isRelocating)
        {
            if(_segments.Count == 0)
            {
                _tail.transform.SetPositionAndRotation(_currentHome.transform.position, _currentHome.transform.rotation);
                _lastSegmentPosition = _tail.transform.position;
                _isRelocating = false;
                StopRelocateSFX();
                return;
            }

            _rigidBody.linearVelocity = Vector2.zero;
            _tail.transform.position = Vector2.MoveTowards(_tail.transform.position, _segments[0].transform.position, _retractSpeed * Time.deltaTime);
            if(Vector2.Distance(_tail.transform.position, _segments[0].transform.position) <= Mathf.Epsilon)
            {
                _tail.transform.position = _segments[0].transform.position;
                GameObject discardedSegment = _segments[0].gameObject;
                _segments.Remove(_segments[0]);
                Destroy(discardedSegment);
                if(_segments.Count == 0)
                {
                    _tail.transform.SetPositionAndRotation(_currentHome.transform.position, _currentHome.transform.rotation);
                    _lastSegmentPosition = _tail.transform.position;
                    _isRelocating = false;
                    StopRelocateSFX();
                    return;
                }
                else
                {
                    _tail.transform.rotation = _segments[0].transform.rotation;

                    _audioSource.pitch -= _retractPitchIncrease;
                }
            }
        }
        else if(_isRetracting || _fullRetract)
        {
            if(_segments.Count == 0)
            {
                transform.position = _currentHome.transform.position;
                _fullRetract = false;
                return;
            }

            _rigidBody.linearVelocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, _segments[^1].transform.position, _retractSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, _segments[^1].transform.position) <= Mathf.Epsilon)
            {
                transform.position = _segments[^1].transform.position;
                GameObject discardedSegment = _segments[^1].gameObject;
                _segments.Remove(_segments[^1]);
                Destroy(discardedSegment);
                if(_segments.Count > 0)
                {
                    _lastSegmentPosition = _segments[^1].transform.position;

                    _audioSource.pitch += _retractPitchIncrease;
                }
                else
                {
                    _lastSegmentPosition = _tail.transform.position;
                    transform.position = _currentHome.transform.position;
                    _fullRetract = false;
                    _audioSource.pitch = 1f;
                }
            }
        }
        else
        {
            _rigidBody.linearVelocity = _moveSpeed * _currentDirection;
            TryPlaceSegment();
        }
    }

    void TryPlaceSegment()
    {
        if(Vector2.Distance(transform.position, _lastSegmentPosition) < _segmentSize + _bufferSpace) { return; }

        Vector2 direction = ((Vector2)transform.position - _lastSegmentPosition).normalized;
        Vector2 position = _lastSegmentPosition + direction * _segmentSize;
        EelSegment newSegement = Instantiate(_segmentPrefab, position, Quaternion.identity);

        Vector3 rotationDirection = transform.position - newSegement.transform.position;
        rotationDirection.z = 0f;
        if(rotationDirection.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg;
            newSegement.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        _segments.Add(newSegement);
        _lastSegmentPosition = position;

        if(_isElectrified)
        {
            newSegement.Electrify();
        }

        if(Vector2.Distance(transform.position, _lastSegmentPosition) >= _segmentSize + _bufferSpace)
        {
            newSegement.SetColliderNotTrigger();
            TryPlaceSegment();
        }
    }

    public void Electrify()
    {
        if(!gameObject.activeInHierarchy) { return; }
        if(_isElectrified) { return; }

        _isElectrified = true;
        _animator.SetBool(ELEC_HASH, _isElectrified);

        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        if(_elecSFX)
        {
            OnEelElectrifiedSFX?.Invoke(_elecSFX, _elecVol);
        }

        _currentCoroutine = StartCoroutine(ElectrifySegmentsRoutine());
    }

    IEnumerator ElectrifySegmentsRoutine()
    {
        for (int i = _segments.Count; i > 0; i--)
        {
            _segments[i - 1].Electrify();
            // if(_elecSFX)
            // {
            //     OnEelElectrifiedSFX?.Invoke(_elecSFX, _elecVol);
            // }
            yield return _elecDelayWait;
        }

        yield return _elecDelayWait;

        // if(_elecSFX)
        // {
        //     OnEelElectrifiedSFX?.Invoke(_elecSFX, _elecVol);
        // }

        _tail.Electrify();
        if(_currentHome)
        {
            _currentHome.Electrify();
        }
        _currentCoroutine = null;
    }

    public void Delectrify()
    {
        if(!gameObject.activeInHierarchy) { return; }   // I feel like this is only an issue because I'm using an interface but I'd rather not focus on dissecting that during a game jam

        _isElectrified = false;
        _animator.SetBool(ELEC_HASH, _isElectrified);

        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(DelectrifySegmentsRoutine());
    }

    IEnumerator DelectrifySegmentsRoutine()
    {
        for (int i = _segments.Count; i > 0; i--)
        {
            _segments[i - 1].Delectrify();
            yield return _elecDelayWait;
        }

        yield return _elecDelayWait;
        _tail.Delectrify();
        if(_currentHome)
        {
            _currentHome.Delectrify();
        }
        _currentCoroutine = null;
    }
}
