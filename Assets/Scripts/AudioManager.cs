using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _collectableSFX, _enemyDeathSFX;
    [SerializeField] float _collectableVol = 1f, _enemyDeathVol = 1f;

    void OnEnable()
    {
        EelController.OnEelHurtSFX += PlaySound;
        EelController.OnEelElectrifiedSFX += PlaySound;

        Collectable.OnAnyCollectableCollected += PlayCollectableSFX;
        Enemy.OnEnemyDestroyed += PlayEnemyDeathSFX;
    }

    void OnDisable()
    {
        EelController.OnEelHurtSFX -= PlaySound;
        EelController.OnEelElectrifiedSFX -= PlaySound;

        Collectable.OnAnyCollectableCollected -= PlayCollectableSFX;
        Enemy.OnEnemyDestroyed -= PlayEnemyDeathSFX;
    }

    void PlaySound(AudioClip clip, float volume)
    {
        if(clip)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
    }

    void PlayCollectableSFX(int _)
    {
        PlaySound(_collectableSFX, _collectableVol);
    }

    void PlayEnemyDeathSFX(Enemy _)
    {
        PlaySound(_enemyDeathSFX, _enemyDeathVol);
    }
}
