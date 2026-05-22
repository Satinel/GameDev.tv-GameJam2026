using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.5f, _tangledSpeed = 0.85f, _minDistance = 0.01f;
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Enemy _thisEnemy;
    public Enemy Enemy => _thisEnemy;

    EelController _eelHead;
    int _segmentIndex;
    Transform _target;
    Vector2 _moveDirection = Vector2.one.normalized;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 point = collision.ClosestPoint(transform.position);
        Vector2 normal = ((Vector2)transform.position - point).normalized;
        if(normal == Vector2.zero) { return; }

        _moveDirection = Vector2.Reflect(_moveDirection, normal).normalized;

        if(point.y > transform.position.y)
        {
            transform.up = -transform.up;
        }
    }

    void Update()
    {
        if(_eelHead)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;

            if(Vector2.Distance(transform.position, _target.position) <= _minDistance)
            {
                if(_eelHead.Segments.Count < _segmentIndex)
                {
                    _target = _eelHead.transform;
                }
                else
                {
                    _segmentIndex++;
                    _target = _eelHead.Segments[_segmentIndex].transform;
                }
            }
            _sprite.transform.rotation = _target.rotation;
            Vector2.MoveTowards(transform.position, _target.position, _tangledSpeed * Time.deltaTime);
        }
        else
        {
            _rigidbody2D.linearVelocity = _moveSpeed * _moveDirection;
        }
    }

    public void Tangle(EelController eel, int segmentIndex)
    {
        _eelHead = eel;
        _segmentIndex = segmentIndex;

        if(_eelHead.Segments.Count < _segmentIndex) { return; } // This SHOULD be impossible

        _target = _eelHead.Segments[_segmentIndex].transform;
    }

    public void Untangle()
    {
        _eelHead = null;
        _sprite.transform.rotation = transform.rotation;
    }

    public void Zap()
    {
        _thisEnemy.Zap();
    }
}
