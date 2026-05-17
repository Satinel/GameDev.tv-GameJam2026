using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDestroyed;

    public void DealDamage()
    {
        OnEnemyDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
