using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource, _pitchedAudioSource;
    [SerializeField] AudioClip _collectableSFX, _enemyBitSFX, _enemyZapSFX, _pemmingDefeatSFX;
    [SerializeField] float _collectableVol = 1f, _enemyBitVol = 1, _enemyZapVol = 1, _pemmingDefeatVol = 1;
    [SerializeField] float _collectableBasePitch = 1f, _collectablePitchIncrease = 0.1f, _collectableMaxPitch = 2.5f, _resetPitchLimit = 1.25f;

    bool _isGamePaused, _isTimerStarted;
    float _pitchTimer;

    void OnEnable()
    {
        VolumeControl.OnPauseStateChanged += TogglePausedState;

        EelController.OnEelHurtSFX += PlaySound;
        EelController.OnEelElectrifiedSFX += PlaySound;

        PemmingController.OnHurtSFX += PlaySound;
        PemmingController.OnDefeat += PlayPemmingPlayerDefeat;

        Collectable.OnAnyCollectableCollected += PlayCollectableSFX;
        Enemy.OnEnemyBit += PlayEnemyBitSFX;
        Enemy.OnEnemyZapped += PlayEnemyZapSFX;

        Pemming.OnPemmingDefeat += PlayPemmingDefeat;
    }

    void OnDisable()
    {
        VolumeControl.OnPauseStateChanged -= TogglePausedState;

        EelController.OnEelHurtSFX -= PlaySound;
        EelController.OnEelElectrifiedSFX -= PlaySound;

        PemmingController.OnHurtSFX -= PlaySound;
        PemmingController.OnDefeat -= PlayPemmingPlayerDefeat;

        Collectable.OnAnyCollectableCollected -= PlayCollectableSFX;
        Enemy.OnEnemyBit -= PlayEnemyBitSFX;
        Enemy.OnEnemyZapped -= PlayEnemyZapSFX;

        Pemming.OnPemmingDefeat -= PlayPemmingDefeat;
    }

    void Update()
    {
        if(!_isTimerStarted) { return; }

        _pitchTimer += Time.deltaTime;

        if(_pitchTimer >= _resetPitchLimit)
        {
            _pitchedAudioSource.pitch = _collectableBasePitch;
            _pitchTimer = 0;
            _isTimerStarted = false;
        }
    }

    void TogglePausedState(bool isPaused)
    {
        _isGamePaused = isPaused;
    }

    void PlaySound(AudioClip clip, float volume)
    {
        if(!clip) { return; }
        if(_isGamePaused) { return; }

        _audioSource.PlayOneShot(clip, volume);
    }

    void PlaySoundWithPitch(AudioClip clip, float volume)
    {
        if(!clip) { return; }
        if(_isGamePaused) { return; }

        _pitchedAudioSource.PlayOneShot(clip, volume);
    }

    void PlayCollectableSFX(int _)
    {
        if(!_isTimerStarted)
        {
            _isTimerStarted = true;
            _pitchedAudioSource.pitch = _collectableBasePitch;
        }
        _pitchTimer = 0;

        PlaySoundWithPitch(_collectableSFX, _collectableVol);
        _pitchedAudioSource.pitch = Mathf.Min(_pitchedAudioSource.pitch + _collectablePitchIncrease, _collectableMaxPitch);
    }

    void PlayEnemyBitSFX()
    {
        PlaySound(_enemyBitSFX, _enemyBitVol);
    }

    void PlayEnemyZapSFX()
    {
        PlaySound(_enemyZapSFX, _enemyZapVol);
    }

    void PlayPemmingDefeat(Vector2 _)
    {
        PlaySound(_pemmingDefeatSFX, _pemmingDefeatVol);
    }

    void PlayPemmingPlayerDefeat()
    {
        PlaySound(_pemmingDefeatSFX, _pemmingDefeatVol);
    }
}
