using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class StoryTeller : MonoBehaviour
{
    public static event Action OnStoryCanvasClosed;

    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _continueButton;
    [SerializeField] Image _leftCharacter, _rightCharacter;
    [SerializeField] TextMeshProUGUI _text, _leftNameText, _rightNameText;
    [SerializeField] TextAsset _dialogue;
    Story _story;

    const string _noContentString = "NO CONTENT FOUND!";

    void OnEnable()
    {
        InputManager.OnJumpAction += ContinueStory;
        VolumeControl.OnPauseStateChanged += OnPauseStateChanged;
    }

    void OnDisable()
    {
        InputManager.OnJumpAction -= ContinueStory;
        VolumeControl.OnPauseStateChanged -= OnPauseStateChanged;
    }

    void Start()
    {
        _text.text = _noContentString;
        if(_dialogue)
        {
            _story = new(_dialogue.text);
        }
    }

    void OnPauseStateChanged(bool isPaused)
    {
        if(!isPaused && _canvas.isActiveAndEnabled)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_continueButton);
        }
    }

    public bool CheckForStory()
    {
        if(_story && _story.canContinue)
        {
            return true;
        }

        return false;
    }

    public void BeginStory()
    {
        _canvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_continueButton);
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
            return;
        }
    }
}
