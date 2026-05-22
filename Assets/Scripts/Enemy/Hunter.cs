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
    Vector2 _moveDirection;
    bool _isFinished;

    void Awake()
    {
        EelController.OnEelDefeat += SetIsFinished;
        LevelManager.OnLevelFinished += SetIsFinished;
    }

    void OnDestroy()
    {
        EelController.OnEelDefeat -= SetIsFinished;
        LevelManager.OnLevelFinished -= SetIsFinished;
    }

    void Start()
    {
        _moveDirection = new(Random.value < 0.5f ? -1 : 1, Random.value < 0.5f ? -1 : 1);   // I should learn more about Random functions beyond Random.Range
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isFinished) { return; }

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
        if(_isFinished)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }

        if(_eelHead)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;

            if(!_target)
            {
                SetTarget();
            }

            if(_target && Vector2.Distance(transform.position, _target.position) <= _minDistance)
            {
                SetTarget();
            }

            _sprite.transform.up = _target.right;
            transform.position = Vector2.MoveTowards(transform.position, _target.position, _tangledSpeed * Time.deltaTime);
        }
        else
        {
            _rigidbody2D.linearVelocity = _moveSpeed * _moveDirection;
        }
    }

    void SetTarget()
    {
        if(_segmentIndex >= _eelHead.Segments.Count - 1)
        {
            _target = _eelHead.transform;
        }
        else
        {
            _segmentIndex++;

            _target = _eelHead.Segments[_segmentIndex].transform;
        }
    }

    public void Tangle(EelController eel, int segmentIndex)
    {
        if(_isFinished) { return; }

        if(_eelHead != null)
        {
            if(!_target)
            {
                SetTarget();
            }

            return;
        }

        _eelHead = eel;
        _segmentIndex = segmentIndex;

        if(_segmentIndex >= _eelHead.Segments.Count - 1) { return; }

        _target = _eelHead.Segments[_segmentIndex].transform;
    }

    public void Untangle()
    {
        if(_isFinished) { return; }
        _eelHead = null;
        _target = null;
        _segmentIndex = 0;
        _sprite.transform.up = transform.up;
    }

    public void Zap()
    {
        _thisEnemy.Zap();
    }

    void SetIsFinished()
    {
        _isFinished = true;
    }
}
