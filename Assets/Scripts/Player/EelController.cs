using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelController : MonoBehaviour, IElectrifiable
{
    [SerializeField] float _moveSpeed = 5f, _retractSpeed = 7.5f;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] EelSegment _segmentPrefab, _tailPrefab;
    [SerializeField] float _segmentSize = 1;
    [SerializeField] float _bufferSpace = 0.25f;
    [SerializeField] Animator _animator;
    [SerializeField] float _elecDelay = 0.1f;

    WaitForSeconds _elecDelayWait = new(0.01f);

    Vector2 _currentDirection = Vector2.zero;
    bool _isRetracting = false;
    bool _isElectrified = false;
    bool _fullRetract = false;

    Vector2 _lastSegmentPosition;
    EelSegment _tail;
    List<EelSegment> _segments = new();
    Coroutine _currentCoroutine;

    static readonly int ELEC_HASH = Animator.StringToHash("Electrified");
    static readonly int MOVE_HASH = Animator.StringToHash("Moving");

    void Awake()
    {
        InputManager.OnMoveAction += GetMoveValue;
        InputManager.OnActAction += SetRetract;

        EelSegment.OnEelSegmentAttacked += HandleAttack;
    }

    void OnDestroy()
    {
        InputManager.OnMoveAction -= GetMoveValue;
        InputManager.OnActAction -= SetRetract;

        EelSegment.OnEelSegmentAttacked -= HandleAttack;
    }

    void Start()
    {
        _tail = Instantiate(_tailPrefab, transform.position, transform.rotation);
        _lastSegmentPosition = _tail.transform.position;
        _elecDelayWait = new(_elecDelay);
    }

    void Update()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }

    void GetMoveValue(Vector2 input)
    {
        _currentDirection = input;

        if(input == Vector2.zero)
        {
            _animator.SetBool(MOVE_HASH, false);
        }
        else
        {
            _animator.SetBool(MOVE_HASH, true);
            Rotate();
        }
    }

    void Rotate()
    {
        Vector3 rotationDirection = _currentDirection;
        rotationDirection.z = 0f;
        if(rotationDirection.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void SetRetract(bool value)
    {
        _isRetracting = value;
    }

    void HandleAttack()
    {
        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = null;

        _fullRetract = true;
    }

    void Move()
    {
        if(_isRetracting || _fullRetract)
        {
            if(_segments.Count == 0)
            {
                transform.position = _tail.transform.position;
                return;
            }

            _rigidBody.linearVelocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, _segments[^1].transform.position, _retractSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, _segments[^1].transform.position) <= Mathf.Epsilon)
            {
                transform.position = _segments[^1].transform.position;
                GameObject discardedSegment = _segments[^1].gameObject;
                _segments.Remove(_segments[^1]);
                Destroy(discardedSegment);
                if(_segments.Count > 0)
                {
                    _lastSegmentPosition = _segments[^1].transform.position;
                }
                else
                {
                    _lastSegmentPosition = _tail.transform.position;
                    _fullRetract = false;
                }
            }
        }
        else
        {
            _rigidBody.linearVelocity = _moveSpeed * _currentDirection;
            TryPlaceSegment();
        }
    }

    void TryPlaceSegment()
    {
        if(Vector2.Distance(transform.position, _lastSegmentPosition) < _segmentSize + _bufferSpace) { return; }

        Vector2 direction = ((Vector2)transform.position - _lastSegmentPosition).normalized;
        Vector2 position = _lastSegmentPosition + direction * _segmentSize;
        EelSegment newSegement = Instantiate(_segmentPrefab, position, Quaternion.identity);

        Vector3 rotationDirection = transform.position - newSegement.transform.position;
        rotationDirection.z = 0f;
        if(rotationDirection.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg;
            newSegement.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        _segments.Add(newSegement);
        _lastSegmentPosition = position;

        if(_isElectrified)
        {
            newSegement.Electrify();
        }

        if(Vector2.Distance(transform.position, _lastSegmentPosition) >= _segmentSize + _bufferSpace)
        {
            newSegement.SetColliderNotTrigger();
            TryPlaceSegment();
        }
    }

    public void Electrify()
    {
        if(_isElectrified) { return; }

        _isElectrified = true;
        _animator.SetBool(ELEC_HASH, _isElectrified);

        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(ElectrifySegmentsRoutine());
    }

    IEnumerator ElectrifySegmentsRoutine()
    {
        for (int i = _segments.Count; i > 0; i--)
        {
            _segments[i - 1].Electrify();
            yield return _elecDelayWait;
        }

        _tail.Electrify();
        _currentCoroutine = null;
    }

    public void Delectrify()
    {
        _isElectrified = false;
        _animator.SetBool(ELEC_HASH, _isElectrified);

        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(DelectrifySegmentsRoutine());
    }

    IEnumerator DelectrifySegmentsRoutine()
    {
        for (int i = _segments.Count; i > 0; i--)
        {
            _segments[i - 1].Delectrify();
            yield return _elecDelayWait;
        }

        _tail.Delectrify();
        _currentCoroutine = null;
    }
}
