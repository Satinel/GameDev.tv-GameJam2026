using UnityEngine;

public class SideMover : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] Rigidbody2D _rigidBody;

    bool _isStopped;

    void Awake()
    {
        EelController.OnEelDefeat += StopMovement;
    }

    void OnDestroy()
    {
        EelController.OnEelDefeat -= StopMovement;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        transform.right *= -1;
    }

    void Update()
    {
        if(_isStopped) { return; }

        _rigidBody.linearVelocity = _moveSpeed * transform.right;
    }

    void StopMovement()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        _isStopped = true;
    }
}
