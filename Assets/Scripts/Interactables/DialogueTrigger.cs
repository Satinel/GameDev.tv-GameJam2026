using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] StoryTeller _storyTeller;
    [SerializeField] TextAsset _story;

    bool _storyPlayed;

    void Start()
    {
        if(!_storyTeller)
        {
            _storyTeller = FindFirstObjectByType<StoryTeller>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_storyTeller) { return; }

        if(collision.CompareTag("Player"))
        {
            if(_story && !_storyPlayed)
            {
                _storyPlayed = true;
                _storyTeller.StartSideStory(_story);
            }
            else if(_storyTeller.CheckForStory())
            {
                _storyTeller.BeginStory(true);
            }
        }
    }
}
