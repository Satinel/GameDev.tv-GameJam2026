using UnityEngine;

public class SurvivorSpawner : MonoBehaviour
{
    [SerializeField] float _spawnRate =  7.5f;
    [SerializeField] float _maxRandomStart = 6.5f, _staticStartDelay = 1.5f, _maxRandomRespawn = 12.5f;
    [SerializeField] Enemy[] _enemyPrefabs;
    [SerializeField] Transform[] _spawnPoints;

    float _timer;
    bool _isTimerEnabled;

    void Awake()
    {
        if(_enemyPrefabs.Length < 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _timer -= Random.Range(-_maxRandomStart, 0) + _staticStartDelay;
    }

    void OnEnable()
    {
        LevelManager.OnLevelStarted += EnableTimer;
        LevelManager.OnLevelFinished += DisableTimer;
        EelController.OnEelDefeat += DisableTimer;
        PemmingController.OnDefeat += DisableTimer;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= EnableTimer;
        LevelManager.OnLevelFinished -= DisableTimer;
        EelController.OnEelDefeat -= DisableTimer;
        PemmingController.OnDefeat -= DisableTimer;
    }

    void Update()
    {
        if(!_isTimerEnabled) { return; }

        _timer += Time.deltaTime;

        if(_timer >= _spawnRate)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)], _spawnPoints[Random.Range(0, _spawnPoints.Length)].position, Quaternion.identity);
        _timer -= _spawnRate + Random.Range(0, _maxRandomRespawn);
    }

    void EnableTimer()
    {
        _isTimerEnabled = true;
    }

    void DisableTimer()
    {
        _isTimerEnabled = false;
    }
}
