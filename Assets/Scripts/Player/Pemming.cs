using UnityEngine;

public class Pemming : MonoBehaviour
{
    [SerializeField] float _moveSpeed =  3f;
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Animator _animator;
    [SerializeField] float _castCheckDistance = 0.2f;
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] Vector2 _boxSize = Vector2.one;

    bool _isLevelStarted;

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

    void FixedUpdate()
    {
        if(!_isLevelStarted) { return; }

        _rigidbody2D.linearVelocity = _moveSpeed * transform.right;

        if(!Physics2D.BoxCast(transform.position, _boxSize, transform.rotation.z, transform.right, _castCheckDistance, _obstacleLayer))
        {
            _animator.SetBool(MOVE_HASH, true);
        }
        else
        {
            _animator.SetBool(MOVE_HASH, false);
        }
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
