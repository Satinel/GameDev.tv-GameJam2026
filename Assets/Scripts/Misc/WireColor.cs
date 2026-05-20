using UnityEngine;

public class WireColor : MonoBehaviour, IElectrifiable
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Color _wireColor = Color.white, _electrifiedColor = Color.cyan;

    void Awake()
    {
        _spriteRenderer.color = _wireColor;
    }

    public void Delectrify()
    {
        _spriteRenderer.color = _wireColor;
    }

    public void Electrify()
    {
        _spriteRenderer.color = _electrifiedColor;
    }
}
