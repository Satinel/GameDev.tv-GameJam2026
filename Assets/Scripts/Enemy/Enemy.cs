using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDestroyed;

    void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke(this);
    }
}
