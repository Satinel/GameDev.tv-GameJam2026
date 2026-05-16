using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action<int> OnAnyCollectableCollected;

    [SerializeField] int _value;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            OnAnyCollectableCollected?.Invoke(_value);
            Destroy(gameObject);
        }
    }
}
