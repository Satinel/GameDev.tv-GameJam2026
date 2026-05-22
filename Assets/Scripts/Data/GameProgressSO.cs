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
}
