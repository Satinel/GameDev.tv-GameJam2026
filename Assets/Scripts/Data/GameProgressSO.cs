using UnityEngine;

[CreateAssetMenu(fileName = "GameProgressSO", menuName = "Scriptable Objects/GameProgressSO")]
public class GameProgressSO : ScriptableObject
{
    [field:SerializeField] public bool Scene1StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene2StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene3StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene4StoryViewed { get; private set; }
    [field:SerializeField] public bool Scene5StoryViewed { get; private set; }
    [field:SerializeField] public bool YemmepEaten { get; private set; }

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
            _ => false,
        };
    }

    public void SetYemmepEaten()
    {
        YemmepEaten = true;
    }

    public void Reset()
    {
        Scene1StoryViewed = false;
        Scene2StoryViewed = false;
        Scene3StoryViewed = false;
        Scene4StoryViewed = false;
        Scene5StoryViewed = false;

        YemmepEaten = false;
    }
}
