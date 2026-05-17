using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float _spawnRate =  7.5f;
    [SerializeField] float _maxRandomStart = 12.5f;
    [SerializeField] Enemy _enemyPrefab;

    Enemy _currentEnemy;

    float _timer;

    void Awake()
    {
        if(!_enemyPrefab)
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
        Enemy.OnEnemyDestroyed += CheckCurrentEnemy;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= CheckCurrentEnemy;
    }

    void Update()
    {
        if(_currentEnemy) { return; }

        _timer += Time.deltaTime;

        if(_timer >= _spawnRate)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
        _timer -= _spawnRate + Random.Range(0, _maxRandomStart);
    }

    void CheckCurrentEnemy(Enemy enemy)
    {
        if(enemy == _currentEnemy)
        {
            _currentEnemy = null;
        }
    }
}
