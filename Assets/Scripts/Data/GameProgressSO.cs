using UnityEngine;

[CreateAssetMenu(fileName = "GameProgressSO", menuName = "Scriptable Objects/GameProgressSO")]
public class GameProgressSO : ScriptableObject
{
    [field:SerializeField] public bool Scene1StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene2StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene3StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene4StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene5StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene6StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene7StoryViewed { get; private set; }
    [field:SerializeField] public bool YemmepEaten { get; private set; }
    [field:SerializeField] public bool CutsceneViewed { get; private set; }
    [field:SerializeField] public bool TigeyMet { get; private set; }

    [SerializeField] IntReferenceSO _score, _fishEaten, _fishZapped, _health, _speed, _noseA, _noseD, _otherA, _otherD;

    public void SetStoryViewedByBuildIndex(int index)
    {
        switch(index)
        {
            case 1:
                Scene1StoryViewed = true;
                break;
            case 2:
                Scene2StoryViewed = true;
                break;
            case 3:
                Scene3StoryViewed = true;
                break;
            case 4:
                Scene4StoryViewed = true;
                break;
            case 5:
                Scene5StoryViewed = true;
                break;
            case 6:
                Scene6StoryViewed = true;
                break;
            case 7:
                Scene7StoryViewed = true;
                break;
            default:
                break;
        }
    }

    public bool CheckStoryViewedByBuildIndex(int index)
    {
        return index switch
        {
            1 => Scene1StoryViewed,
            2 => Scene2StoryViewed,
            3 => Scene3StoryViewed,
            4 => Scene4StoryViewed,
            5 => Scene5StoryViewed,
            6 => Scene6StoryViewed,
            7 => Scene7StoryViewed,
            _ => false,
        };
    }

    public void SetYemmepEaten()
    {
        YemmepEaten = true;
    }

    public void SetCutsceneViewed()
    {
        CutsceneViewed = true;
    }

    public void SetTigeyMet()
    {
        TigeyMet = true;
    }

    public void Reset()
    {
        Scene1StoryViewed = false;
        Scene2StoryViewed = false;
        Scene3StoryViewed = false;
        Scene4StoryViewed = false;
        Scene5StoryViewed = false;
        Scene6StoryViewed = false;
        Scene7StoryViewed = false;

        YemmepEaten = false;
        CutsceneViewed = false;
        TigeyMet = false;

        _score.Reset();
        _fishEaten.Reset();
        _fishZapped.Reset();
        _health.Reset();
        _speed.Reset();
        _noseA.Reset();
        _noseD.Reset();
        _otherA.Reset();
        _otherD.Reset();
    }
}
