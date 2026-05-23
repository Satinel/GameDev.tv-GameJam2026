using System;
using UnityEngine;

public class PemmingController : MonoBehaviour
{
    public static event Action<AudioClip, float> OnHurtSFX;
    public static event Action<int> OnHealthChange;
    public static event Action OnDefeat;

    [SerializeField] int _health = 3;
    [SerializeField] float _moveSpeed = 5f, _invincibleDuration = 0.15f;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] GameObject _hurtmarkerPrefab, _defeatMarkerPrefab;
    [SerializeField] Animator _animator;
    [SerializeField] AudioClip _hurtSFX;
    [SerializeField] float _hurtVol = 1f;

    Vector2 _currentDirection = Vector2.zero;

    bool _isLevelStarted, _isGamePaused, _isDefeated, _isInvincible;
    float _timer;

    static readonly int MOVE_HASH = Animator.StringToHash("IsMoving");

    void Awake()
    {
        InputManager.OnMoveAction += GetMoveValue;
        // InputManager.OnActAction += Act; // TODO ? Have an action??

        VolumeControl.OnPauseStateChanged += TogglePausedState;

        LevelManager.OnLevelStarted += SetLevelStarted;
        LevelManager.OnLevelFinished += SetLevelFinished;
    }

    void OnDestroy()
    {
        InputManager.OnMoveAction -= GetMoveValue;
        // InputManager.OnActAction -= Act; // TODO ? Have an action??

        VolumeControl.OnPauseStateChanged -= TogglePausedState;

        LevelManager.OnLevelStarted -= SetLevelStarted;
        LevelManager.OnLevelFinished -= SetLevelFinished;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hazard"))
        {
            HandleDamage();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(_isInvincible) { return; }

        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hazard"))
        {
            HandleDamage();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("Hazard"))
        {
            HandleDamage();
        }
    }


    void Update()
    {
        if(!_isLevelStarted || _isGamePaused || _isDefeated) { return; }

        Move();

        if(_isInvincible)
        {
            _timer += Time.deltaTime;

            if(_timer >= _invincibleDuration)
            {
                _isInvincible = false;
            }
        }
    }

    void Move()
    {
        _rigidBody.linearVelocity = _moveSpeed * _currentDirection;
    }

    void HandleDamage()
    {
        if(_isInvincible || !_isLevelStarted || _isDefeated) { return; }

        Instantiate(_hurtmarkerPrefab, transform.position, Quaternion.identity);

        OnHurtSFX?.Invoke(_hurtSFX, _hurtVol);

        _health--;
        OnHealthChange?.Invoke(_health);

        if(_health <= 0)
        {
            HandleDefeat();
        }
        else
        {
            _timer = 0;
            _isInvincible = true;
        }
    }



    void HandleDefeat()
    {
        if(_isDefeated) { return; }

        _isDefeated = true;
        _rigidBody.linearVelocity = Vector2.zero;
        _animator.SetBool(MOVE_HASH, false);

        OnDefeat?.Invoke();

        Instantiate(_defeatMarkerPrefab, transform);
        OnDefeat?.Invoke();
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

    void TogglePausedState(bool isPaused)
    {
        _isGamePaused = isPaused;
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
        _animator.SetBool(MOVE_HASH, false);
    }
}
