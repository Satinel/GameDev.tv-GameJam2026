using UnityEngine;

public class EelSegment : MonoBehaviour, IElectrifiable
{
    public Color DefaultColor, ElectrifiedColor;

    [SerializeField] Collider2D _collider;
    [SerializeField] SpriteRenderer _spriteRenderer;
    bool _isElectrified;

    void Start()
    {
        DefaultColor = _spriteRenderer.color;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EelController>()) { return; }

        if(_isElectrified)
        {
Debug.Log("ZAP!");
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

        _spriteRenderer.color = ElectrifiedColor;
    }

    public void Delectrify()
    {
        _isElectrified = false;
        _spriteRenderer.color = DefaultColor;
    }
}
