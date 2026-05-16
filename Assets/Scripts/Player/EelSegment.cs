using System;
using UnityEngine;

public class EelSegment : MonoBehaviour, IElectrifiable
{
    public static event Action OnEelSegmentAttacked;

    public Color DefaultColor, ElectrifiedColor;

    [SerializeField] Collider2D _collider;
    [SerializeField] SpriteRenderer _spriteRenderer;
    bool _isElectrified;

    void Start()
    {
        DefaultColor = _spriteRenderer.color;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EelController>()) { return; }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(_isElectrified)
            {
                Destroy(collision.gameObject);
            }
            else
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
        _collider.isTrigger = false;
    }

    public void Electrify()
    {
        _isElectrified = true;

        if(_spriteRenderer)
        {
            _spriteRenderer.color = ElectrifiedColor;
        }
    }

    public void Delectrify()
    {
        _isElectrified = false;

        if(_spriteRenderer)
        {
            _spriteRenderer.color = DefaultColor;
        }
    }
}
