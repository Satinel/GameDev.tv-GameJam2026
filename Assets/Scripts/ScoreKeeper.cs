using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
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
        _scoreReference.AddToValue(enemy.ScoreValue);
    }

    void IncreaseScore(int value)
    {
        _scoreReference.AddToValue(value);
    }
}
