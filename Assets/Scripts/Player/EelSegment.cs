using System;
using UnityEngine;

public class EelSegment : MonoBehaviour
{
    public static event Action OnEelSegmentAttacked;

    [SerializeField] Collider2D _collider;
    [SerializeField] Animator _animator;
    [SerializeField] IntReferenceSO _totalFishElectrified;

    bool _isElectrified;

    static readonly int ELEC_HASH = Animator.StringToHash("Electrified");

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EelController>()) { return; }  // Why is this here? I'm sure I must have had a reason...

        if(collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            if(_isElectrified)
            {
                enemy.Zap();
                _totalFishElectrified.AddToValue(1);
            }
            else
            {
                OnEelSegmentAttacked?.Invoke();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hazard"))
        {
            if(!_isElectrified)
            {
                OnEelSegmentAttacked?.Invoke();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<EelController>())
        {
            SetColliderNotTrigger();
        }
    }

    public void SetColliderNotTrigger()
    {
        if(!_collider) { return; }

        _collider.isTrigger = false;
    }

    public void Electrify()
    {
        if(_isElectrified) { return; }

        _isElectrified = true;

        if(_animator)
        {
            _animator.SetBool(ELEC_HASH, _isElectrified);
        }
    }

    public void Delectrify()
    {
        _isElectrified = false;

        if(_animator)
        {
            _animator.SetBool(ELEC_HASH, _isElectrified);
        }
    }
}
