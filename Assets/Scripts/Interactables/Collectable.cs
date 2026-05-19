using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action<int> OnAnyCollectableCollected;

    [SerializeField] int _value;
    [SerializeField] Animator _animator;

    static readonly int APPEAR_HASH = Animator.StringToHash("Appear");

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            OnAnyCollectableCollected?.Invoke(_value);
            Destroy(gameObject);
        }
    }

    public void SetAppearTrigger()
    {
        _animator.SetTrigger(APPEAR_HASH);
    }
}
