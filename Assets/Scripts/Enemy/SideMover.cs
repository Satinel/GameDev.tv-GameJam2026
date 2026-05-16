using UnityEngine;

public class SideMover : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] Vector2 _moveDirection;
    [SerializeField] SpriteRenderer _spriteRenderer;

    void Update()
    {
        _rigidBody.linearVelocity = _moveSpeed * _moveDirection;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if(collision.gameObject.CompareTag("Obstacle"))
        // {
        _moveDirection *= -1;
        _spriteRenderer.flipX = true;
        // }
    }
}
