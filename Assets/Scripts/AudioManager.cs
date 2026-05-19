using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _collectableSFX, _enemyDeathSFX, _goalJingleSFX;
    [SerializeField] float _collectableVol = 1f, _enemyDeathVol = 1f, _goalJingleVol = 1f;

    bool _isGamePaused;

    void OnEnable()
    {
        VolumeControl.OnPauseStateChanged += TogglePausedState;

        EelController.OnEelHurtSFX += PlaySound;
        EelController.OnEelElectrifiedSFX += PlaySound;

        Collectable.OnAnyCollectableCollected += PlayCollectableSFX;
        Enemy.OnEnemyDestroyed += PlayEnemyDeathSFX;

        Goal.OnGoalAchieved += PlayGoalJingleSFX;
    }

    void OnDisable()
    {
        VolumeControl.OnPauseStateChanged -= TogglePausedState;

        EelController.OnEelHurtSFX -= PlaySound;
        EelController.OnEelElectrifiedSFX -= PlaySound;

        Collectable.OnAnyCollectableCollected -= PlayCollectableSFX;
        Enemy.OnEnemyDestroyed -= PlayEnemyDeathSFX;

        Goal.OnGoalAchieved -= PlayGoalJingleSFX;
    }

    void TogglePausedState(bool state)
    {
        _isGamePaused = state;
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

    void PlayGoalJingleSFX()
    {
        PlaySound(_goalJingleSFX, _goalJingleVol);
    }
}
