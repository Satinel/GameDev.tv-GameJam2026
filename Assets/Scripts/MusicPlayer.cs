using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _victoryMusic, _defeatMusic;
    [Range(0, 1)]
    [SerializeField] float _victoryVol = 1f, _defeatVol = 1f;
    [SerializeField] bool _playOnLevelReady;
    float _startingVolume, _halfVolume;

    void OnEnable()
    {
        LevelManager.OnLevelReady += MaybePlayMusic;
        LevelManager.OnLevelStarted += PlayMusic;
        LevelManager.OnLevelFinished += PlayVictory;
        EelController.OnEelDefeat += PlayDefeat;
        PemmingController.OnDefeat += PlayDefeat;
        VolumeControl.OnEnableAudioCanvas += OnEnableAudioCanvas;
        VolumeControl.OnDisableAudioCanvas += OnDisableAudioCanvas;
        StoryTeller.OnStoryStarted += HalveVolume;
        StoryTeller.OnStoryCompleted += RestoreVolume;
    }

    void OnDisable()
    {
        LevelManager.OnLevelReady -= MaybePlayMusic;
        LevelManager.OnLevelStarted -= PlayMusic;
        LevelManager.OnLevelFinished -= PlayVictory;
        EelController.OnEelDefeat -= PlayDefeat;
        PemmingController.OnDefeat -= PlayDefeat;
        VolumeControl.OnEnableAudioCanvas -= OnEnableAudioCanvas;
        VolumeControl.OnDisableAudioCanvas -= OnDisableAudioCanvas;
        StoryTeller.OnStoryStarted -= HalveVolume;
        StoryTeller.OnStoryCompleted -= RestoreVolume;
    }

    void Start()
    {
        _startingVolume = _audioSource.volume;
        _halfVolume = _startingVolume * 0.5f;
    }

    void MaybePlayMusic()
    {
        if(_playOnLevelReady)
        {
            PlayMusic();
        }
    }

    void PlayMusic()
    {
        if(!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    void PlayVictory()
    {

        if(_victoryMusic)
        {
            _audioSource.Stop();
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

    void OnEnableAudioCanvas()
    {
        _audioSource.Pause();
    }

    void OnDisableAudioCanvas()
    {
        _audioSource.UnPause();
    }

    void HalveVolume()
    {
        _audioSource.volume = _halfVolume;
    }

    void RestoreVolume()
    {
        _audioSource.volume = _startingVolume;
    }
}
