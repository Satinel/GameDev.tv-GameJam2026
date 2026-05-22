using System.Collections;
using UnityEngine;

public class Switch : MonoBehaviour, IElectrifiable
{
    [SerializeField] Vector2 _moveDirection = new(0, 2f);
    [SerializeField] float _totalDuration = 1.2f;
    [SerializeField] Transform _wiresParent;

    Coroutine _activeRoutine;
    Vector2 _startPosition, _targetPosition;
    bool _isElectrified;

    void Start()
    {
        _startPosition = transform.position;
        _targetPosition = _startPosition + _moveDirection;
    }

    public void Electrify()
    {
        if(!gameObject.activeInHierarchy) { return; }
        if(_isElectrified) { return; }

        _isElectrified = true;

        if(_wiresParent)
        {
            foreach(WireColor wire in _wiresParent.GetComponentsInChildren<WireColor>())
            {
                wire.Electrify();
            }
        }

        if(_activeRoutine != null)
        {
            StopCoroutine(_activeRoutine);
        }

        _activeRoutine = StartCoroutine(RaiseRoutine());
    }

    public void Delectrify()
    {
        if(!gameObject.activeInHierarchy) { return; }
        if(!_isElectrified) { return; }

        _isElectrified = false;

        if(_wiresParent)
        {
            foreach(WireColor wire in _wiresParent.GetComponentsInChildren<WireColor>())
            {
                wire.Delectrify();
            }
        }

        if(_activeRoutine != null)
        {
            StopCoroutine(_activeRoutine);
        }

        _activeRoutine = StartCoroutine(LowerRoutine());
    }

    IEnumerator RaiseRoutine()
    {
        float timer = 0;
        while(timer < _totalDuration)
        {
            timer += Time.deltaTime;
            float distanceDelta = Vector2.Distance(transform.position, _targetPosition) / (_totalDuration - timer) * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, distanceDelta);
            yield return null;
        }

        transform.position = _targetPosition;
        _activeRoutine = null;
    }

    IEnumerator LowerRoutine()
    {
        float timer = 0;
        while(timer < _totalDuration)
        {
            timer += Time.deltaTime;
            float distanceDelta = Vector2.Distance(transform.position, _startPosition) / (_totalDuration - timer) * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _startPosition, distanceDelta);
            yield return null;
        }

        transform.position = _startPosition;
        _activeRoutine = null;
    }
}
