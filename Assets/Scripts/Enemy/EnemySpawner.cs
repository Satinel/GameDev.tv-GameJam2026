using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float _spawnRate =  7.5f;
    [SerializeField] float _maxRandomStart = 6.5f, _maxRandomRespawn = 12.5f;
    [SerializeField] Enemy[] _enemyPrefabs;

    Enemy _currentEnemy;

    float _timer;
    bool _isLevelStarted;

    void Awake()
    {
        if(_enemyPrefabs.Length < 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _timer -= Random.Range(0, _maxRandomStart);
    }

    void OnEnable()
    {
        LevelManager.OnLevelStarted += SetLevelStarted;
        Enemy.OnEnemyDestroyed += CheckCurrentEnemy;
        LevelManager.OnLevelFinished += SetLevelFinished;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= SetLevelStarted;
        Enemy.OnEnemyDestroyed -= CheckCurrentEnemy;
        LevelManager.OnLevelFinished -= SetLevelFinished;
    }

    void Update()
    {
        if(!_isLevelStarted || _currentEnemy) { return; }

        _timer += Time.deltaTime;

        if(_timer >= _spawnRate)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)], transform.position, transform.rotation);
        _timer -= _spawnRate + Random.Range(0, _maxRandomRespawn);
    }

    void SetLevelStarted()
    {
        _isLevelStarted = true;
    }

    void SetLevelFinished()
    {
        _isLevelStarted = false;
    }

    void CheckCurrentEnemy(Enemy enemy)
    {
        if(enemy == _currentEnemy)
        {
            _currentEnemy = null;
        }
    }
}
