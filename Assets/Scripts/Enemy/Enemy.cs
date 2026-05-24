using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDestroyed;
    public static event Action OnEnemyBit, OnEnemyZapped;
    public static event Action OnYemmepBit, OnYemmepZapped;

    [field:SerializeField] public int ScoreValue { get; private set; } = 100;
    [field:SerializeField] public bool IsYemmep { get; private set; } = false;
    [SerializeField] FloatingText _floatingTextPrefab;
    [SerializeField] int _health = 1;

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

    public void Zap()
    {
        OnEnemyZapped?.Invoke();
        if(IsYemmep)
        {
            OnYemmepZapped?.Invoke();
        }

        HandleDeath();
    }

    public void Bite()
    {
        OnEnemyBit?.Invoke();
        if(IsYemmep)
        {
            OnYemmepBit?.Invoke();
        }
        HandleDeath();
    }

    void HandleDeath()
    {
        OnEnemyDestroyed?.Invoke(this);
        FloatingText floatingText = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity);
        if(IsYemmep)
        {
            floatingText.SetText("YEMMEP!");
        }
        else
        {
            floatingText.SetTextFromInt(ScoreValue);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            HandleDeath();
        }
    }
}
