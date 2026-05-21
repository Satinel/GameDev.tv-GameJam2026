using System;
using UnityEngine;

public class Pemming : MonoBehaviour
{
    public static event Action OnPemmingDefeat;

    [SerializeField] float _moveSpeed =  3f;
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Animator _animator;
    [SerializeField] float _castCheckDistance = 0.2f;
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] Vector2 _boxSize = Vector2.one;

    bool _isLevelStarted, _isDefeated;

    static readonly int MOVE_HASH = Animator.StringToHash("IsMoving");

    void OnEnable()
    {
        LevelManager.OnLevelStarted += SetLevelStarted;
        LevelManager.OnLevelFinished += SetLevelFinished;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= SetLevelStarted;
        LevelManager.OnLevelFinished -= SetLevelFinished;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Hazard"))
        {
            HandleDefeat();
        }
    }

    void Update()
    {
        if(!_isLevelStarted) { return; }

        _rigidbody2D.linearVelocity = new(_moveSpeed, _rigidbody2D.linearVelocityY);

        if(!Physics2D.BoxCast(transform.position, _boxSize, transform.rotation.z, transform.right, _castCheckDistance, _obstacleLayer))
        {
            _animator.SetBool(MOVE_HASH, true);
        }
        else
        {
            _animator.SetBool(MOVE_HASH, false);
        }
    }

    void HandleDefeat()
    {
        if(_isDefeated) { return; }

        _isDefeated = true;
        OnPemmingDefeat?.Invoke();
    }

    void SetLevelStarted()
    {
        _isLevelStarted = true;
    }

    void SetLevelFinished()
    {
        _isLevelStarted = false;
        _animator.SetBool(MOVE_HASH, false);
    }
}
