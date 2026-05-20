using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;

public class StoryTeller : MonoBehaviour
{
    public static event Action OnStoryCanvasClosed, OnStoryStarted;

    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _continueButton;
    [SerializeField] Image _leftCharacter, _leftCharacterAlt, _rightCharacter, _rightCharacterAlt;
    [SerializeField] TextMeshProUGUI _text, _leftNameText, _rightNameText;
    [SerializeField] GameObject _leftNamePlate, _rightNamePlate;
    [SerializeField] TextAsset[] _dialogues;
    Story _story;
    int _dialogueIndex;

    [SerializeField] CharacterSO _emptyChar, _pemmingChar, _eelChar, _fishChar, _tigeyChar, _redChar;

    const string _noContentString = "NO CONTENT FOUND!";
    const string CHARACTER_TAG = "character", LAYOUT_TAG = "layout", OPPOSITE_TAG = "opposite";
    const string PEMMING_NAME = "Pemming", EEL_NAME = "Eel", FISH_NAME = "Fish", TIGEY_NAME = "Tigey", RED_NAME = "Red";//, MOLE_NAME = "something..."

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
            HandleTags();
        }
        else
        {
            _canvas.enabled = false;
            OnStoryCanvasClosed?.Invoke();
            _text.text = _noContentString;
            return;
        }
    }

    void HandleTags()
    {
        List<string> currentTags = _story.currentTags;

        bool isLeftAligned = true;
        CharacterSO character = _emptyChar;
        bool oppositeSpeaker = false;

        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2) { Debug.LogError("You tried to get fancy with tag splitting didn't you?");}

            string tagKey = splitTag[0].Trim(), tagValue = splitTag[1].Trim();

            if(tagKey == LAYOUT_TAG)
            {
                isLeftAligned = tagValue == "left";
            }

            if(tagKey == CHARACTER_TAG)
            {
                character = tagValue switch
                {
                    PEMMING_NAME => _pemmingChar,
                    EEL_NAME => _eelChar,
                    FISH_NAME => _fishChar,
                    TIGEY_NAME => _tigeyChar,
                    RED_NAME => _redChar,
                    _ => _emptyChar,
                };
            }

            if(tagKey == OPPOSITE_TAG)
            {
                bool.TryParse(tagValue, out oppositeSpeaker);
            }
        }

        SetNamePlate(isLeftAligned, character, oppositeSpeaker);
    }

    void SetNamePlate(bool isLeftAligned, CharacterSO character, bool oppositeSpeaker)
    {
        if(isLeftAligned)
        {
            _rightNamePlate.SetActive(false);
            _rightCharacter.enabled = oppositeSpeaker;
            _rightCharacter.color = Color.gray3;
            _leftNameText.text = character.CharacterName;
            _leftNamePlate.SetActive(true);
            _leftCharacter.color = Color.white;
            _leftCharacter.enabled = true;
            _leftCharacter.sprite = character.LeftSprites[0];
            // _leftCharacterAlt.sprite = sprite1;
        }
        else
        {
            _leftNamePlate.SetActive(false);
            _leftCharacter.enabled = oppositeSpeaker;
            _leftCharacter.color = Color.gray3;
            _rightNameText.text = character.CharacterName;
            _rightNamePlate.SetActive(true);
            _rightCharacter.color = Color.white;
            _rightCharacter.enabled = true;
            _rightCharacter.sprite = character.RightSprites[0];
            // _rightCharacterAlt.sprite = sprite1;
        }
    }
}
