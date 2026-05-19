using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDestroyed;

    [field:SerializeField] public int ScoreValue { get; private set; } = 100;
    [SerializeField] FloatingText _floatingTextPrefab;

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
        FloatingText floatingText = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity);
        floatingText.SetTextFromInt(ScoreValue);

        Destroy(gameObject);
    }
}
