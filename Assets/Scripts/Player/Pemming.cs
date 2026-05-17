using UnityEngine;

public class Pemming : MonoBehaviour
{
    [SerializeField] float _moveSpeed =  3f;
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Animator _animator;
    [SerializeField] float _castCheckDistance = 0.2f;
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] Vector2 _boxSize = Vector2.one;

    static readonly int MOVE_HASH = Animator.StringToHash("IsMoving");

    void FixedUpdate()
    {
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
}
