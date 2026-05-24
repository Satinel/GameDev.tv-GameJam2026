using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryTeller : MonoBehaviour
{
    public static event Action OnStoryCanvasClosed, OnStoryStarted, OnStoryCompleted;

    [SerializeField] GameProgressSO _gameProgress;
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _continueButton;
    [SerializeField] Image _leftCharacter, _rightCharacter;
    [SerializeField] TextMeshProUGUI _text, _leftNameText, _rightNameText;
    [SerializeField] GameObject _leftNamePlate, _rightNamePlate;
    [SerializeField] TextAsset[] _dialogues;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] float _textDelay = 0.25f, _pitchChange = 0.1f;
    [SerializeField] char _linebreakSymbol = '~', _pauseSymbol = '^';
    [SerializeField] IntReferenceSO _totalFishtEaten;
    [SerializeField] TextAsset _pacifistStory;
    WaitForSecondsRealtime _textDelayWait = new(0.05f), _textDelayDecaWait = new(0.5f);
    Story _story;
    string _fullLine;
    int _dialogueIndex;
    Coroutine _typingRoutine;

    [SerializeField] CharacterSO _emptyChar, _pemmingChar, _eelChar, _fishChar, _swordChar, _tigeyChar, _redChar, _claraChar, _boneChar;
    CharacterSO _currentCharacter;
    bool _isLeftAligned;
    int _spriteIndex = 0;

    const string CHARACTER_TAG = "character", LAYOUT_TAG = "layout", OPPOSITE_TAG = "opposite";
    const string PEMMING_NAME = "Pemming", EEL_NAME = "Eel", FISH_NAME = "Fish", SWORD_NAME = "Sword", TIGEY_NAME = "Tigey", RED_NAME = "Red", CLARA_NAME = "Clara", BONE_NAME="Bone";//, MOLE_NAME = "something..."

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
        _textDelayWait = new(_textDelay);
        _textDelayDecaWait = new(_textDelay * 10);
        _text.text = string.Empty;
    }

    void OnUnpausedWithStoryActive()
    {
        OnPauseStateChanged(true);
    }

    void OnPauseStateChanged(bool isPaused) // TODO (someday but not during jam period) Pause coroutines when game paused and don't allow story to continue (such as via Jump input)
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
        int index = SceneManager.GetActiveScene().buildIndex;

        if(IsStoryAlreadyViewed(index))  // AND THIS is the refactor which made my decision worthwhile!
        {
            return false;
        }

        if(index == 6 && _totalFishtEaten.Value < 1 && _pacifistStory)
        {
            _story = new(_pacifistStory.text);

            if(_story.canContinue)
            {
                return true;
            }
        }
        else if(_dialogues.Length > _dialogueIndex)
        {
            _story = new(_dialogues[_dialogueIndex].text);

            if(_story.canContinue)
            {
                return true;
            }
        }

        return false;
    }

    bool IsStoryAlreadyViewed(int index)
    {
        return _gameProgress.CheckStoryViewedByBuildIndex(index);
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
        if(_typingRoutine != null)
        {
            StopCoroutine(_typingRoutine);
            _typingRoutine = null;
            SkipTyping();
            return;
        }

        if(_story.canContinue)
        {
            _fullLine = _story.Continue();
            HandleTags();
            _typingRoutine = StartCoroutine(TextDisplayRoutine());
        }
        else
        {
            _canvas.enabled = false;
            OnStoryCompleted?.Invoke();
            OnStoryCanvasClosed?.Invoke();
            _text.text = string.Empty;
            return;
        }
    }

    IEnumerator TextDisplayRoutine()
    {
        _text.text = string.Empty;
        _spriteIndex = 0;

        HandleCharacterTalking();

        foreach(char letter in _fullLine.ToCharArray())
        {
            if(letter == _linebreakSymbol)
            {
                _text.text += "\n";
                HandleCharacterTalking();
            }
            else if(letter == _pauseSymbol)
            {
                yield return _textDelayDecaWait;
            }
            else
            {
                _text.text += letter;
                if(letter == ' ')
                {
                    HandleCharacterTalking();
                }
                yield return _textDelayWait;
            }
        }

        _typingRoutine = null;

        if(_isLeftAligned)
        {
            _leftCharacter.sprite = _currentCharacter.LeftSprites[0];
        }
        else
        {
            _rightCharacter.sprite = _currentCharacter.RightSprites[0];
        }
    }

    void HandleCharacterTalking()
    {
        if(_isLeftAligned)
        {
            _spriteIndex = _currentCharacter.LeftSprites.Length - 1 > _spriteIndex ? _spriteIndex + 1 : 0;  // _spriteIndex++ doesn't work but ++_spriteIndex does
            _leftCharacter.sprite = _currentCharacter.LeftSprites[_spriteIndex];
        }
        else
        {
            _spriteIndex = _currentCharacter.RightSprites.Length - 1 > _spriteIndex ? _spriteIndex + 1 : 0;
            _rightCharacter.sprite = _currentCharacter.RightSprites[_spriteIndex];
        }

        if(!_audioSource.isPlaying)
        {
            if(_currentCharacter.SoundFX)
            {
                _audioSource.pitch = UnityEngine.Random.Range(1f - _pitchChange, 1f + _pitchChange);
                _audioSource.PlayOneShot(_currentCharacter.SoundFX, _currentCharacter.SoundVol);
            }
        }
    }

    void SkipTyping()
    {
        _text.text = string.Empty;

        foreach(char letter in _fullLine.ToCharArray())
        {
            if(letter == _linebreakSymbol)
            {
                _text.text += "\n";
            }
            else if(letter == _pauseSymbol)
            {
                continue;
            }
            else
            {
                _text.text += letter;
            }
        }

        if(_isLeftAligned)
        {
            _leftCharacter.sprite = _currentCharacter.LeftSprites[0];
        }
        else
        {
            _rightCharacter.sprite = _currentCharacter.RightSprites[0];
        }
    }

    void HandleTags()
    {
        List<string> currentTags = _story.currentTags;

        _isLeftAligned = true;
        _currentCharacter = _emptyChar;
        bool oppositeSpeaker = false;

        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2) { Debug.LogError("You tried to get fancy with tag splitting didn't you?");}

            string tagKey = splitTag[0].Trim(), tagValue = splitTag[1].Trim();

            if(tagKey == LAYOUT_TAG)
            {
                _isLeftAligned = tagValue == "left";
            }

            if(tagKey == CHARACTER_TAG)
            {
                _currentCharacter = tagValue switch
                {
                    PEMMING_NAME => _pemmingChar,
                    EEL_NAME => _eelChar,
                    FISH_NAME => _fishChar,
                    SWORD_NAME => _swordChar,
                    TIGEY_NAME => _tigeyChar,
                    RED_NAME => _redChar,
                    CLARA_NAME => _claraChar,
                    BONE_NAME => _boneChar,
                    _ => _emptyChar,
                };
            }

            if(tagKey == OPPOSITE_TAG)
            {
                bool.TryParse(tagValue, out oppositeSpeaker);
            }
        }

        SetNamePlate(oppositeSpeaker);
    }

    void SetNamePlate(bool oppositeSpeaker)
    {
        if(_isLeftAligned)
        {
            _rightNamePlate.SetActive(false);
            _rightCharacter.enabled = oppositeSpeaker;
            _rightCharacter.color = Color.gray3;
            _leftNameText.text = _currentCharacter.CharacterName;
            _leftNamePlate.SetActive(true);
            _leftCharacter.color = Color.white;
            _leftCharacter.enabled = true;
            _leftCharacter.sprite = _currentCharacter.LeftSprites[0];
        }
        else
        {
            _leftNamePlate.SetActive(false);
            _leftCharacter.enabled = oppositeSpeaker;
            _leftCharacter.color = Color.gray3;
            _rightNameText.text = _currentCharacter.CharacterName;
            _rightNamePlate.SetActive(true);
            _rightCharacter.color = Color.white;
            _rightCharacter.enabled = true;
            _rightCharacter.sprite = _currentCharacter.RightSprites[0];
        }
    }
}
