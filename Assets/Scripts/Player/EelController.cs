using System.Collections.Generic;
using UnityEngine;

public class EelController : MonoBehaviour, IElectrifiable
{
    [SerializeField] float _moveSpeed = 5f, _retractSpeed = 7.5f;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] EelSegment _segmentPrefab;
    [SerializeField] GameObject _tailPrefab;
    [SerializeField] float _segmentSize = 1;
    [SerializeField] float _bufferSpace = 0.25f;

    Vector2 _currentDirection = Vector2.zero;
    bool _isRetracting = false;
    bool _isElectrified = false;

    Vector2 _lastSegmentPosition;
    GameObject _tail;
    List<EelSegment> _segments = new();

    void Awake()
    {
        InputManager.OnMoveAction += GetMoveValue;
        InputManager.OnActAction += SetRetract;
    }

    void OnDestroy()
    {
        InputManager.OnMoveAction -= GetMoveValue;
        InputManager.OnActAction -= SetRetract;
    }

    void Start()
    {
        _tail = Instantiate(_tailPrefab, transform.position, transform.rotation);
        _lastSegmentPosition = _tail.transform.position;
    }

    void Update()
    {
        Move();
    }

    void GetMoveValue(Vector2 input)
    {
        _currentDirection = input;
    }

    void SetRetract(bool value)
    {
        _isRetracting = value;
    }

    void Move()
    {
        if(_isRetracting)
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
        EelSegment newSegement = Instantiate(_segmentPrefab, position, transform.rotation);
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

        foreach(EelSegment segment in _segments)    // TODO: Coroutine to electrify each segment on a short delay?
        {
            segment.Electrify();
        }
    }

    public void Delectrify()
    {
        _isElectrified = false;

        foreach(EelSegment segment in _segments)    // TODO: Coroutine here as well?
        {
            segment.Delectrify();
        }
    }
}
