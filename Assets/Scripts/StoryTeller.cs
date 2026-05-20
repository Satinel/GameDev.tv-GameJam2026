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

    [SerializeField] Sprite[] _portraitSprites; // This is an awful way to do things but given the time constraints of a game jam I'm excusing it

    const string _noContentString = "NO CONTENT FOUND!";
    const string SPEAKER_TAG = "speaker", PORTRAIT_TAG = "portrait", LAYOUT_TAG = "layout", OPPOSITE_TAG = "opposite";
    const string PEMMING_NAME = "Pemmy", EEL_NAME = "Electra", FISH_NAME = "Bitey", TIGEY_NAME = "Tigey";//, MOLE_NAME = "something..."

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
        string speakerName = "???";
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

            if(tagKey == SPEAKER_TAG)
            {
                speakerName = tagValue;
            }

            if(tagKey == OPPOSITE_TAG)
            {
                bool.TryParse(tagValue, out oppositeSpeaker);
            }
        }

        SetNamePlate(isLeftAligned, speakerName, oppositeSpeaker);
    }

    void SetNamePlate(bool isLeftAligned, string speakerName, bool oppositeSpeaker)
    {
        GetSprites(speakerName, out Sprite sprite0, out Sprite sprite1);

        if(isLeftAligned)
        {
            _rightNamePlate.SetActive(false);
            _rightCharacter.enabled = oppositeSpeaker;
            _rightCharacter.color = Color.gray3;
            _leftNameText.text = speakerName;
            _leftNamePlate.SetActive(true);
            _leftCharacter.color = Color.white;
            _leftCharacter.enabled = true;
            _leftCharacter.sprite = sprite0;
            _leftCharacterAlt.sprite = sprite1;
        }
        else
        {
            _leftNamePlate.SetActive(false);
            _leftCharacter.enabled = oppositeSpeaker;
            _leftCharacter.color = Color.gray3;
            _rightNameText.text = speakerName;
            _rightNamePlate.SetActive(true);
            _rightCharacter.color = Color.white;
            _rightCharacter.enabled = true;
            _rightCharacter.sprite = sprite0;
            _rightCharacterAlt.sprite = sprite1;
        }
    }

    void GetSprites(string value, out Sprite sprite1, out Sprite sprite2)
    {
        switch(value)
        {
            case PEMMING_NAME:
                sprite1 = _portraitSprites[0];
                sprite2 = _portraitSprites[1];
                break;
            case EEL_NAME:
                sprite1 = _portraitSprites[2];
                sprite2 = _portraitSprites[3];
                break;
            case FISH_NAME:
                sprite1 = _portraitSprites[4];
                sprite2 = _portraitSprites[4];
                break;
            case TIGEY_NAME:
                sprite1 = _portraitSprites[5];
                sprite2 = _portraitSprites[5];
                break;
            default:
                sprite1 = _portraitSprites[0];
                sprite2 = _portraitSprites[1];
                break;
        }
    }
}
