using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelStarted, OnLevelFinished;

    [SerializeField] Canvas _startCanvas, _winCanvas;
    [SerializeField] GameObject _startButton, _nextLevelButton;
    [SerializeField] Transform[] _rendererParents;
    [SerializeField] float _startWait = 0.25f;
    [SerializeField] SceneTransitions _sceneTransitions;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] float _minPitch = 0.5f, _pitchIncrease = 0.1f;

    WaitForSeconds _startWaitDelay = new(0.1f);

    void Awake()
    {
        _startWaitDelay = new(_startWait);
    }

    void OnEnable()
    {
        Goal.OnGoalAchieved += Goal_OnGoalAchieved;
    }

    void OnDisable()
    {
        Goal.OnGoalAchieved -= Goal_OnGoalAchieved;
    }

    IEnumerator Start()
    {
        List<SpriteRenderer>[] itemsToEnable = new List<SpriteRenderer>[_rendererParents.Length];

        for(int i = 0; i < _rendererParents.Length; i++)
        {
            var sprites = _rendererParents[i].GetComponentsInChildren<SpriteRenderer>(true);
            itemsToEnable[i] = new List<SpriteRenderer>(sprites);
            foreach(SpriteRenderer renderer in sprites)
            {
                renderer.enabled = false;
            }
        }

        int maxIndex = 0;

        foreach(var item in itemsToEnable)
        {
            if(item.Count > maxIndex)
            {
                maxIndex = item.Count;
            }
        }

        PlayAudioSource();

        for(int i = 0; i < maxIndex; i++)
        {
            foreach(List<SpriteRenderer> list in itemsToEnable)
            {
                if(list.Count > i)
                {
                    list[i].enabled = true;
                    if(list[i].transform.parent.TryGetComponent(out Collectable collectable))
                    {
                        collectable.SetAppearTrigger();
                    }
                }
            }
            _audioSource.pitch += _pitchIncrease;
            yield return _startWaitDelay;
        }

        StopAudioSource();
        yield return null;

        LevelReady();
    }

    void PlayAudioSource()
    {
        if(_audioSource.isPlaying) { return; }

        _audioSource.pitch = _minPitch;
        _audioSource.Play();
    }

    void StopAudioSource()
    {
        _audioSource.Stop();
        _audioSource.pitch = _minPitch;
    }


    void LevelReady()
    {
        _startCanvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    void Goal_OnGoalAchieved()
    {
        OnLevelFinished?.Invoke();
        _winCanvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_nextLevelButton);
    }

    public void StartLevel()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _startCanvas.enabled = false;
        OnLevelStarted?.Invoke();
    }

    public void NextLevel()
    {
        _winCanvas.enabled = false;
        _sceneTransitions.RequestNextLevel();
    }

    public void RestartLevel()
    {
        _winCanvas.enabled = false;
        _sceneTransitions.RequestRestartLevel();
    }
}
