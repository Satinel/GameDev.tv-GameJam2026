using UnityEngine;

public class PemmingUpgrades : MonoBehaviour
{
    [SerializeField] IntReferenceSO _health, _speed, _noseWeapon, _noseDamage, _otherWeapon, _otherDamage;
    [SerializeField] Transform[] _noseSpawnPoints, _otherSpawnPoints;
    [SerializeField] PemmingController _pemming;
    [SerializeField] GameObject _noseWeaponPrefab, _otherWeaponPrefab;
    [SerializeField] float _noseFireRate, _otherFireRate;
    float _noseTimer, _otherTimer;

    void Start()
    {
        _pemming.SetHealth(_health.Value);
        _pemming.SetSpeed(_speed.Value);
    }

    void Update()
    {
        if(_noseWeapon.Value > 0)
        {
            _noseTimer += Time.deltaTime;
            if(_noseTimer >= _noseFireRate)
            {
                _noseTimer -= _noseFireRate;
                for(int i = 0; i < _noseWeapon.Value; i++)
                {
                   Instantiate(_noseWeaponPrefab, _noseSpawnPoints[i].position, _noseSpawnPoints[i].rotation);
                }
            }
        }

        if(_otherWeapon.Value > 0)
        {
            _otherTimer += Time.deltaTime;
            if(_otherTimer >= _otherFireRate)
            {
                _otherTimer -= _otherFireRate;
                for(int i = 0; i < _otherWeapon.Value; i++)
                {
                   Instantiate(_otherWeaponPrefab, _otherSpawnPoints[i].position, _otherSpawnPoints[i].rotation);
                }
            }
        }
    }
}
