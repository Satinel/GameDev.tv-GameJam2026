using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] IntReferenceSO _damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage.Value);
        }
    }

    void Update()
    {
        transform.position = _moveSpeed * Time.deltaTime * transform.right;
    }
}
