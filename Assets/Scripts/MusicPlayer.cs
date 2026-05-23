using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _victoryMusic, _defeatMusic;
    [Range(0, 1)]
    [SerializeField] float _victoryVol = 1, _defeatVol;

    void OnEnable()
    {
        LevelManager.OnLevelStarted += PlayMusic;
        LevelManager.OnLevelFinished += PlayVictory;
        EelController.OnEelDefeat += PlayDefeat;
        PemmingController.OnDefeat += PlayDefeat;
        VolumeControl.OnPauseStateChanged += OnPauseStateChanged;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= PlayMusic;
        LevelManager.OnLevelFinished -= PlayVictory;
        EelController.OnEelDefeat -= PlayDefeat;
        PemmingController.OnDefeat -= PlayDefeat;
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

    void PlayDefeat()
    {
        _audioSource.Stop();

        if(_defeatMusic)
        {
            _audioSource.PlayOneShot(_defeatMusic, _defeatVol);
        }
    }

    void OnPauseStateChanged(bool isPaused)
    {
        if(isPaused)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.UnPause();
        }
    }
}
