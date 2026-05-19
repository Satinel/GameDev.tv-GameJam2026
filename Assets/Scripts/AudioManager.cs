using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _collectableSFX, _enemyDeathSFX;
    [SerializeField] float _collectableVol = 1f, _enemyDeathVol = 1f;

    bool _isGamePaused;

    void OnEnable()
    {
        VolumeControl.OnPauseStateChanged += TogglePausedState;

        EelController.OnEelHurtSFX += PlaySound;
        EelController.OnEelElectrifiedSFX += PlaySound;

        Collectable.OnAnyCollectableCollected += PlayCollectableSFX;
        Enemy.OnEnemyDestroyed += PlayEnemyDeathSFX;
    }

    void OnDisable()
    {
        VolumeControl.OnPauseStateChanged -= TogglePausedState;

        EelController.OnEelHurtSFX -= PlaySound;
        EelController.OnEelElectrifiedSFX -= PlaySound;

        Collectable.OnAnyCollectableCollected -= PlayCollectableSFX;
        Enemy.OnEnemyDestroyed -= PlayEnemyDeathSFX;
    }

    void TogglePausedState(bool isPaused)
    {
        _isGamePaused = isPaused;
    }

    void PlaySound(AudioClip clip, float volume)
    {
        if(_isGamePaused) { return; }

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
