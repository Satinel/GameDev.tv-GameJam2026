using UnityEngine;

public class SideMover : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] Rigidbody2D _rigidBody;

    void Update()
    {
        _rigidBody.linearVelocity = _moveSpeed * transform.right;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        transform.right *= -1;
    }
}
