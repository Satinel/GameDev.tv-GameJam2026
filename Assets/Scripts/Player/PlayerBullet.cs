using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] int _damage = 1;
    [SerializeField] IntReferenceSO _damageRef;
    float _timer, _destructionTime = 5f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage + _damageRef.Value);
        }
    }

    void Update()
    {
        transform.position += _moveSpeed * Time.deltaTime * transform.right;

        _timer += Time.deltaTime;

        if(_timer >= _destructionTime)
        {
            Destroy(gameObject);
        }
    }
}
