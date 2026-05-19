using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class StoryTeller : MonoBehaviour
{
    public static event Action OnStoryCanvasClosed, OnStoryStarted;

    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _continueButton;
    [SerializeField] Image _leftCharacter, _rightCharacter;
    [SerializeField] TextMeshProUGUI _text, _leftNameText, _rightNameText;
    [SerializeField] TextAsset[] _dialogues;
    Story _story;
    int _dialogueIndex;

    const string _noContentString = "NO CONTENT FOUND!";

    void OnEnable()
    {
        InputManager.OnJumpAction += ContinueStory;
        VolumeControl.OnPauseStateChanged += OnPauseStateChanged;
        VolumeControl.OnUnpausedWithStoryActive += OnUnpausedWithStoryActive;
    }

    void OnDisable()
    {
        InputManager.OnJumpAction -= ContinueStory;
        VolumeControl.OnPauseStateChanged -= OnPauseStateChanged;
        VolumeControl.OnUnpausedWithStoryActive -= OnUnpausedWithStoryActive;
    }

    void Start()
    {
        _text.text = _noContentString;
    }

    void OnUnpausedWithStoryActive()
    {
        OnPauseStateChanged(true);
    }

    void OnPauseStateChanged(bool isPaused)
    {
        if(_canvas.isActiveAndEnabled)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_continueButton);
        }
    }

    public void StartSideStory(TextAsset sideStory)
    {
        _story = new(sideStory.text);
        if(_story.canContinue)
        {
            BeginStory(false);
        }
    }

    public bool CheckForStory() // There's some argument to be made to simply call BeginStory instead of returning a bool but I might want to refactor things in other ways
    {
        if(_dialogues.Length > _dialogueIndex)
        {
            _story = new(_dialogues[_dialogueIndex].text);
            if(_story.canContinue)
            return true;
        }

        return false;
    }

    public void BeginStory(bool increaseIndex)
    {
        OnStoryStarted?.Invoke();
        _canvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_continueButton);
        if(increaseIndex)
        {
            _dialogueIndex++;
        }
        ContinueStory();
    }

    public void ContinueStory()
    {
        if(_story.canContinue)
        {
            _text.text = _story.Continue();
        }
        else
        {
            _canvas.enabled = false;
            OnStoryCanvasClosed?.Invoke();
            _text.text = _noContentString;
            return;
        }
    }
}
