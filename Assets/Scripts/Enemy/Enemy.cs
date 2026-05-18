using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDestroyed;

    void OnEnable()
    {
        LevelManager.OnLevelFinished += LevelManager_OnLevelFinished;
    }

    void OnDisable()
    {
        LevelManager.OnLevelFinished -= LevelManager_OnLevelFinished;
    }

    void LevelManager_OnLevelFinished()
    {
        gameObject.SetActive(false);
    }

    public void DealDamage()
    {
        OnEnemyDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
