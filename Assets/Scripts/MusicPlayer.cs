using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _victoryMusic;
    [Range(0, 1)]
    [SerializeField] float _victoryVol = 1;

    void OnEnable()
    {
        LevelManager.OnLevelStarted += PlayMusic;
        LevelManager.OnLevelFinished += PlayVictory;
        // TODO : Defeat Music
        VolumeControl.OnPauseStateChanged += OnPauseStateChanged;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= PlayMusic;
        LevelManager.OnLevelFinished -= PlayVictory;
        VolumeControl.OnPauseStateChanged -= OnPauseStateChanged;
    }

    void PlayMusic()
    {
        _audioSource.Play();
    }

    void PlayVictory()
    {
        _audioSource.Stop();

        if(_victoryMusic)
        {
            _audioSource.PlayOneShot(_victoryMusic, _victoryVol);
        }
    }

    void OnPauseStateChanged(bool state)
    {
        if(state)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.UnPause();
        }
    }
}
