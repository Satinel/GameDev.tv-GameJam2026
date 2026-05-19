using UnityEngine;

public class StoryTeller : MonoBehaviour
{
    [SerializeField] TextAsset _dialogue;

    void Start()
    {
        if(_dialogue)
            Debug.Log(_dialogue);
    }
}
