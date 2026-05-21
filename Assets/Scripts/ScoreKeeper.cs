using System;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static event Action OnScoreChanged;
    [SerializeField] IntReferenceSO _scoreReference;

    void OnEnable()
    {
        Enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
        Collectable.OnAnyCollectableCollected += IncreaseScore;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;
        Collectable.OnAnyCollectableCollected -= IncreaseScore;
    }

    void Enemy_OnEnemyDestroyed(Enemy enemy)
    {
        IncreaseScore(enemy.ScoreValue);
    }

    void IncreaseScore(int value)
    {
        _scoreReference.AddToValue(value);
        OnScoreChanged?.Invoke();
    }
}
